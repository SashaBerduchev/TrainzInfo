using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfoModel.Models.Information.Images;

namespace TrainzInfoModel.Models.Information.Main
{
    public class NewsInfo
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
        [Required]
        public string NameNews { get; set; }
        [Required]
        public string BaseNewsInfo { get; set; }
        public string NewsInfoAll { get; set; }
        public DateTime DateTime { get; set; }
        public string Imgsrc { get; set; }
        public byte[] NewsImage { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public ICollection<NewsComments> NewsComments { get; set; }
        public NewsImage NewsImages { get; set; }
        public IdentityUser User { get; set; }

    }
}
