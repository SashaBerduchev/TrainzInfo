using TrainzInfoShared.DTO.GetDTO;

namespace TrainzInfoShared.RequesDTO;

public class TrainCreateRequest
{
    public TrainDTO Train { get; set; }
    public List<TrainsShaduleDTO> TrainsShedullers { get; set; }
}