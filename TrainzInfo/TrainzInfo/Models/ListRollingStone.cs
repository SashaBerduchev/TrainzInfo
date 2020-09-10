using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class ListRollingStone
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Depot { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Photo { get; set; }
    }
}
