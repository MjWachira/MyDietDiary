using API.Models;

namespace API.Services.IServices
{
    public interface IExcelStorageService
    {
        IEnumerable<Meal> ReadMeals();
        void AddMeal(Meal meal);
        void RemoveMeal(Guid id);

        IEnumerable<WeightEntry> ReadWeights();
        void AddWeight(WeightEntry w);
        void RemoveWeight(Guid id);

        Profile GetProfile(string profileName);
        void SetProfile(Profile profile);
    }
}
