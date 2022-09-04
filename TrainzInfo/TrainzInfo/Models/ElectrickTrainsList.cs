using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class ElectrickTrainsList
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int NumberTrain { get; set; }
        [Required]
        public string Depo { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Imgsrc { get; set; }
    }
}
