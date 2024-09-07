using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class Oblast
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string OblCenter { get; set; }
        public ICollection<Stations> Stations { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
