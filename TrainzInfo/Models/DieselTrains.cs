namespace TrainzInfo.Models
{
    public class DieselTrains
    {
        public int Id { get; set; }
        public SuburbanTrainsInfo SuburbanTrainsInfo { get; set; }
        public string NumberTrain { get; set; }
        public DepotList DepotList { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }

    }
}
