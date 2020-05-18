using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class CargoCarrieges
    {
        public int id { get; set; }
        [Required]
        public string CarriegeType { get; set; }
        [Required]
        public int MaxSpeed { get; set; }
        [Required]
        public string CargoType { get; set; }
        public int CargoWeight { get; set; }
    }
}
