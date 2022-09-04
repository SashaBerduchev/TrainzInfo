using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class Plants
    {
        public int id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Adress { get; set; }
    }
}
