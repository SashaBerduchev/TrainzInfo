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
        public string UserName { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime DateTime { get; set;}
        public string LocmotiveName { get; set; }
        public string Marshrute { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string BaseInfo { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }

    }
}
