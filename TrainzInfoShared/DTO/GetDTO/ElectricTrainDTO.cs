namespace TrainzInfoShared.DTO.GetDTO
{
    public class ElectricTrainDTO
    {
        public int id { get; set; }

        public string Name { get; set; }

        public string Model { get; set; }

        public int MaxSpeed { get; set; }

        public string DepotTrain { get; set; }
        public string DepotCity { get; set; }
        public DateOnly LastKvr { get; set; }
        public DateOnly CreatedTrain { get; set; }
        public string PlantCreate { get; set; }
        public string PlantKvr { get; set; }
        public string Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public string DepotList { get; set; }
        public string Oblast { get; set; }
        public string UkrainsRailway { get; set; }
        public string City { get; set; }
        public string PlantsCreate { get; set; }
        public string PlantsKvr { get; set; }
        public string TrainsInfo { get; set; }
        public string ElectrickTrainzInformation { get; set; }
    }
}
