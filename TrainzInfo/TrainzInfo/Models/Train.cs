using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class Train
    {
        public int id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string StationFrom { get; set; }
        [Required]
        public string StationTo { get; set; }
        [Required]
        public string Type { get; set; }
        public string NameOfTrain { get; set; }
        public ICollection<TrainsShadule> TrainsShadules { get; set; }
<<<<<<< HEAD
        public ICollection<StationsShadule> StationsShadules { get; set; }
=======
        public TypeOfPassTrain TypeOfPassTrain { get; set; }
>>>>>>> 4302c7c10c83add875102e7f62efb04210f5681d

    }
}
