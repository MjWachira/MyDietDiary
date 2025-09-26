using API.DTOs;
using API.Models;

namespace API.Services.DietService
{
    public interface IDietService
    {
        IEnumerable<Meal> GetMeals(string profile);
        Meal AddMeal(MealDto dto);
        void DeleteMeal(Guid id);

        IEnumerable<WeightEntry> GetWeights(string profile);
        WeightEntry AddWeight(WeightEntryDto dto);
        void DeleteWeight(Guid id);

        Profile GetProfile(string profile);
        void UpdateTarget(string profile, int targetCalories);
    }
}
