using System;
using System.Collections.Generic;

namespace TrainzInfo.Models
{
    public class TrainsShadule
    {
        public int id { get; set; }
        public string NameStation { get; set; }
        public string NumberTrain { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public string Distance { get; set; }
        public Stations Stations { get; set; }
        public Train Train { get; set; }
    }
}
