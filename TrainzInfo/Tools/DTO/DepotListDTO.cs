namespace TrainzInfo.Tools.DTO
{
    public class DepotListDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Назва філії ("Укрзалізниця — XYZ")
        public string UkrainsRailways { get; set; } = string.Empty;

        // Детальна інформація про філію
        public string UkrainsRailway { get; set; }

        // Місто, де знаходиться депо
        public string? City { get; set; }
        public string Oblast { get; set; }

        // Кількість локомотивів / поїздів — Blazor не потребує повних моделей
        public int LocomotivesCount { get; set; }
        public int ElectricTrainsCount { get; set; }
        public int DieselTrainsCount { get; set; }
    }
}
