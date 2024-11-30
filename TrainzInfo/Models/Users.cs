using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class Users
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } // имя пользователя
        [Required]
        public int Age { get; set; } // возраст пользователя
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Status { get; set; }
        public string IpAddress { get; set; }
        public string Role { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public ICollection<UserTrainzPhoto> UserTrainzPhotos { get; set; }
        public ICollection<UserLocomotivePhotos> UserLocomotivePhotos { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<IpAdresses> IpAdresses { get; set; }
        public ICollection<RailwayUsersPhoto> RailwayUsersPhotos { get; set; }
        public ICollection<Stations> Stations { get; set; }
    }
}
