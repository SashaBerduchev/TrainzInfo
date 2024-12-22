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
        static string StandartLog = "StandartLog.log";
        static string SQLserversLog = "SQLserversLog.log";
        static string worklog = "WorkLog.log";
        static string ExceptionLog = "ExceptionLog.log";
        static string ErrorLog = "ErrorLog.log";
        static string ConnectionLog = "ConnectionLog.log";
        static string MailLog = "MailLog.log";
        static string ExcelErrors = "ExcelErrors.log";

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
                FileStream fileStreamLog = new FileStream(folderlog + "\\" + ConnectionLog, FileMode.Append);
                for (int i = 0; i < standartlogging.Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(standartlogging.ToString());
                    fileStreamLog.Write(array, 0, array.Length);
                }
                fileStreamLog.Close();
            }
            catch (System.Exception exp)
            {
                Console.WriteLine(exp.StackTrace);
            }

        }
        public static void EFLog(string log)
        {
            Trace.WriteLine("------Start log------- \n" + log + "\n -------EndLog--------\n");
            Console.WriteLine(log);
            try
            {
                string standartlogging = "------Start log------- \n" + log + "\n -------EndLog--------\n" + "\n";
                FileStream fileStreamLog = new FileStream(folderlog + "\\" + StandartLog, FileMode.Append);
                for (int i = 0; i < standartlogging.Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(standartlogging.ToString());
                    fileStreamLog.Write(array, 0, array.Length);
                }
                fileStreamLog.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("------Start log------- \n" + ex.Message + "\n -------EndLog--------\n");
                Console.WriteLine(ex.Message);
            }
            WriteSqlLog(log);
            ErrorLogEF(log);

        }

        private static void WriteSqlLog(string log)
        {
            try
            {
                if (log.Contains("Executing DbCommand"))
                {
                    string standartlogging = "------Start log------- \n" + log + "\n -------EndLog--------\n" + "\n";
                    FileStream fileStreamLog = new FileStream(folderlog + "\\" + SQLserversLog, FileMode.Append);
                    for (int i = 0; i < standartlogging.Length; i++)
                    {
                        byte[] array = Encoding.Default.GetBytes(standartlogging.ToString());
                        fileStreamLog.Write(array, 0, array.Length);
                    }
                    fileStreamLog.Close();
                }
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
                FileStream fileStreamLog = new FileStream(folderlog + "\\" + ErrorLog, FileMode.Append);
                for (int i = 0; i < standartlogging.Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(standartlogging.ToString());
                    fileStreamLog.Write(array, 0, array.Length);
                }
                fileStreamLog.Close();
            }
        }

        public static void AddException(string exception)
        {
            try
            {
                string log = "------Start log------- \n" + exception + "\n -------EndLog--------";
                string dir = folderlog + "\\" + log;
                FileStream fileStreamLog = new FileStream(folderlog + "\\" + ExceptionLog, FileMode.Append);
                for (int i = 0; i < log.Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(log.ToString());
                    fileStreamLog.Write(array, 0, array.Length);
                }
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
                FileStream fileStreamLog = new FileStream(folderlog + "\\" + ExcelErrors, FileMode.Append);
                for (int i = 0; i < log.Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(log.ToString());
                    fileStreamLog.Write(array, 0, array.Length);
                }
                Trace.WriteLine(log);
                Console.WriteLine(log);
                fileStreamLog.Close();
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
            }

        }
    }
}
