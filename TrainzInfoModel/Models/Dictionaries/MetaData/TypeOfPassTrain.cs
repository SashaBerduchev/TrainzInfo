using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainzInfoModel.Models.Trains;

namespace TrainzInfoModel.Models.Dictionaries.MetaData
{
    public class TypeOfPassTrain
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
        [Required]
        public string Type { get; set; }
        public ICollection<Train> Trains { get; set; }

    }
}
