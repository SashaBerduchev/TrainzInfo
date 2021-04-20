using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class Diesel_trainz
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int VagonCount { get; set; }
        [Required]
        public int Power { get; set; }
        public string BaseInfo { get; set; }
        public string AllInfo { get; set; }
        [Required]
        public string ImgSrc { get; set; }
    }
}
