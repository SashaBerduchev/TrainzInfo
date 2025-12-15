using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class Stations
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        
        public string Railway { get; set; }
        [Required]
        public string Oblast { get; set; }
        public string Imgsrc { get; set; }
        public string DopImgSrc { get; set; }
        public string DopImgSrcSec { get; set; }
        public string DopImgSrcThd { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public UkrainsRailways UkrainsRailways { get; set; }
        public Oblast Oblasts { get; set; }
        public City Citys { get; set; }
        public ICollection<StationsShadule> StationsShadules { get; set; }
        public StationInfo StationInfo { get; set; }
        public ICollection<RailwayUsersPhoto> railwayUsersPhotos { get; set; }  
        public Metro Metro { get; set; }
        public StationImages StationImages { get; set; }
        public ICollection<Locomotive> Locomotives { get; set; }
        public ICollection<ElectricTrain> ElectricTrains { get; set; }
    }

}
