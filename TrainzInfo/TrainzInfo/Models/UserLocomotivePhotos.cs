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
        public string UserName { get; set; }
        [Required]
        public string UserSername { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string BaseInfo { get; set; }
        [Required]
        public string AllInfo{ get; set; }
        [Required]
        public string PhotoLink { get; set; }
        
    }
}
