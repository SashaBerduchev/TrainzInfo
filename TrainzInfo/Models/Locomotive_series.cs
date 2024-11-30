using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class Locomotive_series
    {
        public int id { get; set; }
        [Required]
        public string Seria { get; set; }
        public ICollection<Locomotive> Locomotives { get; set; } 
    }
}
