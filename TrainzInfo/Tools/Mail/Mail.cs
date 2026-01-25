using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private static SendMail _sendMail; 
        private static bool _success = true;
        private static string _error = string.Empty;
        private static UserManager<IdentityUser> _userManager;
        public Mail(IHttpContextAccessor httpContextAccessor, MailSettingsService mailSettingsService, ApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _mailSettingsService = mailSettingsService;
            _context = context;
            _userManager = userManager;
            _sendMail = new SendMail(_settings, _mailSettingsService,  _context);
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
            await _sendMail.SendMailOneAsync(subject, body, user, true);
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
            string subject = "Додано новий локомотив";
            await _sendMail.SendMailOneAsync(subject, body, user, true);
        }


        public async Task SendTrainAddMail(string number, string from, string to, IdentityUser user)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string logoUrl = $"{baseUrl}/MainImages/MainImageSite";

            string body = $@"
                        <p>Шановний користувачу, поїзд номер: {number} з маршрутом: {from} - {to}</p>
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
            List<string> emails =  _userManager.Users.Select(u => u.Email).ToList();
            await _sendMail.SendMailManyAsync(subject, body, user, emails, true);
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
            await _sendMail.SendMailOneAsync(subject, body, user, true);
        }

        public async Task SendNewsMessage(int newsid, IdentityUser user)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string logoUrl = $"{baseUrl}/MainImages/MainImageSite";
            string orderUrl = $"{baseUrl}/Orders/Details/{newsid}";

            string body = $@"
                        <p>Шановний користувачу, ваша новина опублікована {newsid}</p>
                         <p>
                             Ви можете переглянути його за посиланням: 
                             <a href='{newsid}' target='_blank'>Відкрити новину</a>
                         </p>
                        <p>Дякуємо за користування нашим сервісом!</p>
                        <br />
                        <hr />
                        <table style='font-family:Arial;font-size:12px;color:#444;'>
                            <tr>
                                <td>
                                    <img src='{newsid}'  alt='Логотип' width='120' alt='Trainzinfo Logo' />
                                </td>
                                <td style='padding-left:10px;'>
                                    <strong>Arsshina Service</strong><br/>
                                    <a href='https://trinzinfo.com.ua'>www.arsshina.com</a><br/>
                                    ✉️ <a href='mailto:support@atrainzingo.com.ua'>support@arsshina.com</a>
                                </td>
                            </tr>
                        </table>";
            string subject = "На сайті опубліковано новину!";
            await _sendMail.SendMailOneAsync(subject, body, user, true);
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
            await _sendMail.SendMailOneAsync(subject, body, user, true);
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
            await _sendMail.SendMailOneAsync(subject, body, user, true);
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
            await _sendMail.SendMailOneAsync(subject, body, user, true);
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
            await _sendMail.SendMailOneAsync(subject, body, user, true);
        }
    }
}
   
