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

        public PriorityQueue<News, long> pQueueRecent = new PriorityQueue<News, long>();
        public PriorityQueue<News, int> pQueueTrend = new PriorityQueue<News, int>();

        public void CreateSubJsonFiles()
        {
            Dictionary<int, News> dictOfNews = ReadFile("MOCK_DATA.json");
            FillPQueueTrend(dictOfNews);
            FillPQueueRecent(dictOfNews);

            CreateNewsFileByTrend();
            CreateNewsFileByTime();
        }

        public void FillPQueueTrend(Dictionary<int, News> dictTrend)
        {
            foreach (News val in dictTrend.Values)
            {
                pQueueTrend.Enqueue(val, -val.Hits);
            }
        }

        /// <summary>
        /// TO DO TO DO
        /// </summary>
        /// <param name="dictRecent"></param>
        public void FillPQueueRecent(Dictionary<int, News> dictRecent)
        {
            foreach (News val in dictRecent.Values)
            {
                pQueueRecent.Enqueue(val, -val.Time);
            }

        }

        public Dictionary<int, News> ReadFile(string path)
        {
            //List<News> allNews = new List<News>();
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

                dynamic? jsonFile = JsonConvert.DeserializeObject(jsonStr);

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

        public void CreateNewsFileByTrend()
        {
            string fileName = "newsByTrend.json";
            FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
            using (StreamWriter wr = new StreamWriter(fileStream))
            {
                wr.WriteLine("[");
                for (int i = 0; i < 500 && pQueueTrend.Count > 0; i++)
                {
                    string strNews = JsonConvert.SerializeObject(pQueueTrend.Dequeue());
                    wr.Write(strNews);
                    if (pQueueTrend.Count == 0)
                    {
                        break;
                    }
                    if (i < 499)
                    {
                        wr.WriteLine(",");
                    }
                    wr.WriteLine();

                }
                wr.WriteLine("]");
            }
        }

        public void CreateNewsFileByTime()
        {
            string fileName = "newsByTime.json";
            FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
            using (StreamWriter wr = new StreamWriter(fileStream))
            {
                wr.WriteLine("[");
                for (int i = 0; i < 500 && pQueueRecent.Count > 0; i++)
                {
                    string strNews = JsonConvert.SerializeObject(pQueueRecent.Dequeue());
                    wr.Write(strNews);
                    if (pQueueRecent.Count == 0)
                    {
                        break;
                    }
                    if (i < 499)
                    {
                        wr.WriteLine(",");
                    }
                    wr.WriteLine();

                    //Console.WriteLine(strNews);
                }
                wr.WriteLine("]");
            }
        }



    }
}