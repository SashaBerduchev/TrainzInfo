using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class DieselLocomoives
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        public int MaxSpeed { get; set; }
        public int SectionCount { get; set; }
        public int DiseslPower { get; set; }
    }
}
