using System.ComponentModel.DataAnnotations;

namespace TrainzInfoWebGW.DTO
{
    public class NewsDTO
    {
        public int id { get; set; }
        public string NameNews { get; set; }
        public string BaseNewsInfo { get; set; }
        public string NewsInfoAll { get; set; }
        public DateOnly DateTime { get; set; }
        public string Imgsrc { get; set; }
        public byte[] NewsImage { get; set; }
        public string ImageMimeTypeOfData { get; set; }
    }
}
