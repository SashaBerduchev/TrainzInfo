using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class IpAdresses
    {
        public int id { get; set; }
        public string IpAddres { get; set; }
        public DateTime Date { get; set; }
        public Users Users { get; set; }
    }
}
