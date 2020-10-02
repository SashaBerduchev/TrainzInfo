using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class Electic_locomotive
    {
        public int id { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public int Speed { get; set; }
        [Required]
        public int SectionCount { get; set; }
        [Required]
        public int ALlPowerP { get; set; }
        [Required]
        public string Seria { get; set; }
        public string LocomotiveImg { get; set; }
    }
}
