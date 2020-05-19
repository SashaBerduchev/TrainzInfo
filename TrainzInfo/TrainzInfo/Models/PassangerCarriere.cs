using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class PassangerCarriere
    {
        public int id { get; set; }
        [Required]
        public string Calss { get; set; }
        [Required]
        public int CountPlace { get; set; }
        [Required]
        public string PlaceType { get; set; }
    }
}
