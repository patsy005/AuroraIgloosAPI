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
            var igloos = await _context.Igloo
                .Include(i => i.Discount)
                .Select(i => new IglooDTO
                {
                    Id = i.Id,
                    Name = i.Name ?? "",
                    Capacity = i.Capacity ?? 0,
                    PricePerNight = i.PricePerNight ?? 0,
                    Discount = i.Discount ?? null,
                    IdDiscount = i.IdDiscount,
                    PhotoUrl = i.PhotoUrl ?? "",
                    Description = i.Description ?? "",
                    
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
        public async Task<IActionResult> PutIgloo(int id, [FromForm] IglooFormDTO iglooDto)
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
            igloo.Discount = iglooDto.Discount;
            igloo.IdDiscount = iglooDto.IdDiscount;
            igloo.Description = iglooDto.Description;

            if (iglooDto.PhotoFile != null && iglooDto.PhotoFile.Length > 0)
            {
                var uploadsPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "contents",
                    "images",
                    "igloos"
                );
                Directory.CreateDirectory(uploadsPath);
                
                var originalFileName = Path.GetFileName(iglooDto.PhotoFile.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}-{originalFileName}";
                
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await iglooDto.PhotoFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrWhiteSpace(igloo.PhotoUrl))
                {
                    var oldFilePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        igloo.PhotoUrl.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                
                var relativePath = Path.Combine("contents", "images", "igloos", uniqueFileName).Replace('\\', '/');
                
                igloo.PhotoUrl = relativePath;
            }

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
        public async Task<ActionResult<Igloo>> PostIgloo([FromForm] IglooFormDTO iglooDto)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? photoPath = null;

            if (iglooDto.PhotoFile != null && iglooDto.PhotoFile.Length > 0)
            {
                var uploadsPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "contents",
                    "images",
                    "igloos"
                );
                Directory.CreateDirectory(uploadsPath);
                
                var originalFileName = Path.GetFileName(iglooDto.PhotoFile.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}-{originalFileName}";
                
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await iglooDto.PhotoFile.CopyToAsync(stream);
                }
                
                photoPath = Path.Combine("contents", "images", "igloos", uniqueFileName).Replace('\\', '/');
            }
            var igloo = new Igloo
            {
                Name = iglooDto.Name,
                Capacity = iglooDto.Capacity,
                PricePerNight = iglooDto.PricePerNight,
                Discount = iglooDto.Discount,
                IdDiscount = iglooDto.IdDiscount,
                PhotoUrl = photoPath,
                Description = iglooDto.Description,
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
            try
            {
                var igloo = await _context.Igloo
                    .Include(i => i.Discount)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (igloo == null) return NotFound($"Igloo with id {id} not found");


                _context.Igloo.Remove(igloo);

                await _context.SaveChangesAsync();


                return NoContent();
            }
            catch (DbUpdateException ex) 
                when (ex.InnerException?.Message.Contains("FK_Booking_Igloo_IdIgloo") == true)
            {
                return Conflict(new
                {
                    message = "Igloo cannot be deleted - it has existing bookings."
                });
            }
        }

        private bool IglooExists(int id)
        {
            return _context.Igloo.Any(e => e.Id == id);
        }
    }
}
