using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class ElectricTrain
    {
        public int id { get; set; }
        [Required]
        public string User { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string VagonsCountP { get; set; }
        [Required]
        public int MaxSpeed { get; set; }
        [Required]
        public string DepotTrain { get; set; }
        public string DepotCity { get; set; }
        public DateTime LastKvr { get; set; }
        public DateTime Created { get; set; }
        public string Plant { get; set; }
        public string PlaceKvr{ get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
    }
}
