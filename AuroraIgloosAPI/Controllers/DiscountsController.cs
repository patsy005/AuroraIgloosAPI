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

namespace AuroraIgloosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public DiscountsController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/Discounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountDTO>>> GetDiscount()
        {
            var discounts = await _context.Discount
                .Include(d => d.Igloo)
                .Select( d => new DiscountDTO
                {
                    Id = d.Id,
                    IdIgloo = d.IdIgloo,
                    Name = d.Name ?? "",
                    Discount = d.Discount1,
                    Description = d.Description ?? "",
                    IglooName = d.Igloo.Name ?? ""
                })
                .ToListAsync();

            return Ok(discounts);
        }

        // GET: api/Discounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Discount>> GetDiscount(int id)
        {
            var discount = await _context.Discount.FindAsync(id);

            if (discount == null)
            {
                return NotFound();
            }

            return discount;
        }

        // PUT: api/Discounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscount(int id, DiscountDTO discountDto)
        {
           
            if(id != discountDto.Id) return BadRequest("Id mismatch");

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var igloo = await _context.Igloo.FindAsync(discountDto.IdIgloo);

            if(igloo == null) return NotFound($"Igloo with id {discountDto.IdIgloo} not found");

            var discount = await _context.Discount
                .Include(d => d.Igloo)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (discount == null) return NotFound($"Discount with id {id} not found");

            discount.IdIgloo = discountDto.IdIgloo;
            discount.Name = discountDto.Name;
            discount.Discount1 = discountDto.Discount;
            discount.Description = discountDto.Description;
            discount.Igloo = igloo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscountExists(id))
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

        // POST: api/Discounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Discount>> PostDiscount(DiscountDTO discountDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var igloo = await _context.Igloo.FindAsync(discountDto.IdIgloo);

            if (igloo == null) return NotFound($"Igloo with id {discountDto.IdIgloo} not found");

            var discount = new Discount
            {
                IdIgloo = discountDto.IdIgloo,
                Name = discountDto.Name,
                Discount1 = discountDto.Discount,
                Description = discountDto.Description,
                Igloo = igloo
            };

            try
            {
                _context.Discount.Add(discount);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return CreatedAtAction("GetDiscount", new { id = discount.Id }, discount);
        }

        // DELETE: api/Discounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var discount = await _context.Discount
                .Include(d => d.Igloo)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (discount == null) return NotFound($"Discount with id {id} not found");

            _context.Discount.Remove(discount);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DiscountExists(int id)
        {
            return _context.Discount.Any(e => e.Id == id);
        }
    }
}
