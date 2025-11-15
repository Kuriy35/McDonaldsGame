using AutoMapper;
using McDonalds.Data;
using McDonalds.Models;
using McDonalds.Models.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using McDonalds.ViewModels.Api;

namespace McDonalds.Controllers6
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Ресурси за складністю")]
    public class DifficultyResourcesController : ControllerBase
    {
        private readonly McDonaldsContext _context;
        private readonly IMapper _mapper;

        public DifficultyResourcesController(McDonaldsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Отримати всі ресурси за складністю")]
        public async Task<ActionResult<IEnumerable<DifficultyResourceApiViewModel>>> Get()
        {
            var data = await _context.DifficultyResources.ToListAsync();
            return Ok(_mapper.Map<List<DifficultyResourceApiViewModel>>(data));
        }

        [HttpGet("{difficulty}")]
        [SwaggerOperation(Summary = "Отримати всі ресурси для певної складності")]
        public async Task<ActionResult<List<ResourceApiViewModel>>> GetByDifficulty(GameDifficulty difficulty)
        {
            var data = await _context.DifficultyResources
                .Where(dr => dr.Difficulty == difficulty)
                .ToListAsync();

            var resources = data.Select(dr => new ResourceApiViewModel
            {
                Name = dr.ResourceName,
                Quantity = dr.BaseQuantity,
                BuyPrice = dr.BuyPrice,
                SellPrice = dr.SellPrice
            }).ToList();

            return Ok(resources);
        }

        [HttpGet("{difficulty}/{resourceName}")]
        [SwaggerOperation(Summary = "Отримати ресурс за складністю та назвою")]
        public async Task<ActionResult<DifficultyResourceApiViewModel>> Get(GameDifficulty difficulty, string resourceName)
        {
            var res = await _context.DifficultyResources
                .FindAsync(difficulty, resourceName);
            if (res == null) return NotFound();
            return Ok(_mapper.Map<DifficultyResourceApiViewModel>(res));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Створити ресурс за складністю")]
        public async Task<ActionResult> Post([FromBody] DifficultyResourceApiViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var res = _mapper.Map<DifficultyResource>(vm);
            _context.DifficultyResources.Add(res);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { difficulty = res.Difficulty, resourceName = res.ResourceName }, vm);
        }

        [HttpPut("{difficulty}/{resourceName}")]
        [SwaggerOperation(Summary = "Оновити (або перемістити) ресурс за складністю")]
        public async Task<ActionResult> Put(
    GameDifficulty difficulty,
    string resourceName,
    [FromBody] DifficultyResourceApiViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _context.DifficultyResources
                .FindAsync(difficulty, resourceName);

            if (existing == null) return NotFound();

            if (vm.Difficulty != difficulty || vm.ResourceName != resourceName)
            {
                _context.DifficultyResources.Remove(existing);

                var newRes = _mapper.Map<DifficultyResource>(vm);
                _context.DifficultyResources.Add(newRes);
            }
            else
            {
                _mapper.Map(vm, existing);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{difficulty}/{resourceName}")]
        [SwaggerOperation(Summary = "Видалити ресурс за складністю")]
        public async Task<ActionResult> Delete(GameDifficulty difficulty, string resourceName)
        {
            var res = await _context.DifficultyResources
                .FindAsync(difficulty, resourceName);
            if (res == null) return NotFound();

            _context.DifficultyResources.Remove(res);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}