using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ModelDB.Models.Information.Main;

namespace ModelDB.Models.Information.Additional
{
    public class SuburbanTrainsInfo
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string BaseInfo { get; set; }
        [Required]
        public string AllInfo { get; set; }
        public ICollection<ElectricTrain> ElectricTrain { get; set; }
        public ICollection<DieselTrains> DieselTrains { get; set; }
    }
}
