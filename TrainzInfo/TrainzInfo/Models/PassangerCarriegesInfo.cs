using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class PassangerCarriegesInfo
    {
        public int id { get; set; }
        [Required]
        public string Type{ get; set; }
        [Required]
        public string Info { get; set; }
    }
}
