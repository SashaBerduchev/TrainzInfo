using Microsoft.AspNetCore.Identity;

namespace TrainzInfo.Models
{
    public class PlanningUserTrains
    {
        public int ID { get; set; }
        public IdentityUser User { get; set; }
        public Train Train { get; set; }
        public int TrainID { get; set; }
        public string Username { get; set; }

    }
}
