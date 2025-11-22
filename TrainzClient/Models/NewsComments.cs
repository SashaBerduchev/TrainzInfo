namespace TrainzClient.Models
{
    public class NewsComments
    {
        public int Id { get; set; }
        
        public string Comment { get; set; }
        public DateTime DateTime { get; set; }
        public NewsInfo NewsInfo { get; set; }
    }
}
