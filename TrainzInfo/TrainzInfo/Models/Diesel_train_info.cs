using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class Diesel_train_info
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string AllInfo { get; set; }
        [Required]
        public string Imgsrc { get; set; }
    }
}
