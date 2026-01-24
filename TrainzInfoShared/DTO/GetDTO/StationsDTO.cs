using System.ComponentModel.DataAnnotations;

namespace TrainzInfoShared.DTO.GetDTO
{
    public class StationsDTO
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        public string DopImgSrc { get; set; }
        public string DopImgSrcSec { get; set; }
        public string DopImgSrcThd { get; set; }
        public string Image { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        [Required]
        public string UkrainsRailways { get; set; }
        [Required]
        public string Oblasts { get; set; }
        [Required]
        public string Citys { get; set; }
        //public StationsShadulesDTO StationsShadules { get; set; }
        public string StationInfo { get; set; }
        //public RailwayUsersPhotosDTO railwayUsersPhotos { get; set; }
        public string Metro { get; set; }
        public string StationImages { get; set; }
        public List<StationsShadulerDTO> stationsShadulers { get; set; }
    }
}
