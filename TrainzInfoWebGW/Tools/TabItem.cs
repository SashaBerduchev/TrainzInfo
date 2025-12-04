namespace TrainzInfoWebGW.Tools
{
    public class TabItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = "";
        public Type ComponentType { get; set; }
    }
}
