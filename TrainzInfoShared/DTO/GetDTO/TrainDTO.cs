using System.ComponentModel.DataAnnotations;

namespace TrainzInfoShared.DTO.GetDTO
{
    public class TrainDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Номер обо'язково")]
        public int Number { get; set; }
        [Required(ErrorMessage = "Станція відправлення обов'язково")]
        public string StationFrom { get; set; }
        [Required(ErrorMessage = "Станція прибуття обов'язкова")]
        public string StationTo { get; set; }

        // ID станцій щоб не тягнути повні об’єкти Stations
        public int? FromStationId { get; set; }
        public int? ToStationId { get; set; }
        
        public string? Type { get; set; }
        public string? NameOfTrain { get; set; }

        public bool IsUsing { get; set; }

        // Мінімальна інформація про тип поїзда
        [Required(ErrorMessage = "Тип поїзда обов'язково")]
        public string PassTrainType { get; set; }

        // Для Blazor достатньо тільки кількості пунктів розкладу
        public int TrainsShadulesCount { get; set; }
        public int StationsShadulesCount { get; set; }
    }
}
