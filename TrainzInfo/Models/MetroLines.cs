using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class MetroLines
    {
        public int id { get; set; }
        [Required]
        public string NameLine { get; set; }
        [Required]
        public int CountStation { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public Metro Metro { get; set; }   
        public ICollection<MetroStation> Stations { get; set; }
    }
}
