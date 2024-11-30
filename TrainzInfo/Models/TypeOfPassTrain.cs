using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class TypeOfPassTrain
    {
        public int id { get; set; }
        [Required]
        public string Type { get; set; }
        public ICollection<Train> Trains { get; set; }

    }
}
