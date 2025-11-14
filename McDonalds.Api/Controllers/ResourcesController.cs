using AutoMapper;
using McDonalds.Data;
using McDonalds.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using McDonalds.ViewModels.Api;

namespace McDonalds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Ресурси гри")]
    public class ResourcesController : ControllerBase
    {
        private readonly McDonaldsContext _context;
        private readonly IMapper _mapper;

        public ResourcesController(McDonaldsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Отримати всі ресурси")]
        public async Task<ActionResult<IEnumerable<ResourceApiViewModel>>> Get()
        {
            var data = await _context.Resources.ToListAsync();
            return Ok(_mapper.Map<List<ResourceApiViewModel>>(data));
        }

        [HttpGet("{name}")]
        [SwaggerOperation(Summary = "Отримати ресурс за назвою")]
        public async Task<ActionResult<ResourceApiViewModel>> Get(string name)
        {
            var res = await _context.Resources.FindAsync(name);
            if (res == null) return NotFound();
            return Ok(_mapper.Map<ResourceApiViewModel>(res));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Створити ресурс")]
        public async Task<ActionResult> Post([FromBody] ResourceApiViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var res = _mapper.Map<Resource>(vm);
            _context.Resources.Add(res);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { name = res.Name }, vm);
        }

        [HttpPut("{name}")]
        [SwaggerOperation(Summary = "Оновити (або перейменувати) ресурс")]
        public async Task<ActionResult> Put(string name, [FromBody] ResourceApiViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _context.Resources.FindAsync(name);
            if (existing == null) return NotFound();

            if (vm.Name != name)
            {
                var nameExists = await _context.Resources.AnyAsync(r => r.Name == vm.Name);
                if (nameExists) return Conflict($"Ресурс з назвою '{vm.Name}' вже існує");

                _context.Resources.Remove(existing);

                var newResource = _mapper.Map<Resource>(vm);
                _context.Resources.Add(newResource);
            }
            else
            {
                _mapper.Map(vm, existing);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{name}")]
        [SwaggerOperation(Summary = "Видалити ресурс")]
        public async Task<ActionResult> Delete(string name)
        {
            var res = await _context.Resources.FindAsync(name);
            if (res == null) return NotFound();
            _context.Resources.Remove(res);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}