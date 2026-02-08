using System;
using TrainzInfoModel.Models.Information.Additional;

namespace TrainzInfoModel.Models.Information.Main
{
    public class DieselTrains
    {
        public int Id { get; set; }
        public SuburbanTrainsInfo SuburbanTrainsInfo { get; set; }
        public string NumberTrain { get; set; }
        public DepotList DepotList { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public Stations Stations { get; set; }
        public DateTime Create {  get; set; }
        public DateTime Update {  get; set; }

    }
}
