namespace API.DTOs
{
    public class AdminDashboardDto
    {
        public IEnumerable<API.Models.Profile> Profiles { get; set; } = Array.Empty<API.Models.Profile>();
        public IEnumerable<API.Models.Meal> Meals { get; set; } = Array.Empty<API.Models.Meal>();
        public IEnumerable<API.Models.WeightEntry> Weights { get; set; } = Array.Empty<API.Models.WeightEntry>();
    }
}
