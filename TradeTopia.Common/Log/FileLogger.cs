using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TradeTopia.Common.Log
{
    public class FileLogger
    {
        #region Singleton stuff

        private static volatile FileLogger instance;
        private static object syncRoot = new Object();

        public static FileLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new FileLogger();
                    }
                }

                return instance;
            }
        }

        #endregion

        private FileLogger()
        {
            LogSource = "";

            fileName = System.Reflection.Assembly.GetEntryAssembly().Location.Replace(@".exe", @".log");
            var fi = new FileInfo(fileName);

            if (fi.Exists)
            {
                if (fi.Length > MaxLogfileSize)
                {
                    try
                    {
                        fi.MoveTo(fi.FullName.Replace(@".log", String.Format(@"_{0:yyyyMMddHHmmss}.log", DateTime.Now)));
                    }
                    catch { }
                }

                fi = new FileInfo(fileName);
                var di = new DirectoryInfo(fi.DirectoryName);
                var files = di.GetFiles(fi.Name.Replace(@".log", "") + "_*.log");

                foreach (var f in files)
                {
                    var matches = Regex.Match(f.Name, @"^.*?_(?<date>\d{14})\.log$");

                    if (matches.Success)
                    {
                        var s = matches.Groups[@"date"].Value;
                        var d = new DateTime(
                          Convert.ToInt32(s.Substring(0, 4)),
                          Convert.ToInt32(s.Substring(4, 2)),
                          Convert.ToInt32(s.Substring(6, 2)),
                          Convert.ToInt32(s.Substring(8, 2)),
                          Convert.ToInt32(s.Substring(10, 2)),
                          Convert.ToInt32(s.Substring(12, 2))
                        );

                        if (d.CompareTo(DateTime.Now.AddDays(-10)) == -1)  //The log file is older than 10 days
                        {
                            try
                            {
                                f.Delete();
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        public String LogSource { get; set; }
        public Int32 MaxLogfileSize
        {
            get
            {
                return 1024 * 1000;  //1MB
            }
        }

        private string fileName = "";

        public void Log(String message, LogType logType)
        {
            Log(LogSource, message, logType);
        }

        public void Log(String source, String message, LogType logType)
        {
            try
            {
                String s = string.Format("{0:yyyyMMdd HHmmss}: {1}{2} {3}", DateTime.Now, String.IsNullOrEmpty(source) ? "" : source, Enum.GetName(typeof(LogType), logType), message);

                if (!s.EndsWith(Environment.NewLine)) { s = s + Environment.NewLine; }

                System.IO.File.AppendAllText(fileName, s, Encoding.ASCII);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(String.Format("Logging to file log failed: {0}", ex.Message));
            }
        }
    }
}
