using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfoModel.Models.Information.Main
{
    public class Metro
    {

        public int id { get; set; }
        public string ObjectName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Information { get; set; }
        [Required]
        public string Photo { get; set; }
        public ICollection<MetroLines> MetroLines { get; set; }
    }
}
