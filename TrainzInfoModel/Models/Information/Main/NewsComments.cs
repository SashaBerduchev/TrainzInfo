using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrainzInfoModel.Models.Information.Main
{
    public class NewsComments
    {
        public int Id { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public NewsInfo NewsInfo { get; set; }
        public IdentityUser Author { get; set; }
    }
}
