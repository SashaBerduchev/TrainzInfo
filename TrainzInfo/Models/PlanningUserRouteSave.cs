using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;

namespace TrainzInfo.Models
{
    public class PlanningUserRouteSave
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Depeat { get; set; }
        public string Arrive { get; set; }
        public ICollection<PlanningUserRoute> PlanningUserRoute { get; set; }
        public ICollection<PlanningUserTrains> PlanningUserTrains { get; set; }
        public string Username { get; set; }
        public IdentityUser User { get; set; }
    }
}
