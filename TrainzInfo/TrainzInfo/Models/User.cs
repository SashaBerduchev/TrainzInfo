using System.ComponentModel.DataAnnotations;

namespace TrainzInfo.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } // имя пользователя
        [Required]
        public int Age { get; set; } // возраст пользователя
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Status { get; set; }
    }
}
