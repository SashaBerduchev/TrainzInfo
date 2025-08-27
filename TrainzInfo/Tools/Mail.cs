using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using TrainzInfo.Models;

namespace TrainzInfo.Tools
{
    
    public class Mail
    {
        static string _sendemail;
        static string _sendpassword;
        static string _host;
        static int _port;

        public Mail()
        {
            _sendemail = "dataset@trainzinfo.com.ua";
            _sendpassword = "kbnswj7zcqoegayhrliv";
            _host = "trainzinfo.com.ua";
            _port = 587;
        }
        public static void SendMessageNews( string news, string remoteIpAddres, Users user)
        {
            string bodymail = user.Name + "Новина: " + news + " опублікована, Дякуємо вам!!!";
            SendMail(bodymail, user);
        }
        public static void SendLocomotivesAddMessage(string Loconame, string remoteIpAddres, Users user)
        {
            string bodymail = user.Name + "Локомотив: " + Loconame + " додано на сайт, Дякуємо вам!!!";
            SendMail(bodymail, user);
        }
        public static void SendMail(string body, Users user)
        {
            LoggingExceptions.LogInit("Mail", nameof(SendMessageNews));
            LoggingExceptions.LogStart();
            LoggingExceptions.LogWright("Try find user email");
            
            try
            {
                LoggingExceptions.LogWright("Try send email to " + user.Email);
                MailMessage m = new MailMessage(_sendemail, user.Email);
                m.Body = body;
                LoggingExceptions.LogWright("Create message body " + m.Body);
                SmtpClient smtp = new SmtpClient(_host, _port);
                LoggingExceptions.LogWright("Create SMTP client " + smtp);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(_sendemail, _sendpassword);
                smtp.EnableSsl = true;
                LoggingExceptions.LogWright("Try send email");
                smtp.Send(m);
            }
            catch (Exception exp)
            {
                LoggingExceptions.LogWright("Error send email to " + user.Email);
                LoggingExceptions.LogWright(exp.ToString());
                Trace.WriteLine(exp.ToString());
                LoggingExceptions.AddException(exp.ToString());
            }
            LoggingExceptions.LogFinish();
        }
    }
}
   
