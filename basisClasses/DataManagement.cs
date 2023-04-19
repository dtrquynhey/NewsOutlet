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

        //Dictionary of all the news we load(the two sub files)
        public Dictionary<int, News>? dictOfAllNews = new Dictionary<int, News>();

        //Dictionary of the 10 news that have the most hits
        public Dictionary<int, News>? dictOfNewsByTrend = filesProcess.ReadFile("newsByTrend.json");

        //Dictionary of the latest 10 news
        public Dictionary<int, News>? dictOfNewsByTime = filesProcess.ReadFile("newsByTime.json")/*FillDictNewsByTime()*/;

        //PriorityQueue for the trending news
        public PriorityQueue<News, int>? pQueueOfNewsByTrend = new PriorityQueue<News, int>();

        //Stack to review the selected news
        public Stack<int> selectedNews = new Stack<int>();

        //Fill the dictinaryNewsByTime by reading from the file
        public void FillDictNewsByTime() // O(n)
        {
            dictOfNewsByTime =  filesProcess.ReadFile("newsByTime.json");
        }

        //Fill the PQueue of trending news from the sub file newsByTrend
        public void FillPQueue() // O(n log n)
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

        //Fill the Dictionary of all news from the sub files
        public void FillDictionary() // O(n + m)
        {
            if (dictOfNewsByTrend != null && dictOfAllNews != null && dictOfNewsByTime != null)
            {
                foreach (var item in dictOfNewsByTrend.Values) //O(n)
                {
                    dictOfAllNews.Add(item.ID, new News(item)); // O(1)

                }
                foreach (var item in dictOfNewsByTime.Values) //O(m)
                {
                    if (!dictOfAllNews.ContainsKey(item.ID))
                    {
                        dictOfAllNews.Add(item.ID, new News(item));
                    }
                }
            }
        }

        //Update the hits of news in all the data structures
        private void UpdateDict(int id) // O(n log n)
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
            FillPQueue(); // O(n log n)
        }

        // SELECT
        public void Select(int id) // O(n log n)
        {
            News news = dictOfAllNews[id];
            dictOfAllNews[id].Hits += 1;
            UpdateDict(id); // O(n log n)
            ShowContent(news);
        }

        public void PushNewsToStack(int id) //O(1)
        {
            selectedNews.Push(id);
        }

        public int PopNewsFromStack() //O(1)
        {
            if (selectedNews.Count > 0)
            {
                return selectedNews.Pop();
            }
            return -1;
        }

        // SHOW RECENT
        public void DisplayRecentNews() //O(n)
        {
            if (dictOfNewsByTime != null)
            {
                foreach (News news in dictOfNewsByTime.Values)
                {
                    Console.WriteLine(news);
                }
            }
        }

        public void DisplayRecentNewsByKeywords(string[] keywords) // O(nm)
        {
            string allNews = "";
            if (dictOfNewsByTime != null)
            {
                foreach (News news in dictOfNewsByTime.Values)// O(n)
                {
                    for (int i = 0; i < keywords.Length; i++)// O(m)
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
        public void DisplayTrendNews() //O(n log n)
        {
            if (pQueueOfNewsByTrend != null)
            {
                while (pQueueOfNewsByTrend.Count > 0)
                {
                    Console.WriteLine(pQueueOfNewsByTrend.Dequeue());
                }
            }
        }

        public void DisplayTrendNewsByKeywords(string[] keywords) //O(nm log n)
        {
            string allNews = "";
            if (pQueueOfNewsByTrend != null)
            {
                while (pQueueOfNewsByTrend.Count > 0)
                {
                    for (int i = 0; i < keywords.Length; i++)
                    {
                        News currentNews = pQueueOfNewsByTrend.Dequeue();
                        if (currentNews.Keywords.Contains(keywords[i]))
                        {
                            allNews += currentNews.ToString() + "\n";
                        }
                    }
                }
            }
            Console.WriteLine(allNews);
        }

        public void DisplayAllNews() // O(n)
        {
            if (dictOfAllNews != null)
            {
                foreach (News news in dictOfAllNews.Values)
                {
                    Console.WriteLine(news);
                }
            }
        }

        public void ShowContent(News news) // O(1)
        {
            Console.WriteLine("--> News Info " + news.ToString() + "\nContent: " + news.Content);
        }
    }
}