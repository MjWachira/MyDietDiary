namespace API.DTOs
{
    public class MealDto
    {
        public Guid? Id { get; set; } // optional on create
        public DateTime Date { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Calories { get; set; }
        public string Profile { get; set; } = "default";
    }
}
