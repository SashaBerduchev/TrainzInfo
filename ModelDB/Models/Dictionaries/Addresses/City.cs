using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ModelDB.Models.Information.Additional;
using ModelDB.Models.Information.Main;

namespace ModelDB.Models.Dictionaries.Addresses
{
    public class City
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
        [Required]
        public string Name { get; set; }
        public string Oblast { get; set; }
        public string Region { get; set; }
        public string IsStationExist { get; set; }
        public ICollection<Stations> Stations { get; set; }
        public Oblast Oblasts  { get; set; }
        public ICollection<ElectricTrain> ElectricTrains { get; set; }
        public ICollection<DepotList> DepotLists { get; set; }

    }
}
