using Microsoft.AspNetCore.Identity;
using System;

namespace TrainzInfoModel.Models.Dictionaries.MetaData
{
    public class SendEmail
    {
        public int Id { get; set; }

        public IdentityUser ToUser { get; set; } // Користувач, який відправив листа
        public string ToEmail { get; set; }          // Кому
        public string Subject { get; set; }          // Тема
        public string Body { get; set; }             // Текст листа
        public DateTime SentDate { get; set; } = DateTime.Now; // Дата відправки

        public bool IsSuccess { get ; set; }          // Чи успішно відправлено
        public string ErrorMessage { get; set; }
    }
}
