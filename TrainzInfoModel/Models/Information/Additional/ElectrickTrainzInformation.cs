using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfoModel.Models.Information.Additional
{
    public class ElectrickTrainzInformation
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string AllInformation { get; set; }
        [Required]
        public string Imgsrc { get; set; }
    }
}
