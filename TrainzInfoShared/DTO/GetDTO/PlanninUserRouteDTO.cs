using System;
using System.Collections.Generic;
using System.Text;

namespace TrainzInfoShared.DTO.GetDTO
{
    public class PlanninUserRouteDTO
    {
        public string Name { get; set; }
        public List<TrainDTO>Trains { get; set; }
        public List<TrainsShaduleDTO> trainsShadules { get; set; }
    }
}
