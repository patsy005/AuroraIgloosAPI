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
    public class TripLevelController : ControllerBase
    {
        private readonly CompanyContext _context;

        public TripLevelController(CompanyContext context)
        {
            _context = context;
        }
        
        // GET: api/TripLevel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripLevelOfDifficulty>>> GetTripLevel()
        {
            var tripSeasons = await _context.TripLevelOfDifficulty.ToListAsync();
            
            return Ok(tripSeasons);
        }
        
        // GET: api/TripLevel/1
        [HttpGet("{id}")]
        public async Task<ActionResult<TripLevelOfDifficultyDTO>> GetTripLevel(int id)
        {
            var tripSeason = await _context.TripLevelOfDifficulty.FindAsync(id);

            if (tripSeason == null) return NotFound();
            
            return Ok(tripSeason);
        }
        
        // POST: api/TripLevel
        [HttpPost]
        public async Task<ActionResult<TripLevelOfDifficultyDTO>> PostTripLevel(TripLevelOfDifficultyDTO tripLevelDTO)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            
            var now = DateOnly.FromDateTime(DateTime.Now);

            var tripLevel = new TripLevelOfDifficulty
            {
                Name = tripLevelDTO.Name,
                Description = tripLevelDTO.Description ?? "",
                CreatedAt = now,
                UpdatedAt = now,
            };

            try
            {
                _context.TripLevelOfDifficulty.Add(tripLevel);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
            return CreatedAtAction(nameof(GetTripLevel), new { id = tripLevel.Id }, tripLevel);
        }
        
        // PUT: api/TripLevel/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTripLevel(int id, TripLevelOfDifficultyDTO tripLevelDTO)
        {
            if (id != tripLevelDTO.Id)
                return BadRequest("Invalid Id");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tripLevel = await _context.TripLevelOfDifficulty.FindAsync(id);
            if (tripLevel == null)
                return NotFound($"TripLevel with id {id} not found");

            tripLevel.Name = tripLevelDTO.Name;
            tripLevel.Description = tripLevelDTO.Description ?? "";
            tripLevel.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripLevelExists(id))
                    return NotFound($"TripLevel with id {id} not found");
                throw;
            }

            return NoContent();
        }

        
        // DELETE: api/TripLevel/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripLevel(int id)
        {
            var tripLevel = await _context.TripLevelOfDifficulty.FindAsync(id);
            
            if (tripLevel == null) return NotFound();
            
            _context.TripLevelOfDifficulty.Remove(tripLevel);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        private bool TripLevelExists(int id)
        {
            return _context.TripLevelOfDifficulty.Any(e => e.Id == id);
        }
    }
}