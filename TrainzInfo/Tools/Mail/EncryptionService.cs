using Microsoft.AspNetCore.DataProtection;
using System;

namespace TrainzInfo.Tools.Mail
{
    public class EncryptionService
    {
        private readonly IDataProtector _protector;
        public EncryptionService(IDataProtectionProvider provider)
        {
            // "EmailSettingsProtector" — унікальний ідентифікатор для групи даних
            _protector = provider.CreateProtector("EmailSettingsProtector");
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;
            return _protector.Protect(plainText);
        }

        public string Decrypt(string encryptedText)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedText))
                    return encryptedText;
                return _protector.Unprotect(encryptedText);
            }
            catch (Exception e)
            {
                // Якщо розшифрування не вдалося, повертаємо оригінальний текст
                Log.AddException("Decryption failed in EncryptionService.Decrypt " + e.ToString());
                return encryptedText;
            }
        }

    }
}
