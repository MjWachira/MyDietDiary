using API.DTOs;
using API.Models;
using API.Services.IServices;

namespace API.Services.DietService
{
    public class DietService : IDietService
    {
        private readonly IExcelStorageService _store;

        public DietService(IExcelStorageService store)
        {
            _store = store;
        }

        public IEnumerable<Meal> GetMeals(string profile)
        {
            return _store.ReadMeals().Where(m => string.Equals(m.Profile, profile ?? "default", StringComparison.OrdinalIgnoreCase));
        }

        public Meal AddMeal(MealDto dto)
        {
            var meal = new Meal
            {
                Id = dto.Id ?? Guid.NewGuid(),
                Date = dto.Date.ToUniversalTime(),
                Name = dto.Name,
                Calories = dto.Calories,
                Profile = string.IsNullOrWhiteSpace(dto.Profile) ? "default" : dto.Profile
            };

            _store.AddMeal(meal);
            return meal;
        }

        public void DeleteMeal(Guid id) => _store.RemoveMeal(id);

        public IEnumerable<WeightEntry> GetWeights(string profile)
        {
            return _store.ReadWeights().Where(w => string.Equals(w.Profile, profile ?? "default", StringComparison.OrdinalIgnoreCase));
        }

        public WeightEntry AddWeight(WeightEntryDto dto)
        {
            var w = new WeightEntry
            {
                Id = dto.Id ?? Guid.NewGuid(),
                Date = dto.Date.ToUniversalTime(),
                WeightKg = dto.WeightKg,
                Profile = string.IsNullOrWhiteSpace(dto.Profile) ? "default" : dto.Profile
            };
            _store.AddWeight(w);
            return w;
        }

        public void DeleteWeight(Guid id) => _store.RemoveWeight(id);

        public Profile GetProfile(string profile)
        {
            return _store.GetProfile(profile ?? "default");
        }

        public void UpdateTarget(string profile, int targetCalories)
        {
            var p = new Profile { ProfileName = profile ?? "default", TargetCalories = targetCalories };
            _store.SetProfile(p);
        }
    }
}
