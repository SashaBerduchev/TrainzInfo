using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TrainzInfo.Models
{
    public class PlanningUserRoute
    {
        public int ID { get; set; }
        public IdentityUser User { get; set; }
        public ICollection<TrainsShadule> TrainsShadule { get; set; }
        public ICollection<int> TrainsShaduleID { get; set; }
        public string Username { get; set; }

    }
}
