using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TrainzInfoShared.DTO.GetDTO
{
    public class StationsDetailsDTO
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
        public string? BaseInfo { get; set; }
        public string? AllInfo { get; set; }
        public string Metro { get; set; }
        public byte[] ImageBytes { get; set; }
        public string ImageMime { get; set; }

        // А це поле автоматично згенерує рядок, коли до нього звернуться
        public string StationImages => ImageBytes != null
            ? $"data:{ImageMime};base64,{Convert.ToBase64String(ImageBytes)}"
            : null;
        public List<StationsShadulerDTO> stationsShadulers { get; set; }
    }
}
