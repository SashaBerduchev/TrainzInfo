using TrainzInfo.Models;

namespace TrainzInfo.Tools.DTO
{
    public class DieselTrainsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SuburbanTrainsInfo { get; set; }
        public string NumberTrain { get; set; }
        public string DepotList { get; set; }
        public string Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public string Oblast { get; set; }
        public string City { get; set; }
        public string Filia { get; set; }
    }
}
