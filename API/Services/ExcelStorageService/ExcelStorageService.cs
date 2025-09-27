using API.Models;
using API.Services.IServices;
using ClosedXML.Excel;
using System.Globalization;

namespace API.Services.ExcelStorageService
{
    public class ExcelStorageService : IExcelStorageService
    {
        private readonly string _filePath;
        private readonly object _lock = new();

        private const string MealsSheet = "Meals";
        private const string WeightsSheet = "Weights";
        private const string ProfilesSheet = "Profiles";

        public ExcelStorageService(string filePath)
        {
            _filePath = filePath;
            EnsureFileAndSheets();
        }

        private void EnsureFileAndSheets()
        {
            lock (_lock)
            {
                if (!File.Exists(_filePath))
                {
                    using var wb = new XLWorkbook();
                    var s1 = wb.Worksheets.Add(MealsSheet);
                    s1.Cell(1, 1).Value = "Id";
                    s1.Cell(1, 2).Value = "Date";
                    s1.Cell(1, 3).Value = "Name";
                    s1.Cell(1, 4).Value = "Calories";
                    s1.Cell(1, 5).Value = "Profile";

                    var s2 = wb.Worksheets.Add(WeightsSheet);
                    s2.Cell(1, 1).Value = "Id";
                    s2.Cell(1, 2).Value = "Date";
                    s2.Cell(1, 3).Value = "WeightKg";
                    s2.Cell(1, 4).Value = "Profile";

                    var s3 = wb.Worksheets.Add(ProfilesSheet);
                    s3.Cell(1, 1).Value = "Profile";
                    s3.Cell(1, 2).Value = "TargetCalories";

                    wb.SaveAs(_filePath);
                }
                else
                {
                    // make sure sheets exist
                    using var wb = new XLWorkbook(_filePath);
                    if (!wb.Worksheets.Contains(MealsSheet)) wb.Worksheets.Add(MealsSheet);
                    if (!wb.Worksheets.Contains(WeightsSheet)) wb.Worksheets.Add(WeightsSheet);
                    if (!wb.Worksheets.Contains(ProfilesSheet)) wb.Worksheets.Add(ProfilesSheet);
                    wb.Save();
                }
            }
        }

        public IEnumerable<Meal> ReadMeals()
        {
            lock (_lock)
            {
                using var wb = new XLWorkbook(_filePath);
                var ws = wb.Worksheet(MealsSheet);
                var rows = ws.RowsUsed().Skip(1); // skip header
                var list = new List<Meal>();
                foreach (var r in rows)
                {
                    try
                    {
                        var id = Guid.Parse(r.Cell(1).GetString());
                        var dateStr = r.Cell(2).GetString();
                        var date = DateTime.Parse(dateStr, null, DateTimeStyles.RoundtripKind);
                        var name = r.Cell(3).GetString();
                        var calories = (int)r.Cell(4).GetDouble();
                        var profile = r.Cell(5).GetString();
                        list.Add(new Meal { Id = id, Date = date, Name = name, Calories = calories, Profile = profile });
                    }
                    catch
                    {
                        // skip malformed rows
                    }
                }
                return list;
            }
        }

        public void AddMeal(Meal meal)
        {
            lock (_lock)
            {
                using var wb = new XLWorkbook(_filePath);
                var ws = wb.Worksheet(MealsSheet);
                var last = ws.LastRowUsed();
                var row = last == null ? 2 : last.RowNumber() + 1;
                ws.Cell(row, 1).Value = meal.Id.ToString();
                ws.Cell(row, 2).Value = meal.Date.ToString("o"); // round-trip
                ws.Cell(row, 3).Value = meal.Name;
                ws.Cell(row, 4).Value = meal.Calories;
                ws.Cell(row, 5).Value = meal.Profile;
                wb.Save();
            }
        }

        public void RemoveMeal(Guid id)
        {
            lock (_lock)
            {
                using var wb = new XLWorkbook(_filePath);
                var ws = wb.Worksheet(MealsSheet);
                var rows = ws.RowsUsed().Skip(1);
                foreach (var r in rows)
                {
                    if (r.Cell(1).GetString().Equals(id.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        r.Delete();
                        break;
                    }
                }
                wb.Save();
            }
        }

        public IEnumerable<WeightEntry> ReadWeights()
        {
            lock (_lock)
            {
                using var wb = new XLWorkbook(_filePath);
                var ws = wb.Worksheet(WeightsSheet);
                var rows = ws.RowsUsed()?.Skip(1) ?? Enumerable.Empty<IXLRow>();
                var list = new List<WeightEntry>();
                foreach (var r in rows)
                {
                    try
                    {
                        var id = Guid.Parse(r.Cell(1).GetString());
                        var date = DateTime.Parse(r.Cell(2).GetString(), null, DateTimeStyles.RoundtripKind);
                        var kg = r.Cell(3).GetDouble();
                        var profile = r.Cell(4).GetString();
                        list.Add(new WeightEntry { Id = id, Date = date, WeightKg = kg, Profile = profile });
                    }
                    catch
                    {
                        // skip
                    }
                }
                return list;
            }
        }

        public void AddWeight(WeightEntry w)
        {
            lock (_lock)
            {
                using var wb = new XLWorkbook(_filePath);
                var ws = wb.Worksheet(WeightsSheet);
                var last = ws.LastRowUsed();
                var row = last == null ? 2 : last.RowNumber() + 1;
                ws.Cell(row, 1).Value = w.Id.ToString();
                ws.Cell(row, 2).Value = w.Date.ToString("o");
                ws.Cell(row, 3).Value = w.WeightKg;
                ws.Cell(row, 4).Value = w.Profile;
                wb.Save();
            }
        }

        public void RemoveWeight(Guid id)
        {
            lock (_lock)
            {
                using var wb = new XLWorkbook(_filePath);
                var ws = wb.Worksheet(WeightsSheet);
                var rows = ws.RowsUsed().Skip(1);
                foreach (var r in rows)
                {
                    if (r.Cell(1).GetString().Equals(id.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        r.Delete();
                        break;
                    }
                }
                wb.Save();
            }
        }

        public Profile GetProfile(string profileName)
        {
            lock (_lock)
            {
                using var wb = new XLWorkbook(_filePath);
                var ws = wb.Worksheet(ProfilesSheet);
                var rows = ws.RowsUsed()?.Skip(1) ?? Enumerable.Empty<IXLRow>();
                foreach (var r in rows)
                {
                    if (r.Cell(1).GetString().Equals(profileName, StringComparison.OrdinalIgnoreCase))
                    {
                        var target = (int)r.Cell(2).GetDouble();
                        return new Profile { ProfileName = profileName, TargetCalories = target };
                    }
                }
                // default if not found
                return new Profile { ProfileName = profileName, TargetCalories = 2000 };
            }
        }

        public void SetProfile(Profile profile)
        {
            lock (_lock)
            {
                using var wb = new XLWorkbook(_filePath);
                var ws = wb.Worksheet(ProfilesSheet);
                var rows = ws.RowsUsed()?.Skip(1) ?? Enumerable.Empty<IXLRow>();

                bool found = false;
                foreach (var r in rows)
                {
                    if (r.Cell(1).GetString().Equals(profile.ProfileName, StringComparison.OrdinalIgnoreCase))
                    {
                        r.Cell(2).Value = profile.TargetCalories;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    var last = ws.LastRowUsed();
                    var row = last == null ? 2 : last.RowNumber() + 1;
                    ws.Cell(row, 1).Value = profile.ProfileName;
                    ws.Cell(row, 2).Value = profile.TargetCalories;
                }

                wb.Save();
            }
        }
    }
}
