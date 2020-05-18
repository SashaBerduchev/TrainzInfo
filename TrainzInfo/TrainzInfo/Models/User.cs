using System.ComponentModel.DataAnnotations;

namespace TrainzInfo.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } // имя пользователя
        public int Age { get; set; } // возраст пользователя
    }
}
