namespace API.DTOs
{
    public class WeightEntryDto
    {
        public Guid? Id { get; set; }
        public DateTime Date { get; set; }
        public double WeightKg { get; set; }
        public string Profile { get; set; } = "default";
    }
}
