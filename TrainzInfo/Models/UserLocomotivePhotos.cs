using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class UserLocomotivePhotos
    {
        public int Id { get; set; }
        public string NameLocomotive { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string BaseInfo { get; set; }
        [Required]
        public string AllInfo{ get; set; }
        public DateTime DateTime { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public Locomotive Locomotive { get; set; }
    }
}
