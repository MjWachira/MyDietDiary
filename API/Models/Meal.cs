namespace API.Models
{
    public class Meal
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Calories { get; set; }
        public string Profile { get; set; } = "default";
    }
}
