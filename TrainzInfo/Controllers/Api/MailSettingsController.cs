using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools.Mail;

namespace TrainzInfo.Controllers.Api
{
    public class MailSettingsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly EncryptionService _encryptionService;

        public MailSettingsController(EncryptionService encryptionService, ApplicationContext context)
        {
            _encryptionService = encryptionService;
            _context = context;
        }

        public async Task SaveMailSettingsAsync(string Name, string login, string password, string host, int port, bool ssl)
        {
            var encryptedPassword = _encryptionService.Encrypt(password);

            var settings = await _context.MailSettings.Where(x => x.Name == Name).FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new MailSettings();
                _context.MailSettings.Add(settings);
            }
            settings.Name = Name;
            settings.User = login;
            settings.PasswordEncrypted = encryptedPassword;
            settings.Host = host;
            settings.Port = port;
            settings.EnableSsl = ssl;

            await _context.SaveChangesAsync();
        }

        public async Task<MailSettings> GetMailSettingsAsync(int? id)
        {
            var settings = await _context.MailSettings.FirstOrDefaultAsync(x => x.id == id);
            if (settings == null) return null;

            // розшифровуємо перед використанням
            settings.PasswordEncrypted = _encryptionService.Decrypt(settings.PasswordEncrypted);
            return settings;
        }

        public async Task<MailSettings> GetMailSettingsByNameAsync(string? name)
        {
            var settings = await _context.MailSettings.Where(x => x.Name == name).FirstOrDefaultAsync();
            if (settings == null) return null;

            // розшифровуємо перед використанням
            settings.PasswordEncrypted = _encryptionService.Decrypt(settings.PasswordEncrypted);
            return settings;
        }

        public async Task<List<MailSettings>> GetMailsAll()
        {
            var settingsname = await _context.MailSettings.ToListAsync();
            return settingsname;
        }

    }
}
