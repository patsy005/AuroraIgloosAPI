using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.Models;
using AuroraIgloosAPI.Models.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentBlocksController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ContentBlocksController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/ContentBlocks
        [Authorize(Roles = "Admin,Staff,ReadOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContentBlockDTO>>> GetAll()
        {
            var items = await _context.ContentBlocks
                .OrderByDescending(c => c.LastModifiedAt)
                .Select(cb => new ContentBlockDTO
                {
                    Id = cb.Id,
                    Key = cb.Key,
                    Value = cb.Value,
                })
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/ContentBlocks/5
        [Authorize(Roles = "Admin,Staff,ReadOnly")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ContentBlockDTO>> GetById(int id)
        {
            var item = await _context.ContentBlocks.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new ContentBlockDTO
                {
                    Id = x.Id,
                    Key = x.Key,
                    Value = x.Value
                })
                .FirstOrDefaultAsync();

            if (item == null) return NotFound($"ContentBlock with id {id} not found.");
            return Ok(item);
        }
        

        // POST: api/ContentBlocks
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<ActionResult<ContentBlockDTO>> Create([FromBody] ContentBlockCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            dto.Key = dto.Key.Trim();

            var keyExists = await _context.ContentBlocks.AnyAsync(x => x.Key == dto.Key);
            if (keyExists) return Conflict("Key already exists.");

            var entity = new ContentBlock
            {
                Key = dto.Key,
                Value = dto.Value,
                LastModifiedAt = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.ContentBlocks.Add(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Key already exists.");
            }

            var result = new ContentBlockDTO
            {
                Id = entity.Id,
                Key = entity.Key,
                Value = entity.Value,
                LastModifiedAt = entity.LastModifiedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }

        // PUT: api/ContentBlocks/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ContentBlockUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (dto.Id != id) return BadRequest("Route id does not match body Id.");

            dto.Key = dto.Key.Trim();

            var entity = await _context.ContentBlocks.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound($"ContentBlock with id {id} not found.");

            var keyTaken = await _context.ContentBlocks.AnyAsync(x => x.Key == dto.Key && x.Id != id);
            if (keyTaken) return Conflict("Key already exists.");

            entity.Key = dto.Key;
            entity.Value = dto.Value;
            entity.LastModifiedAt = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Key already exists.");
            }

            return NoContent();
        }

        // DELETE: api/ContentBlocks/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.ContentBlocks.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound($"ContentBlock with id {id} not found.");

            _context.ContentBlocks.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
