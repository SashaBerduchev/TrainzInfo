using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ModelDB.Models.Dictionaries.MetaData;
using ModelDB.Models.Information.Images;

namespace ModelDB.Models.Information.Main
{
    public class NewsInfo
    {
        public int id { get; set; }
        public string ObjectName { get; set; }
        [Required]
        public string NameNews { get; set; }
        [Required]
        public string BaseNewsInfo { get; set; }
        public string NewsInfoAll { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<NewsComments> NewsComments { get; set; }
        public NewsImage NewsImages { get; set; }
        public IdentityUser User { get; set; }
        [Required]
        public string LinkSorce { get; set; }
        public DateOnly DateStartActual { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        [Required]
        public DateOnly DateEndActual { get; set; }
        public StatusObject Status { get; set; }

    }
}
