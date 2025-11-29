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

    public class TripController : ControllerBase
    {
        private readonly CompanyContext _context;

        public TripController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/Trips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripDTO>>> GetTrips()
        {
            var trips = await _context.Trip
                .Include(t => t.Season)
                .Include(t => t.LevelOfDifficulty)
                .Include(t => t.Guide)
                .Select(t => new TripDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    Duration = t.Duration,
                    PricePerPerson = t.PricePerPerson,
                    ShortDescription = t.ShortDescription,
                    LongDescription = t.LongDescription,
                    GuideId = t.GuideId,
                    Guide = t.Guide,
                    LevelOfDifficultyId = t.LevelOfDifficultyId,
                    LevelOfDifficultyName = t.LevelOfDifficulty.Name ?? "",
                    SeasonId = t.SeasonId,
                    SeasonName = t.Season.Name ?? "",
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                })
                .ToListAsync();

            return Ok(trips);
        }

        // GET: api/Trips/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTrip(int id)
        {
            var trip = await _context.Trip.FindAsync(id);

            if (trip == null) return NotFound();

            return trip;
        }

        // POST: api/Trips
        [HttpPost]
        public async Task<ActionResult<Trip>> PostTrip(TripFormDTO tripDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var now = DateOnly.FromDateTime(DateTime.UtcNow);

            var trip = new Trip
            {
                Name = tripDTO.Name,
                Duration = tripDTO.Duration,
                PricePerPerson = tripDTO.PricePerPerson,
                ShortDescription = tripDTO.ShortDescription,
                LongDescription = tripDTO.LongDescription,
                GuideId = tripDTO.GuideId,
                LevelOfDifficultyId = tripDTO.LevelOfDifficultyId,
                SeasonId = tripDTO.SeasonId,
                CreatedAt = now,
                UpdatedAt = now,
            };

            try
            {
                await _context.Trip.AddAsync(trip);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return CreatedAtAction(nameof(GetTrip), new { id = trip.Id }, trip);
        }

        // PUT: api/Trips/1
        [HttpPut("{id}")]
        public async Task<ActionResult<Trip>> PutTrip(int id, TripDTO tripDTO)
        {
            if (id != tripDTO.Id) return BadRequest("Invalid Id");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var trip = await _context.Trip.FindAsync(id);

            if (trip == null) return NotFound();

            trip.Name = tripDTO.Name;
            trip.Duration = tripDTO.Duration;
            trip.PricePerPerson = tripDTO.PricePerPerson;
            trip.ShortDescription = tripDTO.ShortDescription;
            trip.LongDescription = tripDTO.LongDescription;
            trip.GuideId = tripDTO.Guide.Id;
            trip.LevelOfDifficultyId = tripDTO.LevelOfDifficultyId;
            trip.SeasonId = tripDTO.SeasonId;
            trip.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
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
        
        // DELETE: api/Trips/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<Trip>> DeleteTrip(int id)
        {
            var trip = await _context.Trip.FindAsync(id);
            
            if (trip == null) return NotFound();
            
            _context.Trip.Remove(trip);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        private bool TripExists(int id)
        {
            return _context.Trip.Any(e => e.Id == id);
        }
    }
}