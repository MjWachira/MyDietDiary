using API.DTOs;
using API.Models;
using API.Services.DietService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IDietService _diet;

        public AdminController(IDietService diet)
        {
            _diet = diet;
        }

        [HttpGet("dashboard")]
        public ActionResult<AdminDashboardDto> GetDashboard()
        {
            var dashboard = new AdminDashboardDto
            {
                Profiles = _diet.GetProfiles(),
                Meals = _diet.GetAllMeals(),
                Weights = _diet.GetAllWeights()
            };

            return Ok(dashboard);
        }

        [HttpDelete("meals/{id:guid}")]
        public IActionResult DeleteMeal(Guid id)
        {
            _diet.DeleteMeal(id);
            return NoContent();
        }

        [HttpDelete("weights/{id:guid}")]
        public IActionResult DeleteWeight(Guid id)
        {
            _diet.DeleteWeight(id);
            return NoContent();
        }
    }
}
