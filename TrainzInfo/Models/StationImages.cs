using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainzInfo.Models
{
    public class StationImages
    {
        public int id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
