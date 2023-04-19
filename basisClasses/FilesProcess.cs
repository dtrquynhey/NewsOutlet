using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using NewsOutlet.basisClasses;
using System.Collections;



namespace NewsOutlet.basisClasses
{
    class FilesProcess
    {
        //Pqueue for Recent to sort them and write them to the file newsByTime.json
        public PriorityQueue<News, long> pQueueRecent = new PriorityQueue<News, long>();

        //PQueue for Trending, have it sorted and write to the file newsByTrend.json
        public PriorityQueue<News, int> pQueueTrend = new PriorityQueue<News, int>();

        //Dictionary of News, for all the news in given data file
        Dictionary<int, News> dictOfNews = new Dictionary<int, News>();

        //SysDate
        public DateTime sysDate = DateTime.Now;


        //To read from any file and return a dictionary of all the news in that file
        public Dictionary<int, News> ReadFile(string path) // O(n)
        {
            Dictionary<int, News> allNews = new Dictionary<int, News>();
            if (!File.Exists(path))
            {
                Console.WriteLine("File is not found. Make sure that the file exist!");
            }

            try
            {
                string jsonStr = File.ReadAllText(path);
                if (string.IsNullOrEmpty(jsonStr))
                {
                    Console.WriteLine("The file is empty!");
                }

                dynamic? jsonFile = JsonConvert.DeserializeObject(jsonStr); //O(n)

                if (jsonFile == null)
                {
                    Console.WriteLine("The DeserializeObject is null!");
                }
                else
                {
                    for (int i = 0; i < jsonFile.Count; i++)
                    {
                        int id = Convert.ToInt32(jsonFile[i]["ID"]);
                        long time = Convert.ToInt64(jsonFile[i]["Time"]);
                        string content = jsonFile[i]["Content"];
                        string[] keywords = jsonFile[i]["Keywords"].ToObject<string[]>();
                        int hits = Convert.ToInt32(jsonFile[i]["Hits"]);
                        News news = new News(id, time, content, keywords, hits);
                        allNews.Add(news.ID, news);
                    } 
                }
                return allNews;
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not read the file: " + e.Message);
                return allNews;
            }
        }

        //Fill pQueueTrend from the dictionary of all news
        public void FillPQueueTrend(Dictionary<int, News> dictTrend) //O(n log n)
        {
            foreach (News val in dictTrend.Values) // O(n)
            {
                pQueueTrend.Enqueue(val, -val.Hits); // O(log n)
            } 
        }

        //Fill pQueueRecent from the dictionary of all news
        public void FillPQueueRecent(Dictionary<int, News> dictRecent) //O(n log n)
        {
            pQueueRecent.Clear(); //When the user set the time, we have to fill the queue again
            foreach (News val in dictRecent.Values)// O(n)
            {
                if (Last24HoursNews(val)) //O(1)
                {
                    //Console.WriteLine(val);
                    pQueueRecent.Enqueue(val, -val.Time); // O(log n)
                }
            } 
        }

        //To chack if the news was published in the last 24 hours
        public bool Last24HoursNews(News news) // O(1)
        {
            long nowTimestamp = ConvertToUnixEpoch(sysDate);
            long differenceInHours = (nowTimestamp - news.Time) / 3600;
            if (differenceInHours <= 24 && differenceInHours > 0)
            {
                return true;
            }
            return false;
        }

        //To convert date time to Epoch num
        public long ConvertToUnixEpoch(DateTime dateTimeToBeConverted) // O(1)
        {
            TimeSpan timeSpan = dateTimeToBeConverted - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)timeSpan.TotalSeconds;
        }

        //Create a sub json file for trending news
        public void CreateNewsFileByTrend() // O(n log m)
        {
            string fileName = "newsByTrend.json";
            FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
            using (StreamWriter wr = new StreamWriter(fileStream)) // O(1)
            {
                wr.WriteLine("["); 
                for (int i = 0; i < 10 && pQueueTrend.Count > 0; i++) // O(1)
                {
                    string strNews = JsonConvert.SerializeObject(pQueueTrend.Dequeue()); //O(K + log m)
                    wr.Write(strNews);//O(n)
                    if (pQueueTrend.Count == 0)
                    {
                        break;
                    }
                    if (i < 9)
                    {
                        wr.WriteLine(",");
                    }
                    wr.WriteLine();
                }
                wr.WriteLine("]");
            }
        }

        //Create a sub json file for Recent news
        public void CreateNewsFileByTime() // O(n log m)
        {
            string fileName = "newsByTime.json";
            FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
            using (StreamWriter wr = new StreamWriter(fileStream))
            {
                wr.WriteLine("[");
                for (int i = 0; i < 10 && pQueueRecent.Count > 0; i++)
                {
                    string strNews = JsonConvert.SerializeObject(pQueueRecent.Dequeue());
                    wr.Write(strNews);
                    if (pQueueRecent.Count == 0)
                    {
                        break;
                    }
                    if (i < 9)
                    {
                        wr.WriteLine(",");
                    }
                    wr.WriteLine();
                }
                wr.WriteLine("]");
            }
        }

        //Create two sub files and fill the two queue
        public void CreateSubJsonFiles() 
        {
            dictOfNews = ReadFile("MOCK_DATA.json"); // O(n)
            FillPQueueTrend(dictOfNews); // O(n log n)
            FillPQueueRecent(dictOfNews);// O(n log n)

            CreateNewsFileByTrend(); // O(n log m)
            CreateNewsFileByTime(); // O(n log m)
        }


        public void ResetNewsFileByTime()
        {
            string fileName = "newsByTime.json";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            FillPQueueRecent(dictOfNews);// O(n log n)
            CreateNewsFileByTime();// O(n log m)
        }

    }
}