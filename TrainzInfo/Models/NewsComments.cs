using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainzInfo.Models
{
    public class NewsComments
    {
        public int Id { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public NewsInfo NewsInfo { get; set; }
    }
}
