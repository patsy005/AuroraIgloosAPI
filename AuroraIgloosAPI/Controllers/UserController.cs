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
using Microsoft.AspNetCore.Identity;

namespace AuroraIgloosAPI.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
    private readonly CompanyContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserController(CompanyContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }
    
    // GET: api/Users
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
        var users = await _context.User
            .Include(u => u.Role)
            .Include(u => u.UserType)
            .Include(u => u.Employee)
            .Include(u => u.Customer)
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Login = u.Login,
                PasswordHash = u.PasswordHash,
                
                UserRoleId = u.Role.Id,
                Role = u.Role,
                
                UserTypeId = u.UserTypeId,
                UserType = u.UserType,
                
                Employee = u.Employee,
                Customer = u.Customer,
            })
            .ToListAsync();
        
        return Ok(users);
    }
    
    // GET: api/Users/1
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.User
            .Include(u => u.Role)
            .Include(u => u.UserType)
            .Include(u => u.Employee)
            .Include(u => u.Customer)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null) return NotFound();
        
        return user;
    }
    
    // POST: api/Users
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<UserDTO>> PostUser(UserCreateDTO userDTO)
    {
        if (string.IsNullOrWhiteSpace(userDTO.Login) || string.IsNullOrWhiteSpace(userDTO.PasswordHash))
            return BadRequest("Login and Password are required.");

        var user = new User
        {
            Login = userDTO.Login,
            UserRoleId = userDTO.UserRoleId,
            UserTypeId = userDTO.UserTypeId,
        };
        
        user.PasswordHash = _passwordHasher.HashPassword(user, userDTO.PasswordHash);
        
        _context.User.Add(user);
        await _context.SaveChangesAsync();

        var result = new UserDTO
        {
            Id = user.Id,
            Login = user.Login,
            PasswordHash = user.PasswordHash,
            UserRoleId = user.Role.Id,
            Role = user.Role,
            UserTypeId = user.UserTypeId,
            UserType = user.UserType,
            Employee = user.Employee,
            Customer = user.Customer,
        };
        
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, result);
    }
    
    // PUT: api/Users/1
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDTO>> PutUser(int id, UserUpdateDTO userDTO)
    {
        if(id != userDTO.Id) return BadRequest();
        
        var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();
        
        user.Login = userDTO.Login;
        user.PasswordHash = userDTO.PasswordHash;
        user.UserRoleId = userDTO.UserRoleId;
        user.UserTypeId = userDTO.UserTypeId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
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
    
    // DELETE: api/Users/1
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<UserDTO>> DeleteUser(int id)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();
        
        _context.User.Remove(user);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _context.User.Any(e => e.Id == id);
    }
    
}