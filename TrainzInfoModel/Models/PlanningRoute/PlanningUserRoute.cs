using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TrainzInfoModel.Models.Trains;

namespace TrainzInfoModel.Models.PlanningRoute
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
