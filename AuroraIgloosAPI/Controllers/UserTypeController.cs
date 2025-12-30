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
using Microsoft.AspNetCore.Authorization;

namespace AuroraIgloosAPI.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]

public class UserTypeController : ControllerBase
{
    private readonly CompanyContext _context;

    public UserTypeController(CompanyContext context)
    {
        _context = context;
    }
    
    // GET: api/UserType
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserTypeDTO>>> GetUserTypes()
    {
        var userType = await _context.UserType
            .Select(type => new UserTypeDTO
            {
                Id = type.Id,
                Type = type.Type,
            })
            .ToListAsync();

        return Ok(userType);
    }
    
    // GET: api/UserType/1
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserTypeDTO>> GetUserType(int id)
    {
        var userType = await _context.UserType.FindAsync(id);

        if (userType == null)
        {
            return NotFound();
        }
        
        return Ok(userType);
    }
    
    // PUT: api/UserType/1
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserType(int id, UserTypeDTO userTypeDTO)
    {
        if (id != userTypeDTO.Id)
        {
            return BadRequest();
        }
        
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var userType = await _context.UserType.FirstOrDefaultAsync(u => u.Id == id);

        if (userType == null)
        {
            return NotFound();
        }
        userType.Type = userTypeDTO.Type;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserTypeExists(id))
            {
                return NotFound($"User type with id {id} not found");
            }
            else
            {
                throw;
            }
        }
        
        return NoContent();
    }
    
    //POST: api/UserType
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<UserTypeDTO>> PostUserType(UserTypeDTO userTypeDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userType = new UserType
        {
            Type = userTypeDTO.Type,
        };

        try
        {
            await _context.UserType.AddAsync(userType);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        
        return CreatedAtAction("GetUserType", new { id = userType.Id }, userType);
    }
    
    // DELETE: api/UserType
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<UserTypeDTO>> DeleteUserType(int id)
    {
        var userType   = await _context.UserType.FirstOrDefaultAsync(t => t.Id == id);
        
        if (userType == null) return NotFound($"Customer type with id {id} not found");
        
        _context.UserType.Remove(userType);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    
    private bool UserTypeExists(int id)
    {
        return _context.UserType.Any(e => e.Id == id);
    }

}