using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.Models;
using AuroraIgloosAPI.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuroraIgloosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripSeasonController : ControllerBase
    {
        private readonly CompanyContext _context;

        public TripSeasonController(CompanyContext context)
        {
            _context = context;
        }
        
        // GET: api/TripSeason
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripSeasonDTO>>> GetTripSeason()
        {
            var tripSeasons = await _context.TripSeason.ToListAsync();
            
            return Ok(tripSeasons);
        }
        
        // GET: api/TripSeason/1
        [HttpGet("{id}")]
        public async Task<ActionResult<TripSeasonDTO>> GetTripSeason(int id)
        {
            var tripSeason = await _context.TripSeason.FindAsync(id);

            if (tripSeason == null) return NotFound();
            
            return Ok(tripSeason);
        }
        
        // POST: api/TripSeason
        [HttpPost]
        public async Task<ActionResult<TripSeasonDTO>> PostTripSeason(TripSeasonDTO tripSeasonDTO)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            
            var now = DateOnly.FromDateTime(DateTime.Now);

            var tripSeason = new TripSeason
            {
                Name = tripSeasonDTO.Name,
                Description = tripSeasonDTO.Description ?? "",
                CreatedAt = now,
                UpdatedAt = now,
            };

            try
            {
                _context.TripSeason.Add(tripSeason);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
            return CreatedAtAction(nameof(GetTripSeason), new { id = tripSeason.Id }, tripSeason);
        }
        
       // PUT: api/TripSeason/1
       [HttpPut("{id}")]
       public async Task<IActionResult> PutTripSeason(int id, TripSeasonDTO tripSeasonDTO)
       {
           if(!ModelState.IsValid) return BadRequest(ModelState);
           
           var tripSeason = await _context.TripSeason.FindAsync(id);
           
           if(tripSeason == null) return NotFound();
           
           tripSeason.Name = tripSeasonDTO.Name;
           tripSeason.Description = tripSeasonDTO.Description ?? "";
           tripSeason.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);

           try
           {
               _context.TripSeason.Update(tripSeason);
               await _context.SaveChangesAsync();
           }
           catch (Exception ex)
           {
               return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
           }
           
           return Ok(tripSeason);
       }
        
        // DELETE: api/TripSeason/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripSeason(int id)
        {
            var tripSeason = await _context.TripSeason.FindAsync(id);
            
            if (tripSeason == null) return NotFound();
            
            _context.TripSeason.Remove(tripSeason);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        private bool TripSeasonExists(int id)
        {
            return _context.TripSeason.Any(e => e.Id == id);
        }
    }
}