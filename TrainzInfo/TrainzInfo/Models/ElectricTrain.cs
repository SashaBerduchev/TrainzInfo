using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class ElectricTrain
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string VagonsCountP { get; set; }
        
        [Required]
        public int MaxSpeed { get; set; }
        [Required]
        public string Imgsrc{ get; set; }
        [Required]
        public string DepotTrain { get; set; }
        public string DepotCity { get; set; }
    }
}
