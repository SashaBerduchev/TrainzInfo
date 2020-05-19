using System.ComponentModel.DataAnnotations;

namespace TrainzInfo.Models
{
    public class LocomotivesType
    {
        public int id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Type { get; set; }
        
    }
}
