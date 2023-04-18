using NewsOutlet.basisClasses;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using static NewsOutlet.basisClasses.FilesProcess;

namespace NewsOutlet
{
    class Program
    {
        static void Main(string[] args)
        {
            FilesProcess filesProcess = new FilesProcess();
            filesProcess.CreateSubJsonFiles();
            DataManagement dataManagement = new DataManagement();
            dataManagement.fillPQueue();
            dataManagement.fillDictionary();
            bool exit = false;
            //DateTime now = DateTime.Now;
            //long unixEpoch = dataManagement.convertToUnixEpoch(DateTime.Now);


            
            Console.WriteLine("\n\tWelcome to NEWSOUTLET!");

            while (!exit)
            {
                Console.WriteLine("\t\tSysTime > " + filesProcess.sysDate);
                Console.WriteLine("\n\t\tMAIN MENU");
                Console.WriteLine("\t1 - SHOW RECENT");
                Console.WriteLine("\t2 - SHOW TRENDING");
                Console.WriteLine("\t3 - SELECT");
                Console.WriteLine("\t4 - BACK");
                Console.WriteLine("\t5 - DISPLAY ALL NEWS");
                Console.WriteLine("\t6 - SET TIME");
                Console.WriteLine("\t7 - EXIT");
                Console.Write("Please enter your options (1-6) > ");

                int? cmd = Convert.ToInt32(Console.ReadLine());

                switch (cmd)
                {
                    case 1:
                        string[] keywordsRecent = askFilterByKeywords();
                        Console.WriteLine("\n +++++++ RECENT NEWS +++++++ ");
                        if (keywordsRecent.Length > 0)
                        {
                            dataManagement.displayRecentNewsByKeywords(keywordsRecent);
                        }
                        else
                        {
                            dataManagement.displayRecentNews();
                        }
                        break;
                    case 2:
                        string[] keywordsTrending = askFilterByKeywords();
                        Console.WriteLine("\n +++++++ TRENDING NEWS +++++++ ");
                        if (keywordsTrending.Length > 0)
                        {
                            dataManagement.displayRecentNewsByKeywords(keywordsTrending);
                        }
                        else
                        {
                            dataManagement.displayTrendNews();
                        }
                        dataManagement.fillPQueue();
                        break;
                    case 3:
                        Console.Write("Please enter the newsID you want to select > ");
                        int id = Convert.ToInt32(Console.ReadLine());
                        if (dataManagement.dictOfAllNews.ContainsKey(id))
                        {
                            dataManagement.Select(id);
                            dataManagement.pushNewsToStack(id);
                        }
                        else
                        {
                            Console.WriteLine("No News found with this ID");
                        }
                        break;
                    case 4:
                        int prevId = dataManagement.popNewsFromStack();
                        if (prevId == -1)
                        {
                            break;
                        }
                        dataManagement.Select(prevId);
                        break;
                    case 5:
                        Console.WriteLine("\n +++++++ ALL NEWS +++++++");
                        dataManagement.displayAllNews();
                        break;
                    case 6:
                        filesProcess.sysDate = askTime();
                        filesProcess.ResetNewsFileByTime();
                        DataManagement.fillDictNewsByTime();
                        break;
                    default:
                        exit = true;
                        Console.WriteLine("Exiting....");
                        break;
                }
            }
            Console.ReadKey();
        }

        private static string[] askFilterByKeywords()
        {
            Console.Write("Do you want to filter by keywords? (y/n) > ");
            string[] keywords;
            char? input;
            try
            {
                input = Convert.ToChar(Console.ReadLine());
                if (input.Equals('y') || input.Equals('Y'))
                {
                    Console.Write("Please enter keywords (seperated by comma) > ");
                    string? kws = Console.ReadLine();
                    if (kws != null)
                    {
                        kws = kws.Trim();
                        keywords = kws.Split(',');
                        return keywords;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new string[0];
        }

        private static DateTime askTime()
        {
            Console.WriteLine("System Current Time now > " + DateTime.Now);
            Console.Write("Please enter the time to set (yyyy-MM-dd HH:mm:ss) > ");
            string? dateString = Console.ReadLine();
            DateTime desiredTime = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", null);
            Console.WriteLine("Updated Time > " + desiredTime);
            return desiredTime;

          
            
            //return DateTimeProvider.convertToUnixEpoch(dateTime);
        }
    }

}