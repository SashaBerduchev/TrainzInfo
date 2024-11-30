using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public string NameRole { get; set; }
        public string Rules { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
