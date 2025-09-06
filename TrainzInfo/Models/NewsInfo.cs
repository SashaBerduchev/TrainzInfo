using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class NewsInfo
    {
        public int id { get; set; }
        [Required]
        public string NameNews { get; set; }
        [Required]
        public string BaseNewsInfo { get; set; }
        public string NewsInfoAll { get; set; }
        public DateOnly DateTime { get; set; }
        public string Imgsrc { get; set; }
        public byte[] NewsImage { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public ICollection<NewsComments> NewsComments { get; set; }
        
    }
}
