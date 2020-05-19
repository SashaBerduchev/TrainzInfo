using System.ComponentModel.DataAnnotations;

namespace TrainzInfo.Models
{
    public class Electic_locomotive
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Speed { get; set; }
        public int SectionCount{ get; set; }
        public int ALlPowerP { get; set; }
    }
}
