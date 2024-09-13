using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class UkrainsRailways
    {
        public int id { get; set; }
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
        public Users Users { get; set; }
    }
}
