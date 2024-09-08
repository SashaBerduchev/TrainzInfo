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
        [Required]
        public string Railway { get; set; }
        [Required]
        public string Oblast { get; set; }
        public string Imgsrc { get; set; }
        public string DopImgSrc { get; set; }
        public string DopImgSrcSec { get; set; }
        public string DopImgSrcThd { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public int UserId { get; set; }
        public UkrainsRailways UkrainsRailways { get; set; }
        public Oblast Oblasts { get; set; }
        public City Citys { get; set; }
        public ICollection<TrainsShadule> TrainsShadules { get; set; }
        public ICollection<StationsShadule> StationsShadules { get; set; }
    }

}
