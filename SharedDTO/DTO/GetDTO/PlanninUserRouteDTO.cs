using System;
using System.Collections.Generic;
using System.Text;

namespace SharedDTO.DTO.GetDTO
{
    public class PlanninUserRouteDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TrainDTO>Trains { get; set; }
        public List<TrainsShaduleDTO> trainsShadules { get; set; }
    }
}
