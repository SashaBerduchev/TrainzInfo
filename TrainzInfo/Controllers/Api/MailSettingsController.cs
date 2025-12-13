using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Models;
using TrainzInfo.Tools;
using TrainzInfo.Tools.Mail;
using TrainzInfoShared.DTO.GetDTO;

namespace TrainzInfo.Controllers.Api
{
    [ApiController]
    [Route("api/mail")]
    public class MailSettingsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly EncryptionService _encryptionService;

        public MailSettingsController(EncryptionService encryptionService, ApplicationContext context)
        {
            _encryptionService = encryptionService;
            _context = context;
        }

        [HttpPost("create")]
        public async Task<ActionResult> SaveMailSettingsAsync(MailDTO mailsettings)
        {
            Log.Init(this.ToString(), nameof(SaveMailSettingsAsync));
            try
            {
                Log.Wright("Try save mail");
                var encryptedPassword = _encryptionService.Encrypt(mailsettings.Password);

                var settings = await _context.MailSettings.Where(x => x.Name == mailsettings.Name).FirstOrDefaultAsync();
                if (settings == null)
                {
                    settings = new MailSettings();
                }
                settings.Name = mailsettings.Name;
                settings.User = mailsettings.User;
                settings.PasswordEncrypted = encryptedPassword;
                settings.Host = mailsettings.Host;
                settings.Port = mailsettings.Port;
                settings.Email = mailsettings.Email;
                settings.From = mailsettings.From;
                settings.EnableSsl = true;
                _context.MailSettings.Add(settings);
                await _context.SaveChangesAsync();
                return Ok();
            }catch(Exception ex)
            {
                Log.Wright("ERROR");
                Log.AddException(ex.ToString());
                return BadRequest(ex.ToString());
            }
            finally
            {
                Log.Finish();
            }
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
