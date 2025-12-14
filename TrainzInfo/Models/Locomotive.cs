using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainzInfo.Models
{
    public class Locomotive
    {
         public int id { get; set; }
        public string User { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public int Speed { get; set; }
        [Required]
        public string Seria { get; set; }
        public string Depot { get; set; }
        public DateTime Create { get; set; }
        public DateTime Update { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public DepotList DepotList { get; set; }
        public Locomotive_series Locomotive_Series { get; set; }
        public ICollection<UserLocomotivePhotos> UserLocomotivesPhoto { get; set; }
        public LocomotiveBaseInfo LocomotiveBaseInfo { get; set; }
        public Stations Stations { get; set; }

    }
}
