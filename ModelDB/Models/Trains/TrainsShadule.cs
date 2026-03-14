using System;
using System.Collections.Generic;
using ModelDB.Models.Information.Main;

namespace ModelDB.Models.Trains
{
    public class TrainsShadule
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
        public string NameStation { get; set; }
        public string NumberTrain { get; set; }
        public TimeSpan Arrival { get; set; }
        public TimeSpan Departure { get; set; }
        public string Distance { get; set; }
        public bool IsUsing { get; set; }
        public Stations Stations { get; set; }
        public Train Train { get; set; }
    }
}
