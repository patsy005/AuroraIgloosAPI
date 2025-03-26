using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuroraIgloosAPI.Models;
using AuroraIgloosAPI.Models.Contexts;
using AuroraIgloosAPI.DTOs;
using NuGet.Packaging;

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
        public async Task<ActionResult<IEnumerable<IglooDTO>>> GetIgloo()
        {
            //return await _context.Igloo.ToListAsync();
            var igloos = await _context.Igloo
                .Include(i => i.Discount)
                .Select(i => new IglooDTO
                {
                    Id = i.Id,
                    Name = i.Name ?? "",
                    Capacity = i.Capacity ?? 0,
                    PricePerNight = i.PricePerNight ?? 0,
                    Discount = i.Discount.Where(d => d.IdIgloo == i.Id)
                        .Select(d => d.Discount1)
                        .FirstOrDefault(),
                    DiscountName = i.Discount.Where(d => d.IdIgloo == i.Id)
                        .Select(d => d.Name)
                        .FirstOrDefault() ?? ""
                })

                .ToListAsync();

            return Ok(igloos);
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
        public async Task<IActionResult> PutIgloo(int id, IglooDTO iglooDto)
        {
            if (id != iglooDto.Id)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid) return BadRequest(ModelState);


            var igloo = await _context.Igloo
                .Include(i => i.Discount)
                .FirstOrDefaultAsync(i => i.Id == id);

            if(igloo == null) return NotFound($"Igloo with id {id} not found");

            igloo.Name = iglooDto.Name;
            igloo.Capacity = iglooDto.Capacity;
            igloo.PricePerNight = iglooDto.PricePerNight;

            igloo.Discount.Clear();

            var discounts = await _context.Discount
                .Where(d => iglooDto.IdDiscount == d.IdIgloo)
                .ToListAsync();

            igloo.Discount.AddRange(discounts);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Igloo.Any(i => i.Id == id))
                {
                    return NotFound($"Igloo with id {id} not found");
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
        public async Task<ActionResult<Igloo>> PostIgloo(IglooDTO iglooDto)
        {
            //_context.Igloo.Add(igloo);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetIgloo", new { id = igloo.Id }, igloo);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var discounts = await _context.Discount
                .Where(d => iglooDto.IdDiscount == d.IdIgloo)
                .ToListAsync();

            var igloo = new Igloo
            {
                Name = iglooDto.Name,
                Capacity = iglooDto.Capacity,
                PricePerNight = iglooDto.PricePerNight,
                Discount = discounts
            };

            try
            {
                _context.Igloo.Add(igloo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return CreatedAtAction("GetIgloo", new { id = igloo.Id }, igloo);
        }

        // DELETE: api/Igloos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIgloo(int id)
        {
            var igloo = await _context.Igloo 
                .Include(i => i.Discount)
                .FirstOrDefaultAsync(i => i.Id == id);

            if(igloo == null) return NotFound($"Igloo with id {id} not found");

            //_context.Discount.RemoveRange(igloo.Discount);

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
