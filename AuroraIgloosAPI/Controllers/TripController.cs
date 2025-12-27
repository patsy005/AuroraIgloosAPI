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
                    GuideName = t.Guide.Person.Name ?? "",
                    PhotoUrl = t.PhotoUrl ?? "",
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
        public async Task<ActionResult<Trip>> PostTrip([FromForm] TripFormDTO tripDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string? photoPath = null;

            if (tripDTO.PhotoFile != null && tripDTO.PhotoFile.Length > 0)
            {
                var uploadsPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "contents",
                    "images",
                    "trips"
                );
                Directory.CreateDirectory(uploadsPath);
                
                var originalFilename = Path.GetFileName(tripDTO.PhotoFile.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}-{originalFilename}";
                
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await tripDTO.PhotoFile.CopyToAsync(stream);
                }
                
                photoPath = Path.Combine("contents", "images", "trips", uniqueFileName).Replace('\\', '/');
            }

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
                PhotoUrl = photoPath
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
        public async Task<ActionResult<Trip>> PutTrip(int id, [FromForm] TripFormDTO tripDTO)
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
            trip.GuideId = tripDTO.GuideId;
            trip.LevelOfDifficultyId = tripDTO.LevelOfDifficultyId;
            trip.SeasonId = tripDTO.SeasonId;
            trip.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            
            if (tripDTO.PhotoFile != null && tripDTO.PhotoFile.Length > 0)
            {
                var uploadsPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "contents",
                    "images",
                    "trips"
                );
                Directory.CreateDirectory(uploadsPath);
                
                var originalFileName = Path.GetFileName(tripDTO.PhotoFile.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}-{originalFileName}";
                
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await tripDTO.PhotoFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrWhiteSpace(trip.PhotoUrl))
                {
                    var oldFilePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        trip.PhotoUrl.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                
                var relativePath = Path.Combine("contents", "images", "trips", uniqueFileName).Replace('\\', '/');
                
                trip.PhotoUrl = relativePath;
            }

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