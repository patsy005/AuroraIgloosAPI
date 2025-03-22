using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuroraIgloosAPI.Models;
using AuroraIgloosAPI.Models.Contexts;

namespace AuroraIgloosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IgloosController : ControllerBase
    {
        private readonly CompanyContext _context;

        public IgloosController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/Igloos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Igloo>>> GetIgloo()
        {
            return await _context.Igloo.ToListAsync();
        }

        // GET: api/Igloos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Igloo>> GetIgloo(int id)
        {
            var igloo = await _context.Igloo.FindAsync(id);

            if (igloo == null)
            {
                return NotFound();
            }

            return igloo;
        }

        // PUT: api/Igloos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIgloo(int id, Igloo igloo)
        {
            if (id != igloo.Id)
            {
                return BadRequest();
            }

            _context.Entry(igloo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IglooExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Igloos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Igloo>> PostIgloo(Igloo igloo)
        {
            _context.Igloo.Add(igloo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIgloo", new { id = igloo.Id }, igloo);
        }

        // DELETE: api/Igloos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIgloo(int id)
        {
            var igloo = await _context.Igloo.FindAsync(id);
            if (igloo == null)
            {
                return NotFound();
            }

            _context.Igloo.Remove(igloo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IglooExists(int id)
        {
            return _context.Igloo.Any(e => e.Id == id);
        }
    }
}
