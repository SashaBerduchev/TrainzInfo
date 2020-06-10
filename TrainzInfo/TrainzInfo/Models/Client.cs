using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class Client
    {
        public int id { get; set; }
        [Required]
        public double Version { get; set; }
        [Required]
        public string Link { get; set; }

    }
}
