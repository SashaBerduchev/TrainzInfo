﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class MetroStation
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        public int MetroID { get; set; }
        public string MetroLine { get; set; }
        public int MetroLineId { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public MetroLines MetroLines { get; set; }
    }
}
