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
    }
}
