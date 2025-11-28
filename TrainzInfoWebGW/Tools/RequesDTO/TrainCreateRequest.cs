using TrainzInfoWebGW.Tools.DTO;

namespace TrainzInfoWebGW.Tools.RequesDTO
{
    public class TrainCreateRequest
    {
        public TrainDTO Train { get; set; }
        public List<TrainsShedullerDTO> TrainsShedullers { get; set; }
    }
}
