using API.DTOs;
using API.Models;
using API.Services.DietService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly IDietService _diet;

        public MealsController(IDietService diet) => _diet = diet;

        [HttpGet("{profile}")]
        public ActionResult<IEnumerable<Meal>> GetMeals(string profile)
        {
            var list = _diet.GetMeals(profile);
            return Ok(list);
        }

        [HttpPost("{profile}")]
        public ActionResult<Meal> AddMeal(string profile, [FromBody] MealDto dto)
        {
            dto.Profile = profile;
            var meal = _diet.AddMeal(dto);
            return CreatedAtAction(nameof(GetMeals), new { profile = profile }, meal);
        }

        [HttpDelete("{profile}/{id:guid}")]
        public IActionResult DeleteMeal(string profile, Guid id)
        {
            _diet.DeleteMeal(id);
            return NoContent();
        }
    }
}
