using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace TrainzInfo.Tools
{
    public class LoggingExceptions
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

        static string startStandartLogStr = "";

        public static void LogInit(string nameClass, string nameMethod)
        {
            startStandartLogStr = nameClass + " - " + nameMethod;
        }
        public static void LogStart()
        {
            StandartLogFile(startStandartLogStr + " - " + "Start");
        }
        public static void LogFinish()
        {
            StandartLogFile(startStandartLogStr + " - " + "Finish");
            startStandartLogStr = "";
        }

        public static void SQLLogging(string log)
        {
            try
            {
                string logstr = "------Start log------- \n" + log + "\n -------End Log--------" + "\n" + "\n";
                Trace.WriteLine(logstr);
                Console.WriteLine(logstr);
                FileStream fileStreamLog = new FileStream(folderlog + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + " - " + SQLserversLog, FileMode.Append);
                byte[] array = Encoding.Default.GetBytes(logstr.ToString());
                fileStreamLog.Write(array, 0, array.Length);
                fileStreamLog.Close();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                AddException(e.ToString());
            }
        }
        public static void LogWright(string log)
        {
            StandartLogFile(startStandartLogStr + " - " + log);
        }
        
        private static void StandartLogFile(string logmessage)
        {
            logmessage = DateTime.Now.ToString() + " - " + logmessage + "\n";
            Trace.WriteLine( logmessage);
            Console.WriteLine(logmessage);
            FileStream filestreamlog = new FileStream(folderlog + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + " - " + StandartLog, FileMode.Append);
            byte[] array = Encoding.Default.GetBytes(logmessage.ToString());
            filestreamlog.Write(array, 0, array.Length);
            filestreamlog.Close();
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
                LoggingExceptions.AddException(exp.StackTrace);
                LoggingExceptions.AddException(exp.ToString());
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
                string log = "------Start log------- \n" + exception + "\n -------EndLog--------\n";
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
                FileStream fileStreamLog = new FileStream(folderlog + "\\" +  DateTime.Now.ToString("yyyy-MM-dd") + " - " + ExcelErrors, FileMode.Append);
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
