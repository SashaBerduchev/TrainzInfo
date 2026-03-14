using System;
using System.Collections.Generic;
using System.Text;

namespace SharedDTO.DTO.GetDTO
{
    public class StationEditDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Oblasts { get; set; }
        public string UkrainsRailways { get; set; }
        public string Citys { get; set; }
        public string OldImage { get; set; } // Шлях до старого фото
        public string NewImage { get; set; } // Нове завантажене фото.
        public string NewImageType { get; set; } // Тип нового фото (наприклад, "image/jpeg")
    }
}
