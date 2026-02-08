using TrainzInfoModel.Models.Dictionaries.Addresses;
using TrainzInfoModel.Models.Dictionaries.MetaData;
using TrainzInfoModel.Models.Information.Additional;
using TrainzInfoModel.Models.Information.Images;
using TrainzInfoModel.Models.Information.Main;
using TrainzInfoModel.Models.PlanningRoute;
using TrainzInfoModel.Models.Trains;
using TrainzInfoModel.Models.UsersInfo;

namespace TrainzInfoModel.Models.System
{
    public class DocumentToIndex
    {
        public int id { get; set; }
        public string NameObject { get; set; }
        public string SearchContent { get; set; }
        public string Path { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
        public Locomotive Locomotive { get; set; }
        public NewsInfo NewsInfo { get; set; }
        public NewsComments NewsComments { get; set; }
        public NewsImage NewsImage { get; set; }
        public UserLocomotivePhotos UserLocomotivePhoto { get; set; }
        public ElectricTrain Electric { get; set; }
        public UserTrainzPhoto UserTrainzPhoto { get; set; }
        public ElectrickTrainzInformation ElectrickTrainzInformation { get; set; }
        public UkrainsRailways UkrainsRailways { get; set; }
        public DepotList Depots { get; set; }
        public City Cities { get; set; }
        public Oblast Oblasts { get; set; }
        public Stations Stations { get; set; }
        public TypeOfPassTrain TypeOfPassTrains { get; set; }
        public Train Trains { get; set; }
        public StationsShadule StationsShadule { get; set; }
        public Locomotive_series Locomotive_Series { get; set; }
        public LocomotiveBaseInfo LocomotiveBaseInfos { get; set; }
        public StationInfo StationInfos { get; set; }
        public Plants Plants { get; set; }
        public SuburbanTrainsInfo SuburbanTrainsInfos { get; set; }
        public IpAdresses IpAdresses { get; set; }
        public RailwayUsersPhoto RailwayUsersPhotos { get; set; }
        public MainImages MainImages { get; set; }
        public Metro Metros { get; set; }
        public MetroStation MetroStations { get; set; }
        public MetroLines MetroLines { get; set; }
        public TrainsShadule TrainsShadule { get; set; }
        public DieselTrains DieselTrains { get; set; }
        public StationImages StationImage { get; set; }
        public MailSettings MailSettings { get; set; }
        public SendEmail SendEmails { get; set; }
        public PlanningUserTrains PlanningUserTrain { get; set; }
        public PlanningUserRoute PlanningUserRoute { get; set; }
        public PlanningUserRouteSave PlanningUserRouteSaves { get; set; }

    }
}
