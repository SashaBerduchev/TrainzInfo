using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class TrainzStations
    {
        public int id { get; set; }
        [Required]
        public int NumberOFTrain { get; set; }
        [Required]
        public string NameStationStop { get; set; }
        public DateTime TimeOfArrive { get; set; }
        public DateTime TimeOfDepet { get; set; }
    }
}
