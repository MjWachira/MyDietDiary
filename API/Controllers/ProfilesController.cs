using API.DTOs;
using API.Models;
using API.Services.DietService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IDietService _diet;
        public ProfilesController(IDietService diet) => _diet = diet;

        [HttpGet("{profile}")]
        public ActionResult<Profile> GetProfile(string profile)
        {
            var p = _diet.GetProfile(profile);
            return Ok(p);
        }

        [HttpPost("{profile}/target")]
        public IActionResult UpdateTarget(string profile, [FromBody] ProfileDto dto)
        {
            _diet.UpdateTarget(profile, dto.TargetCalories);
            return NoContent();
        }
    }
}
