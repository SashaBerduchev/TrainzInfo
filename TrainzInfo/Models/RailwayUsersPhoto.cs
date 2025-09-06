using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class RailwayUsersPhoto
    {
        public int id { get; set; }
        public string Information { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public string IsProof { get; set; }
        public Stations Stations { get; set; }
    }
}
