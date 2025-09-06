using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class MainImages
    {
        public int id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
    }
}
