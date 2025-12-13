using System.Collections.Generic;
using TrainzInfoShared.DTO.GetDTO;

namespace TrainzInfo.Tools.RequestDTO
{
    public class TrainCreateRequest
    {
        public TrainDTO Train { get; set; }
        public List<TrainsShaduleDTO> TrainsShedullers { get; set; }
    }
}
