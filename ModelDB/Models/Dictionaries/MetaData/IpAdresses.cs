using Microsoft.AspNetCore.Identity;

namespace ModelDB.Models.Dictionaries.MetaData
{
    public class IpAdresses
    {
        public int id { get; set; }
        public string IpAddres { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
        public bool IsActive { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}
