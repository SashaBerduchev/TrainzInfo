using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfoModel.Models.Information.Additional
{
    public class StationInfo
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string BaseInfo { get; set; }
        [Required]
        public string AllInfo { get; set; }
    }
}
