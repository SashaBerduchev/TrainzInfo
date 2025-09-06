using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class Train
    {
        public int id { get; set; }
        [Required(ErrorMessage ="Уведіть номер")]
        public int Number { get; set; }
        [Required(ErrorMessage = "Вкажіть станцію початку руху")]
        public string StationFrom { get; set; }
        [Required (ErrorMessage ="Вкажіть станцію кінця руху")]
        public string StationTo { get; set; }
        public Stations From { get; set; }
        public Stations To { get; set; }
        [Required(ErrorMessage ="Вкажіть тип поїзда")]
        public string Type { get; set; }
        public string NameOfTrain { get; set; }
        public bool IsUsing { get; set; }
        public ICollection<TrainsShadule> TrainsShadules { get; set; }
        public ICollection<StationsShadule> StationsShadules { get; set; }

        public TypeOfPassTrain TypeOfPassTrain { get; set; }


    }
}
