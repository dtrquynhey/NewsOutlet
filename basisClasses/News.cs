using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsOutlet
{
    class News
    {
        public int ID;
        public long Time;
        public string[] Keywords;
        public string Content;
        public int Hits;

        public News(int id, long time, string content, string[] keywords, int hits)
        {
            this.ID = id;
            this.Time = time;
            this.Keywords = keywords;
            this.Content = content;
            this.Hits = hits;
        }

        public News(News news)
        {
            this.ID = news.ID;
            this.Time = news.Time;
            this.Keywords = news.Keywords;
            this.Content = news.Content;
            this.Hits = news.Hits;
        }
        

        private string getKeywords()
        {
            string strKeywords = "";
            for (int i = 0; i < Keywords.Length; i++)
            {
                strKeywords += Keywords[i];

                if (i < Keywords.Length - 1)
                {
                    strKeywords += ", ";
                }
            }
            if (string.IsNullOrEmpty(strKeywords))
            {
                strKeywords = "NO category found for this news";
            }
            return strKeywords;
        }

        private string getTime()
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(Time);
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public override string ToString()
        {
            return "ID: " + ID + ".\tTime: " + getTime() + ".\tKeywords: " + getKeywords() + ".\tHits: " + Hits;
        }
    }
}