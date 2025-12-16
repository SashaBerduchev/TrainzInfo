using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TrainzInfo.Tools
{
    public class Log
    {
        static string folder = "Logs";
        static string folderlog = @"Logs";
        static string StandartLog = "StandartWork.log";
        static string SQLserversLog = "SQL.log";
        static string worklog = "WorkLog.log";
        static string ExceptionLog = "Exceptions.log";
        static string ErrorLog = "ErrorLog.log";
        static string ConnectionLog = "ConnectionLog.log";
        static string MailLog = "Mail.log";
        static string ExcelErrors = "ExcelErrors.log";
        static string Blocking = "BlockingSQL.log";

        static string startStandartLogStr = "";

        private static readonly object _logLock = new object();
        public static void Init(string nameClass, string nameMethod)
        {
            startStandartLogStr = DateTime.Now + " - [INF] " +  nameClass + " - " + nameMethod;
            StandartLogFile(startStandartLogStr + " - " + "Start");
        }
        public static void Finish()
        {
            StandartLogFile(startStandartLogStr + " - " + "Finish");
            startStandartLogStr = "";
        }

        public static void SQLLogging(string log)
        {
            try
            {
                string logstr = "------Start log------- \n " + DateTime.Now + "\n" + log + "\n -------End Log--------" + "\n" + "\n";
                Trace.WriteLine(logstr);
                Console.WriteLine(logstr);
                string filePath = Path.Combine(folderlog, DateTime.Now.ToString("yyyy-MM-dd") + " - " + SQLserversLog);

                lock (_logLock)
                {
                    using (var filestreamlog = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (var writer = new StreamWriter(filestreamlog, Encoding.UTF8))
                    {
                        writer.Write(logstr);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                AddException(e.ToString());
            }
        }

        public static void BlockingLog(string log)
        {
            try
            {
                string logstr = "------Start log------- \n " + DateTime.Now + "\n" + log + "\n -------End Log--------" + "\n" + "\n";
                Trace.WriteLine(logstr);
                Console.WriteLine(logstr);
                string filePath = Path.Combine(folderlog, DateTime.Now.ToString("yyyy-MM-dd") + " - " + Blocking);

                lock (_logLock)
                {
                    using (var filestreamlog = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (var writer = new StreamWriter(filestreamlog, Encoding.UTF8))
                    {
                        writer.Write(logstr);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                AddException(e.ToString());
            }
        }
        public static void Wright(string log)
        {
            StandartLogFile(startStandartLogStr + " - " + log);
        }

        private static void StandartLogFile(string logmessage)
        {
            try
            {
                logmessage = DateTime.Now.ToString() + " - " + logmessage + "\n";
                Trace.WriteLine(logmessage);
                Console.WriteLine(logmessage);
                string filePath = Path.Combine(folderlog, DateTime.Now.ToString("yyyy-MM-dd") + " - " + StandartLog);
                lock (_logLock)
                {
                    using (var filestreamlog = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (var writer = new StreamWriter(filestreamlog, Encoding.UTF8))
                    {
                        writer.Write(logmessage);
                    }
                }
            }
            catch (Exception e)
            {
                Log.AddException(e.ToString());
            }
        }

        public static void CreateFolder()
        {
            try
            {
                if (Directory.Exists(folder))
                {
                    Trace.WriteLine("That path exists already.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(folder);
                Trace.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(folder).Minute.ToString());

            }
            catch (System.Exception exp)
            {
                Log.AddException(exp.StackTrace);
                Log.AddException(exp.ToString());
            }
        }
        public static void ConnLog(string log)
        {
            try
            {
                Trace.WriteLine("------Start log------- \n" + log + "\n -------EndLog--------\n");
                Console.WriteLine(log);
                string standartlogging = "------Start log------- \n" + log + "\n -------EndLog--------\n" + "\n";
                FileStream fileStreamLog = new FileStream(folderlog + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + " - " + ConnectionLog, FileMode.Append);
                byte[] array = Encoding.Default.GetBytes(standartlogging.ToString());
                fileStreamLog.Write(array, 0, array.Length);
                fileStreamLog.Close();
            }
            catch (System.Exception exp)
            {
                Console.WriteLine(exp.StackTrace);
            }

        }

        private static void ErrorLogEF(string log)
        {
            if (log.Contains("Exception"))
            {
                string standartlogging = "------Start log------- \n" + log + "\n -------EndLog--------\n" + "\n";
                Console.WriteLine(standartlogging);
                Trace.Write(standartlogging);
                FileStream fileStreamLog = new FileStream(folderlog + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + " - " + ErrorLog, FileMode.Append);
                byte[] array = Encoding.Default.GetBytes(standartlogging.ToString());
                fileStreamLog.Write(array, 0, array.Length);
                fileStreamLog.Close();
            }
        }

        public static void AddException(string exception)
        {
            try
            {
                string log = "------Start log------- \n" + DateTime.Now + "[EXCEPTION] - " + exception + "\n -------EndLog--------\n" + "\n + \n";
                string dir = folderlog + "\\" + log;
                Console.WriteLine(log);
                Trace.Write(log);
                FileStream fileStreamLog = new FileStream(folderlog + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + " - " + ExceptionLog, FileMode.Append);
                byte[] array = Encoding.Default.GetBytes(log.ToString());
                fileStreamLog.Write(array, 0, array.Length);
                Trace.WriteLine(log);
                Console.WriteLine(log);
                fileStreamLog.Close();
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
            }

        }
        public static void AddExcelExeptions(string exception)
        {
            try
            {
                string log = "------Start log------- \n" + exception + "\n -------EndLog--------";
                string dir = folderlog + "\\" + log;
                FileStream fileStreamLog = new FileStream(folderlog + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + " - " + ExcelErrors, FileMode.Append);
                byte[] array = Encoding.Default.GetBytes(log.ToString());
                fileStreamLog.Write(array, 0, array.Length);
                Trace.WriteLine(log);
                Console.WriteLine(log);
                fileStreamLog.Close();
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
            }

        }

        public static void MailLogging(string log)
        {
            try
            {
                string standartlogging = "------Start log------- \n" + log + "\n -------EndLog--------\n" + "\n";
                Trace.Write(standartlogging);
                Console.WriteLine(standartlogging);
                FileStream fileStreamLog = new FileStream(folderlog + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + " - " + MailLog, FileMode.Append);
                byte[] array = Encoding.Default.GetBytes(standartlogging.ToString());
                fileStreamLog.Write(array, 0, array.Length);
                fileStreamLog.Close();
            }
            catch (System.Exception exp)
            {
                Console.WriteLine(exp.StackTrace);
            }
        }

    }
}
