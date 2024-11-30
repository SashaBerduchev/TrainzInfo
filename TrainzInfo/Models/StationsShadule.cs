using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class StationsShadule
    {
        public int id { get; set; }
        [Required]
        public string Station { get; set; }
        [Required]
        public string UzFilia { get; set; }
        public DateTime TimeOfArrive { get; set; }
        public DateTime TimeOfDepet { get; set; }
        [Required]
        public int NumberTrain { get; set; }
        public UkrainsRailways UkrainsRailways { get; set; }
        public Train Train { get; set; }
        public Stations Stations { get; set; }
    }
}
