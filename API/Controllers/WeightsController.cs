using API.DTOs;
using API.Models;
using API.Services.DietService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightsController : ControllerBase
    {
        private readonly IDietService _diet;
        public WeightsController(IDietService diet) => _diet = diet;

        [HttpGet("{profile}")]
        public ActionResult<IEnumerable<WeightEntry>> GetWeights(string profile)
        {
            return Ok(_diet.GetWeights(profile));
        }

        [HttpPost("{profile}")]
        public ActionResult<WeightEntry> AddWeight(string profile, [FromBody] WeightEntryDto dto)
        {
            dto.Profile = profile;
            var w = _diet.AddWeight(dto);
            return CreatedAtAction(nameof(GetWeights), new { profile = profile }, w);
        }

        [HttpDelete("{profile}/{id:guid}")]
        public IActionResult DeleteWeight(string profile, Guid id)
        {
            _diet.DeleteWeight(id);
            return NoContent();
        }
    }
}
