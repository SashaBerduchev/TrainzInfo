using SharedDTO.DTO.GetDTO;

namespace SharedDTO.RequesDTO;

public class TrainCreateRequest
{
    public TrainDTO Train { get; set; }
    public List<TrainsShaduleDTO> TrainsShedullers { get; set; }
}