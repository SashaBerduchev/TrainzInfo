namespace TrainzInfoShared.DTO.GetDTO
{
    public class LocomotiveDTO
    {
        public int Id { get; set; }

        // Основна інформація
        public string Number { get; set; }
        public int Speed { get; set; }
        public string Seria { get; set; }
        public string BaseInfo { get; set; }
        public string AllInfo { get; set; }


        // Філія / депо / місто / область
        public string Filia { get; set; }
        public string Depot { get; set; }
        public string City { get; set; }
        public string Oblast { get; set; }
        public string Station { get; set; }
        public string StationInformation { get; set; }
        public string StationImages { get; set; }

        // Зображення
        public string ImgSrc { get; set; }  // URL або base64 string
    }
}
