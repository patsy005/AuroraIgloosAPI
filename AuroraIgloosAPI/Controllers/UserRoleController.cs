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

public class UserRoleController : ControllerBase
{
    private readonly CompanyContext _context;

    public UserRoleController(CompanyContext context)
    {
        _context = context;
    }
    
    // GET: api/UserRole
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRoleDTO>>> GetUserRoles()
    {
        var userRole = await _context.UserRole.ToListAsync();
        
        return Ok(userRole);
    }
    
    // GET: api/UserRole/1
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserRoleDTO>> GetUserRole(int id)
    {
        var userRole = await _context.UserRole.FindAsync(id);
        
        if (userRole == null) return NotFound();
        
        return Ok(userRole);
    }
    
    // PUT: api/UserRole/1
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserRole(int id, UserRoleDTO userRoleDTO)
    {
        if(id != userRoleDTO.Id) return BadRequest();
        
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var userRole = await _context.UserRole.FirstOrDefaultAsync(r => r.Id == id);
        
        if(userRole == null) return NotFound();
        
        userRole.Name = userRoleDTO.Name;
        userRole.Description = userRoleDTO.Description;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if(!UserRoleExists(id))
            {
                return NotFound($"User role with id {id} not found");
            }
            else
            {
                throw;
            }
        } 
        
        return NoContent();
    }
    
    // POST: api/UserType
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<UserRoleDTO>> PostUserRole(UserRoleDTO userRoleDTO)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var userRole = new UserRole
        {
            Name = userRoleDTO.Name,
            Description = userRoleDTO.Description,
        };

        try
        {
            await _context.UserRole.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        
        return CreatedAtAction("GetUserRole", new { id = userRole.Id }, userRole);
    }
    
    // DELETE: api/UserRole/1
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<UserRoleDTO>> DeleteUserRole(int id)
    {
        var userRole = await _context.UserRole.FindAsync(id);
        
        if (userRole == null) return NotFound($"User role with id: {id } not found");
        
        _context.UserRole.Remove(userRole);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    private bool UserRoleExists(int id)
    {
        return _context.UserRole.Any(e => e.Id == id);
    }
}