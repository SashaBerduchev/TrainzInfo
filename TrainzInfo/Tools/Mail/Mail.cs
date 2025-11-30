using Azure.Core;
using Microsoft.AspNetCore.Identity;
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
        public static void SendMessageNews( string news, string remoteIpAddres, IdentityUser user)
        {
            
            string bodymail = user.UserName + "Новина: " + news + " опублікована, Дякуємо вам!!!";
            SendMail(bodymail, user);
        }
        public static void SendLocomotivesAddMessage(string Loconame, string remoteIpAddres, IdentityUser user)
        {
            string bodymail = user.UserName + "Локомотив: " + Loconame + " додано на сайт, Дякуємо вам!!!";
            SendMail(bodymail, user);
        }
        public static void SendMail(string body, IdentityUser user)
        {
            Log.Init("Mail", nameof(SendMessageNews));
            Log.Start();
            Log.Wright("Try find user email");
            
            try
            {
                Log.Wright("Try send email to " + user.Email);
                MailMessage m = new MailMessage(_sendemail, user.Email);
                m.Body = body;
                Log.Wright("Create message body " + m.Body);
                SmtpClient smtp = new SmtpClient(_host, _port);
                Log.Wright("Create SMTP client " + smtp);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(_sendemail, _sendpassword);
                smtp.EnableSsl = true;
                Log.Wright("Try send email");
                smtp.Send(m);
            }
            catch (Exception exp)
            {
                Log.Wright("Error send email to " + user.Email);
                Log.Wright(exp.ToString());
                Trace.WriteLine(exp.ToString());
                Log.AddException(exp.ToString());
            }
            Log.Finish();
        }
    }
}
   
