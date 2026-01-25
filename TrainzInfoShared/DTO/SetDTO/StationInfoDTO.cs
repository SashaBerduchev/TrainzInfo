using System.ComponentModel.DataAnnotations;

namespace TrainzInfoShared.DTO.SetDTO
{
    public class StationInfoDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string BaseInfo { get; set; }
        [Required]
        public string AllInfo { get; set; }
    }
}
