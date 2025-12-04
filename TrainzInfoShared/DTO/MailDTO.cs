using System;
using System.Collections.Generic;
using System.Text;

namespace TrainzInfoShared.DTO
{
    internal class MailDTO
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public string Email { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
    }
}
