using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;

namespace TrainzInfo.Tools.Mail
{

    public class Mail
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static ApplicationContext _context;
        private static MailSettingsService _mailSettingsService;
        private static SmtpSettings _settings;
        private static bool _success = true;
        private static string _error = string.Empty;
        public Mail(IHttpContextAccessor httpContextAccessor, MailSettingsService mailSettingsService, ApplicationContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _mailSettingsService = mailSettingsService;
            _context = context;
        }

        public async Task SendMessageNews(string news, IdentityUser user)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string logoUrl = $"{baseUrl}/MainImages/MainImageSite";

            string body = $@"
                        <p>Шановний користувачу, вашу новину {news} було успішно опубліковано</p>
                        <p>Дякуємо за користування нашим сервісом!</p>
                        <br />
                        <hr />
                        <table style='font-family:Arial;font-size:12px;color:#444;'>
                            <tr>
                                <td>
                                    <img src='{logoUrl}'  alt='Логотип' width='120' alt='Arsshina Logo' />
                                </td>
                                <td style='padding-left:10px;'>
                                    <strong>Arsshina Service</strong><br/>
                                    <a href='https://arsshina.com'>www.arsshina.com</a><br/>
                                    ✉️ <a href='mailto:support@arsshina.com'>support@arsshina.com</a>
                                </td>
                            </tr>
                        </table>";
            string subject = "Новина опублікована!";
            await SendMail(subject, body, user, true);
        }

        public async Task SendLocomotivesAddMessage(string name, IdentityUser user)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string logoUrl = $"{baseUrl}/MainImages/MainImageSite";

            string body = $@"
                        <p>Шановний користувачу, локомотив: {name} було успішно додано</p>
                        <p>Дякуємо за користування нашим сервісом!</p>
                        <br />
                        <hr />
                        <table style='font-family:Arial;font-size:12px;color:#444;'>
                            <tr>
                                <td>
                                    <img src='{logoUrl}'  alt='Логотип' width='120' alt='Arsshina Logo' />
                                </td>
                                <td style='padding-left:10px;'>
                                    <strong>Arsshina Service</strong><br/>
                                    <a href='https://www.trainzinfo.com.ua/'>www.arsshina.com</a><br/>
                                    ✉️ <a href='mailto:support@trainzinfo.com'>support@arsshina.com</a>
                                </td>
                            </tr>
                        </table>";
            string subject = "Створено нове звамовлення";
            await SendMail(subject, body, user, true);
        }

        public async Task SendDeleteOrder(int ordernumber, IdentityUser user)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string logoUrl = $"{baseUrl}/MainImages/MainImageSite";

            string body = $@"
                        <p>Шановний користувачу, ваше замовлення з номером: {ordernumber} видалено</p>
                        <p>Дякуємо за користування нашим сервісом!</p>
                        <br />
                        <hr />
                        <table style='font-family:Arial;font-size:12px;color:#444;'>
                            <tr>
                                <td>
                                    <img src='{logoUrl}'  alt='Логотип' width='120' alt='Arsshina Logo' />
                                </td>
                                <td style='padding-left:10px;'>
                                    <strong>Arsshina Service</strong><br/>
                                    <a href='https://arsshina.com'>www.arsshina.com</a><br/>
                                    ✉️ <a href='mailto:support@arsshina.com'>support@arsshina.com</a>
                                </td>
                            </tr>
                        </table>";
            string subject = "Замовлення видалено";
            await SendMail(subject, body, user, true);
        }

        public async Task SendManagerOrderAssigned(int ordernumber, IdentityUser user)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string logoUrl = $"{baseUrl}/MainImages/MainImageSite";
            string orderUrl = $"{baseUrl}/Orders/Details/{ordernumber}";

            string body = $@"
                        <p>Шановний користувачу, на сайті призначено нове замовлення номер {ordernumber}</p>
                         <p>
                             Ви можете переглянути його за посиланням: 
                             <a href='{orderUrl}' target='_blank'>Відкрити замовлення</a>
                         </p>
                        <p>Дякуємо за користування нашим сервісом!</p>
                        <br />
                        <hr />
                        <table style='font-family:Arial;font-size:12px;color:#444;'>
                            <tr>
                                <td>
                                    <img src='{logoUrl}'  alt='Логотип' width='120' alt='Arsshina Logo' />
                                </td>
                                <td style='padding-left:10px;'>
                                    <strong>Arsshina Service</strong><br/>
                                    <a href='https://arsshina.com'>www.arsshina.com</a><br/>
                                    ✉️ <a href='mailto:support@arsshina.com'>support@arsshina.com</a>
                                </td>
                            </tr>
                        </table>";
            string subject = "На сайті створено нове замовлення!";
            await SendMail(subject, body, user, true);
        }

        public async Task SendManagerOrderInProgress(int ordernumber, IdentityUser user)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string logoUrl = $"{baseUrl}/MainImages/MainImageSite";
            string orderUrl = $"{baseUrl}/Orders/Details/{ordernumber}";

            string body = $@"
                        <p>Шановний користувачу, вам призначено нове замовлення номер {ordernumber}</p>
                         <p>
                             Ви можете переглянути його за посиланням: 
                             <a href='{orderUrl}' target='_blank'>Відкрити замовлення</a>
                         </p>
                        <p>Дякуємо за користування нашим сервісом!</p>
                        <br />
                        <hr />
                        <table style='font-family:Arial;font-size:12px;color:#444;'>
                            <tr>
                                <td>
                                    <img src='{logoUrl}'  alt='Логотип' width='120' alt='Arsshina Logo' />
                                </td>
                                <td style='padding-left:10px;'>
                                    <strong>Arsshina Service</strong><br/>
                                    <a href='https://arsshina.com'>www.arsshina.com</a><br/>
                                    ✉️ <a href='mailto:support@arsshina.com'>support@arsshina.com</a>
                                </td>
                            </tr>
                        </table>";
            string subject = "Вам призначено замовлення";
            await SendMail(subject, body, user, true);
        }

        public async Task SendManagerOrderCompleted(int ordernumber, IdentityUser user)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string logoUrl = $"{baseUrl}/MainImages/MainImageSite";
            string orderUrl = $"{baseUrl}/Orders/Details/{ordernumber}";

            string body = $@"
                        <p>Шановний користувачу, ваше замовлення номер {ordernumber} успішно виконано!</p>
                         <p>
                             Ви можете переглянути його за посиланням: 
                             <a href='{orderUrl}' target='_blank'>Відкрити замовлення</a>
                         </p>
                        <p>Дякуємо за користування нашим сервісом!</p>
                        <br />
                        <hr />
                        <table style='font-family:Arial;font-size:12px;color:#444;'>
                            <tr>
                                <td>
                                    <img src='{logoUrl}'  alt='Логотип' width='120' alt='Arsshina Logo' />
                                </td>
                                <td style='padding-left:10px;'>
                                    <strong>Arsshina Service</strong><br/>
                                    <a href='https://arsshina.com'>www.arsshina.com</a><br/>
                                    ✉️ <a href='mailto:support@arsshina.com'>support@arsshina.com</a>
                                </td>
                            </tr>
                        </table>";
            string subject = "Замовлення виконано успішно!";
            await SendMail(subject, body, user, true);
        }

        public async Task SendTireAddToBasket(string tireinbasket, IdentityUser user)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string logoUrl = $"{baseUrl}/MainImages/MainImageSite";

            string body = $@"
                        <p>Шановний користувачу, до вашого кошика було додано шину: {tireinbasket}</p>
                        <p>Дякуємо за користування нашим сервісом!</p>
                        <br />
                        <hr />
                        <table style='font-family:Arial;font-size:12px;color:#444;'>
                            <tr>
                                <td>
                                    <img src='{logoUrl}'  alt='Логотип' width='120' alt='Arsshina Logo' />
                                </td>
                                <td style='padding-left:10px;'>
                                    <strong>Arsshina Service</strong><br/>
                                    <a href='https://arsshina.com'>www.arsshina.com</a><br/>
                                    ✉️ <a href='mailto:support@arsshina.com'>support@arsshina.com</a>
                                </td>
                            </tr>
                        </table>";
            string subject = "До кошика додано шину";
            await SendMail(subject, body, user, true);
        }

        public async Task SendTireDeleteFromBasket(string tireinbasket, IdentityUser user)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string logoUrl = $"{baseUrl}/MainImages/MainImageSite";

            string body = $@"
                        <p>Шановний користувачу, з вашого кошика було видалено шину: {tireinbasket}</p>
                        <p>Дякуємо за користування нашим сервісом!</p>
                        <br />
                        <hr />
                        <table style='font-family:Arial;font-size:12px;color:#444;'>
                            <tr>
                                <td>
                                    <img src='{logoUrl}'  alt='Логотип' width='120' alt='Arsshina Logo' />
                                </td>
                                <td style='padding-left:10px;'>
                                    <strong>Arsshina Service</strong><br/>
                                    <a href='https://arsshina.com'>www.arsshina.com</a><br/>
                                    ✉️ <a href='mailto:support@arsshina.com'>support@arsshina.com</a>
                                </td>
                            </tr>
                        </table>";
            string subject = "Шина видалена з кошика";
            await SendMail(subject, body, user, true);
        }

        private static async Task SendMail(string subject, string body, IdentityUser user, bool isHtml = false)
        {

            Log.Init("Mail", nameof(SendMessageNews));
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
                    EnableSsl = _settings.EnableSsl,
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
                smtp.Send(mail);
            }
            catch (Exception exp)
            {
                Log.Wright("Error send email to " + user.Email);
                Log.Wright(exp.ToString());
                Log.AddException(exp.ToString());
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
   
