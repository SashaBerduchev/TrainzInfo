using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfoModel.Models.Information.Additional;
using TrainzInfoModel.Models.Trains;

namespace TrainzInfoModel.Models.Information.Main
{
    public class UkrainsRailways
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Information { get; set; }
        public string Photo { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public ICollection<DepotList> DepotLists { get; set; }
        public ICollection<Stations> Stations { get; set; }
        public ICollection<StationsShadule> stationsShadules { get; set; }
    }
}
