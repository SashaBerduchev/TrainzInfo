using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TrainzInfoShared.DTO.GetDTO
{
    public class StationsShadulerDTO
    {
        public int id { get; set; }
        public int Train { get; set; }
        public string TrainFrom { get; set; }
        public string TrainTo { get; set; }
        public string UzFilia { get; set; }
        public TimeSpan TimeOfArrive { get; set; }
        public TimeSpan TimeOfDepet { get; set; }
        public int NumberTrain { get; set; }
        public bool IsUsing { get; set; }
    }
}
