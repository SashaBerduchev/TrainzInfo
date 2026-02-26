using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfoModel.Models.Dictionaries.MetaData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrainzInfo.Tools.Mail
{
    public class SendMail
    {
        private static SmtpSettings _settings;
        private static ApplicationContext _context;
        private static MailSettingsService _mailSettingsService;
        private static bool _success = true;
        private static string _error = string.Empty;
        public SendMail(SmtpSettings settings, MailSettingsService mailSettingsService, ApplicationContext context)
        {
            _settings = settings;
            _context = context;
            _mailSettingsService = mailSettingsService;
        }
        public async Task SendMailOneAsync(string subject, string body, IdentityUser user, bool isHtml = false)
        {

            Log.Init("Mail", nameof(SendMail));
            Log.Wright("Try find user email");

            var settings = await _mailSettingsService.GetMailSettingsByNameAsync("Prod");
            _settings = new SmtpSettings
            {
                Host = settings.Host,
                Port = settings.Port,
                EnableSsl = settings.EnableSsl,
                User = settings.User,
                Password = settings.PasswordEncrypted,
                From = settings.User
            };

            // для тест и прод
            //if (Startup.GetEnvironment() == true)
            //{
            //    var settings = await _mailSettingsService.GetMailSettingsByNameAsync("Prod");
            //    _settings = new SmtpSettings
            //    {
            //        Host = settings.Host,
            //        Port = settings.Port,
            //        EnableSsl = settings.EnableSsl,
            //        User = settings.Login,
            //        Password = settings.PasswordEncrypted,
            //        From = settings.Login
            //    };
            //}
            //else
            //{
            //    var settings = await _mailSettingsService.GetMailSettingsByNameAsync("Test");
            //    _settings = new Tools.Mail.SmtpSettings
            //    {
            //        Host = settings.Host,
            //        Port = settings.Port,
            //        EnableSsl = settings.EnableSsl,
            //        User = settings.Login,
            //        Password = settings.PasswordEncrypted,
            //        From = settings.Login
            //    };
            //}

            try
            {
                Log.Wright("Try send email to " + user.Email);
                using var smtp = new SmtpClient
                {
                    Host = _settings.Host,
                    Port = _settings.Port,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_settings.User, _settings.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                Log.Wright("Create message body " + body);
                using var mail = new MailMessage
                {
                    From = new MailAddress(_settings.From),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };

                mail.To.Add(user.Email);
                Log.Wright("Try send mail");
                await smtp.SendMailAsync(mail);
            }
            catch (Exception exp)
            {
                Log.Wright(exp.ToString());
                Log.Exceptions(exp.ToString());
                _success = false;
                _error = exp.ToString();
            }
            try
            {
                var trackedUser = await _context.Users.FindAsync(user.Id);
                var sentEmail = new SendEmail
                {
                    ToUser = trackedUser,
                    ToEmail = user.Email,
                    Subject = subject,
                    Body = body,
                    SentDate = DateTime.Now,
                    IsSuccess = _success,
                    ErrorMessage = _error
                };

                _context.SendEmails.Add(sentEmail);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Wright(ex.ToString());
                Log.Exceptions(ex.ToString());
            }
            finally
            {
                Log.Finish();
            }
        }

        public async Task SendMailManyAsync(string subject, string body, IdentityUser user, List<string> emails, bool isHtml = false)
        {

            Log.Init("Mail", nameof(SendMail));
            Log.Wright("Try find user email");

            var settings = await _mailSettingsService.GetMailSettingsByNameAsync("Prod");
            _settings = new SmtpSettings
            {
                Host = settings.Host,
                Port = settings.Port,
                EnableSsl = settings.EnableSsl,
                User = settings.User,
                Password = settings.PasswordEncrypted,
                From = settings.User
            };

            // для тест и прод
            //if (Startup.GetEnvironment() == true)
            //{
            //    var settings = await _mailSettingsService.GetMailSettingsByNameAsync("Prod");
            //    _settings = new SmtpSettings
            //    {
            //        Host = settings.Host,
            //        Port = settings.Port,
            //        EnableSsl = settings.EnableSsl,
            //        User = settings.Login,
            //        Password = settings.PasswordEncrypted,
            //        From = settings.Login
            //    };
            //}
            //else
            //{
            //    var settings = await _mailSettingsService.GetMailSettingsByNameAsync("Test");
            //    _settings = new Tools.Mail.SmtpSettings
            //    {
            //        Host = settings.Host,
            //        Port = settings.Port,
            //        EnableSsl = settings.EnableSsl,
            //        User = settings.Login,
            //        Password = settings.PasswordEncrypted,
            //        From = settings.Login
            //    };
            //}

            try
            {
                Log.Wright("Try send email to " + user.Email);
                using var smtp = new SmtpClient
                {
                    Host = _settings.Host,
                    Port = _settings.Port,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_settings.User, _settings.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                Log.Wright("Create message body " + body);
                using var mail = new MailMessage
                {
                    From = new MailAddress(_settings.From),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };
                foreach (var item in emails)
                {
                    mail.To.Add(item);
                }
                Log.Wright("Try send mail");
                smtp.Send(mail);
            }
            catch (Exception exp)
            {
                Log.Wright(exp.ToString());
                Log.Exceptions(exp.ToString());
                _success = false;
                _error = exp.ToString();
            }

            var sentEmail = new SendEmail
            {
                ToUser = user,
                ToEmail = user.Email,
                Subject = subject,
                Body = body,
                SentDate = DateTime.Now,
                IsSuccess = _success,
                ErrorMessage = _error
            };

            _context.SendEmails.Add(sentEmail);
            await _context.SaveChangesAsync();

            Log.Finish();
        }
    }
}

