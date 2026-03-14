using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ModelDB.Models.Dictionaries.Addresses;
using ModelDB.Models.Information.Additional;
using ModelDB.Models.Information.Images;
using ModelDB.Models.Trains;
using ModelDB.Models.UsersInfo;

namespace ModelDB.Models.Information.Main
{
    public class Stations
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
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
        public ICollection<DieselTrains> DieselTrains { get; set; }
    }

}
