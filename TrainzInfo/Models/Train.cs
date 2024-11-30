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
        public Users User { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string StationFrom { get; set; }
        [Required]
        public string StationTo { get; set; }
        public Stations From { get; set; }
        public Stations To { get; set; }
        [Required]
        public string Type { get; set; }
        public string NameOfTrain { get; set; }
        public ICollection<TrainsShadule> TrainsShadules { get; set; }
        public ICollection<StationsShadule> StationsShadules { get; set; }

        public TypeOfPassTrain TypeOfPassTrain { get; set; }


    }
}
