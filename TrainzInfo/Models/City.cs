﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class City
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Oblast { get; set; }
        public string Region { get; set; }
        public string IsStationExist { get; set; }
        public ICollection<Stations> Stations { get; set; }
        public Oblast Oblasts  { get; set; }
        public ICollection<ElectricTrain> ElectricTrains { get; set; }
        public ICollection<DepotList> DepotLists { get; set; }

    }
}
