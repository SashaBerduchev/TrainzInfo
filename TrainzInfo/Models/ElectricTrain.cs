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
        public string IsProof { get; set; }
        public DepotList DepotList { get; set; }
        public City City { get; set; }
        public Plants Plants { get; set; }
        public SuburbanTrainsInfo Trains { get; set; }
        public Users Users { get; set; }    
        public ElectrickTrainzInformation ElectrickTrainzInformation { get; set; }
    }
}
