using System;

namespace TrainzInfo.Tools.DTO
{
    public class TrainsShaduleDTO
    {
        public int Id { get; set; }

        public string NameStation { get; set; }
        public string NumberTrain { get; set; }

        public TimeSpan Arrival { get; set; }
        public TimeSpan Departure { get; set; }

        public string Distance { get; set; }

        public bool IsUsing { get; set; }

        // Додаємо ID замість цілих об’єктів EF
        public int? StationId { get; set; }
        public int? TrainId { get; set; }
    }
}
