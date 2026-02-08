using System.ComponentModel.DataAnnotations.Schema;
using TrainzInfoModel.Models.Information.Main;

namespace TrainzInfoModel.Models.Information.Images
{
    public class NewsImage
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int NewsInfoId { get; set; }
    }
}
