
using TrainzInfoShared.DTO;

namespace TrainzInfoShared.RequesDTO;

public class TrainCreateRequest
{
    public TrainDTO Train { get; set; }
    public List<TrainsShaduleDTO> TrainsShedullers { get; set; }
}