using System;
using ModelDB.Models.Information.Additional;

namespace ModelDB.Models.Information.Main
{
    public class DieselTrains
    {
        public int Id { get; set; }
        public string ObjectName { get; set; }
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
