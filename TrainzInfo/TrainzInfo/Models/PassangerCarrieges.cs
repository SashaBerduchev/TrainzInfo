using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class PassangerCarrieges
    {
        public int id { get; set; }
        [Required]
        public string CarriegeType {get; set;}
        public int PlaceCount { get; set; }
    }
}
