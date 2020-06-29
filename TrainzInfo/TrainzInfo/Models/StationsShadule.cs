using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class StationsShadule
    {
        public int id { get; set; }
        [Required]
        public string Station { get; set; }
        [Required]
        public string UzFilia { get; set; }
        [Required]
        public DateTime TimeOfArrive { get; set; }
        [Required]
        public DateTime TimeOfDepet { get; set; }
        [Required]
        public string TrainInfo { get; set; }
        [Required]
        public string ImgTrain { get; set; }
    }
}
