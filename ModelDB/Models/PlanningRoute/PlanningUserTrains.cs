using Microsoft.AspNetCore.Identity;
using ModelDB.Models.Trains;

namespace ModelDB.Models.PlanningRoute
{
    public class PlanningUserTrains
    {
        public int ID { get; set; }
        public string ObjectName { get; set; }
        public IdentityUser User { get; set; }
        public Train Train { get; set; }
        public int TrainID { get; set; }
        public string Username { get; set; }

    }
}
