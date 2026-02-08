namespace TrainzInfoModel.Models.Dictionaries.MetaData
{
    public class MailSettings
    {
        public int id {  get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public string Email { get; set; }
        public string User { get; set; }
        public string PasswordEncrypted { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
        public bool EnableSsl{ get; set; }
    }
}
