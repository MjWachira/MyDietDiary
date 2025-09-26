namespace API.Models
{
    public class WeightEntry
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double WeightKg { get; set; }
        public string Profile { get; set; } = "default";
    }
}
