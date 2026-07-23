using API.DTOs;
using API.Models;

namespace API.Services.DietService
{
    public interface IDietService
    {
        IEnumerable<Meal> GetMeals(string profile);
        IEnumerable<Meal> GetAllMeals();
        Meal AddMeal(MealDto dto);
        void DeleteMeal(Guid id);

        IEnumerable<WeightEntry> GetWeights(string profile);
        IEnumerable<WeightEntry> GetAllWeights();
        WeightEntry AddWeight(WeightEntryDto dto);
        void DeleteWeight(Guid id);

        IEnumerable<Profile> GetProfiles();
        Profile GetProfile(string profile);
        void UpdateTarget(string profile, int targetCalories);
    }
}
