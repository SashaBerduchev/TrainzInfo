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
        public string Number { get; set; }
        [Required]
        public int VagonCount { get; set; }
        public string Depo { get; set; }

        [Required]
        public string ImgSrc { get; set; }
    }
}
