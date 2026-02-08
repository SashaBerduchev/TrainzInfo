using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfoModel.Models.Dictionaries.Addresses;
using TrainzInfoModel.Models.Dictionaries.MetaData;
using TrainzInfoModel.Models.Information.Additional;

namespace TrainzInfoModel.Models.Information.Main
{
    public class ElectricTrain
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
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
        public Stations Stations { get; set; }
        public DateTime Create { get; set; }
        public DateTime Update { get; set; }
    }
}
