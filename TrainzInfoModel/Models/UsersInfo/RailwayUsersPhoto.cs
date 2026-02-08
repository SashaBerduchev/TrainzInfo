using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfoModel.Models.Information.Main;

namespace TrainzInfoModel.Models.UsersInfo
{
    public class RailwayUsersPhoto
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
        public string Information { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public string IsProof { get; set; }
        public Stations Stations { get; set; }
    }
}
