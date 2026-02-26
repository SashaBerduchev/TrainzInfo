using System.ComponentModel.DataAnnotations;

namespace TrainzInfoShared.DTO.SetDTO
{
    public class NewsSetDTO
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
        [Required(ErrorMessage = "Назва обо'язково")]
        public string NameNews { get; set; }
        [Required(ErrorMessage = "Головна частина обо'язково")]
        public string BaseNewsInfo { get; set; }
        [Required(ErrorMessage = "Вся новина обо'язково")]
        public string NewsInfoAll { get; set; }
        public string DateTime { get; set; }
        [Required(ErrorMessage = "Дата початку актуалізації обо'язково")]
        public DateOnly DateEndActual { get; set; }
        [Required(ErrorMessage = "Посилання на першоджерело обо'язково")]
        public string LinkSorce { get; set; }
        [Required(ErrorMessage = "Зображення обо'язково")]
        public string NewsImage { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public string username { get; set; }
    }
}
