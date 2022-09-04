using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class LocomotiveBaseInfo
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string BaseInfo { get; set; }
    }
}
