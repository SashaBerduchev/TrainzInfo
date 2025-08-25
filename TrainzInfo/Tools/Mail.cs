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
            string remoteIpAddress = remoteIpAddres;
            try
            {
                MailMessage m = new MailMessage(_sendemail, user.Email);
                m.Body = user.Name + "Новина: " + news + " опублікована, Дякуємо вам!!!";
                SmtpClient smtp = new SmtpClient(_host, _port);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(_sendemail, _sendpassword);
                smtp.EnableSsl = true;
                smtp.Send(m);
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp.ToString());
                LoggingExceptions.MailLogging(exp.ToString());
            }
        }
    }
}
   
