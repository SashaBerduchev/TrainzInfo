namespace TrainzInfoWebGW.Tools.DTO
{
    public class TrainDTO
    {
        public int Id { get; set; }
        public int Number { get; set; }

        public string StationFrom { get; set; }
        public string StationTo { get; set; }

        // ID станцій щоб не тягнути повні об’єкти Stations
        public int? FromStationId { get; set; }
        public int? ToStationId { get; set; }

        public string Type { get; set; }
        public string NameOfTrain { get; set; }

        public bool IsUsing { get; set; }

        // Мінімальна інформація про тип поїзда
        public string PassTrainType { get; set; }

        // Для Blazor достатньо тільки кількості пунктів розкладу
        public int TrainsShadulesCount { get; set; }
        public int StationsShadulesCount { get; set; }
    }
}
