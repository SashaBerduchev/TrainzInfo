using System.ComponentModel.DataAnnotations;

namespace TrainzInfo.Models
{
    public class Locomotive
    {
         public int id { get; set; }
        public string User { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public int Speed { get; set; }
        [Required]
        public int SectionCount { get; set; }
        public string ALlPowerP { get; set; }
        [Required]
        public string Seria { get; set; }
        public string Depot { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        public string DieselPower { get; set; }
        public DepotList DepotList { get; set; }
        public Locomotive_series Locomotive_Series { get; set; }
    }
}
