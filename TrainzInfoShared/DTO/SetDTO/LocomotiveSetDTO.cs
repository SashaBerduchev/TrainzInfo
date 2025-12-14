using System;
using System.Collections.Generic;
using System.Text;

namespace TrainzInfoShared.DTO.SetDTO
{
    public class LocomotiveSetDTO
    {
        public string Number { get; set; }
        public int Speed { get; set; }
        public string Seria { get; set; }

        // Філія / депо / місто / область
        public string Depot { get; set; }
        public string Oblast { get; set; }

        // Зображення
        public string ImgSrc { get; set; }  // URL або base64 string
        public string ImageType { get; set; }  // URL або base64 string
    }
}
