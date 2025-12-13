using System;
using System.Collections.Generic;
using System.Text;

namespace TrainzInfoShared.DTO.SetDTO
{
    public class ElectricTrainSetDTO
    {
        public int id { get; set; }

        public string Name { get; set; }

        public string Model { get; set; }

        public int MaxSpeed { get; set; }

        public string Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public string DepotList { get; set; }
    }
}
