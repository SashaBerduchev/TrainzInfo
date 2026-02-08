using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfoModel.Models.Information.Main;

namespace TrainzInfoModel.Models.Information.Additional
{
    public class Locomotive_series
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
        [Required]
        public string Seria { get; set; }
        public ICollection<Locomotive> Locomotives { get; set; } 
    }
}
