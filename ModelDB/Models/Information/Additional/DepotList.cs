using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ModelDB.Models.Dictionaries.Addresses;
using ModelDB.Models.Information.Main;

namespace ModelDB.Models.Information.Additional
{
    public class DepotList
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string UkrainsRailways { get; set; }
        public City City { get; set; }
        public UkrainsRailways UkrainsRailway { get; set; }
        public ICollection<Locomotive> Locomotives { get; set; }
        public ICollection<ElectricTrain> ElectricTrains { get; set; }
        public ICollection<DieselTrains> DieselTrains { get; set; }
    }
}
