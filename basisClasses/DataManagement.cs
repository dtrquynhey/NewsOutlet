using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsOutlet.basisClasses
{
    class DataManagement
    {
        static FilesProcess filesProcess = new FilesProcess();

        //Dictionary of all the news we load
        public Dictionary<int, News>? dictOfAllNews = new Dictionary<int, News>();

        //Dictionary of the 500 news that have the most hits
        public Dictionary<int, News>? dictOfNewsByTrend = filesProcess.ReadFile("newsByTrend.json");

        //Dictionary of the latest 500 news
        public Dictionary<int, News>? dictOfNewsByTime = fillDictNewsByTime();

        //PriorityQueue for the trending news
        public PriorityQueue<News, int>? pQueueOfNewsByTrend = new PriorityQueue<News, int>();

        //Stack to review the selected news
        public Stack<int> selectedNews = new Stack<int>();

       
        public static Dictionary<int, News> fillDictNewsByTime()
        {
            return filesProcess.ReadFile("newsByTime.json");
        }


        public void fillPQueue()
        {
            if (pQueueOfNewsByTrend != null)
            {
                pQueueOfNewsByTrend.Clear();
                if (dictOfNewsByTrend != null)
                {
                    foreach (News news in dictOfNewsByTrend.Values)
                    {
                        pQueueOfNewsByTrend.Enqueue(news, -news.Hits);
                    }
                }

            }
        }

        public void fillDictionary()
        {
            if (dictOfNewsByTrend != null && dictOfAllNews != null && dictOfNewsByTime != null)
            {
                foreach (var item in dictOfNewsByTrend.Values)
                {
                    dictOfAllNews.Add(item.ID, new News(item));

                }
                foreach (var item in dictOfNewsByTime.Values)
                {
                    if (!dictOfAllNews.ContainsKey(item.ID))
                    {
                        dictOfAllNews.Add(item.ID, new News(item));
                    }
                }
            }
        }

        public void pushNewsToStack(int id)
        {
            selectedNews.Push(id);
        }

        public int popNewsFromStack()
        {
            if (selectedNews.Count > 0)
            {
                return selectedNews.Pop();
            }
            return -1;
        }

        private void updateDict(int id)
        {
            if (dictOfAllNews != null && dictOfNewsByTrend != null && dictOfNewsByTime != null)
            {
                int newHits = dictOfAllNews[id].Hits;
                if (dictOfNewsByTrend.ContainsKey(id))
                {
                    dictOfNewsByTrend[id].Hits = newHits;
                }
                if (dictOfNewsByTime.ContainsKey(id))
                {
                    dictOfNewsByTime[id].Hits = newHits;
                }
               
            }

            fillPQueue();
        }

        // SELECT
        public void Select(int id)
        {
                News news = dictOfAllNews[id];
                dictOfAllNews[id].Hits += 1;
                updateDict(id);
                showContent(news);
           
        }

        // SHOW RECENT
        
        public void displayRecentNews()
        {
            if (dictOfNewsByTime != null)
            {
                foreach (News news in dictOfNewsByTime.Values)
                {
                        Console.WriteLine(news);   
                }
            }
        }

        public void displayRecentNewsByKeywords(string[] keywords)
        {
            string allNews = "";
            if (dictOfNewsByTime != null)
            {
                foreach (News news in dictOfNewsByTime.Values)
                {

                    for (int i = 0; i < keywords.Length; i++)
                    {
                        if (news.Keywords.Contains(keywords[i]))
                        {
                            allNews += news.ToString() + "\n";

                        }
                    }
                }
            }
            Console.WriteLine(allNews); 
        }


        // SHOW TREND
        public void displayTrendNews()
        {
            if (pQueueOfNewsByTrend != null)
            {
                while (pQueueOfNewsByTrend.Count > 0)
                {
                    Console.WriteLine(pQueueOfNewsByTrend.Dequeue());
                }
            }

        }

        public void displayTrendNewsByKeywords(string[] keywords)
        {
            string allNews = "";
            if (dictOfNewsByTrend != null)
            {
                foreach (News news in dictOfNewsByTrend.Values)
                {

                    for (int i = 0; i < keywords.Length; i++)
                    {
                        if (news.Keywords.Contains(keywords[i]))
                        {
                            allNews += news.ToString() + "\n";

                        }
                    }
                }
            }
            Console.WriteLine(allNews);
        }

        public void displayAllNews()
        {
            if (dictOfAllNews != null)
            {
                foreach (News news in dictOfAllNews.Values)
                {
                    Console.WriteLine(news);
                }
            }
        }

        public void showContent(News news)
        {
            Console.WriteLine("--> News Info " + news.ToString() + "\nContent: " + news.Content); 
        }
    }
}