using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using ModelDB.Models.Trains;

namespace ModelDB.Models.PlanningRoute
{
    public class PlanningUserRoute
    {
        public int ID { get; set; }
        public string ObjectName { get; set; }
        public IdentityUser User { get; set; }
        public ICollection<TrainsShadule> TrainsShadule { get; set; }
        public ICollection<int> TrainsShaduleID { get; set; }
        public string Username { get; set; }

    }
}
