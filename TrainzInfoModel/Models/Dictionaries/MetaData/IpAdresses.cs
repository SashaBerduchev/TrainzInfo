using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfoModel.Models.Dictionaries.MetaData
{
    public class IpAdresses
    {
        public int id { get; set; }
        public string IpAddres { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}
