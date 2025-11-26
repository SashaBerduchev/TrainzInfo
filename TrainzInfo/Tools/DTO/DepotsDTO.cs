namespace TrainzInfo.Tools.DTO
{
    public class DepotsDTO
    {
        public int Id { get; set; }

        // Назва станції
        public string Name { get; set; }

        // Назва філії (УЗ)
        public string UkrainsRailways { get; set; }

        // Назва міста
        public string City { get; set; }

        // Назва області
        public string Oblast { get; set; }
        public int LocomotivesCount { get; set; }
        public int ElectricTrainsCount { get; set; }
        public int DieselTrainsCount { get; set; }
    }
}
