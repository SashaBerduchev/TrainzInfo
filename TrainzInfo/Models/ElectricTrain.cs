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
        public int MaxSpeed { get; set; }
        [Required]
        public string DepotTrain { get; set; }
        public string DepotCity { get; set; }
        public DateOnly LastKvr { get; set; }
        public DateOnly CreatedTrain { get; set; }
        public string PlantCreate { get; set; }
        public string PlantKvr{ get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public string IsProof { get; set; }
        public DepotList DepotList { get; set; }
        public City City { get; set; }
        public Plants PlantsCreate { get; set; }
        public Plants PlantsKvr { get; set; }
        public SuburbanTrainsInfo Trains { get; set; }
        public ElectrickTrainzInformation ElectrickTrainzInformation { get; set; }
    }
}
