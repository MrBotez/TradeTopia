using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTopia.Common.Log
{
    public class EventLogger
    {
        #region Singleton stuff

        private static volatile EventLogger instance;
        private static object syncRoot = new Object();

        private EventLogger() { }

        public static EventLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new EventLogger();
                    }
                }

                return instance;
            }
        }

        #endregion

        public String EventLogSource { get; set; }

        public void LogToEventLog(String AMessage, LogType ALogType)
        {
            LogToEventLog(EventLogSource, AMessage, ALogType);
        }

        public void LogToEventLog(String ASource, String AMessage, LogType ALogType)
        {
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists(ASource))
                {
                    System.Diagnostics.EventLog.CreateEventSource(ASource, "Application");
                }

                switch (ALogType)
                {
                    case LogType.ltError:
                        {
                            System.Diagnostics.EventLog.WriteEntry(ASource, AMessage, System.Diagnostics.EventLogEntryType.Error);
                            break;
                        }
                    case LogType.ltWarning:
                        {
                            System.Diagnostics.EventLog.WriteEntry(ASource, AMessage, System.Diagnostics.EventLogEntryType.Warning);
                            break;
                        }
                    case LogType.ltInfo:
                        {
                            System.Diagnostics.EventLog.WriteEntry(ASource, AMessage, System.Diagnostics.EventLogEntryType.Information);
                            break;
                        }
                    case LogType.ltDebug:
                        {
                            System.Diagnostics.EventLog.WriteEntry(ASource, AMessage, System.Diagnostics.EventLogEntryType.Information);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(String.Format("Logging to the event log failed: Message=[{0}] Error=[{1}]", AMessage, ex.Message));

                try
                {
                    FileLogger.Instance.Log(ASource, String.Format("Logging to the event log failed: Message=[{0}] Error=[{1}]", AMessage, ex.Message), LogType.ltError);
                }
                catch { }
            }

            switch (ALogType)
            {
                case LogType.ltError: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogType.ltWarning: Console.ForegroundColor = ConsoleColor.Yellow; break;
                default: Console.ForegroundColor = ConsoleColor.Gray; break;
            }

            Console.WriteLine(AMessage);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }

}
