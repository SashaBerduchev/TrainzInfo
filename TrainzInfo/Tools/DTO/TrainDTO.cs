namespace TrainzInfo.Tools.DTO
{
    public class TrainDTO
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string StationFrom { get; set; }

        public string StationTo { get; set; }

        // ID станцій замість об'єктів Stations
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }

        public string Type { get; set; }

        public string NameOfTrain { get; set; }

        public bool IsUsing { get; set; }

        // Якщо TypeOfPassTrain був enum — передаємо як string або int
        public string TypeOfPassTrain { get; set; }
    }
}
