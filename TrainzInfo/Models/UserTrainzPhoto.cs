using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class UserTrainzPhoto
    {
        public int id { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime DateTime { get; set;}
        [Required]
        public string Type { get; set; }
        [Required]
        public string BaseInfo { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public Users Userid { get; set; }
        public Stations Stations { get; set; }
    }
}
