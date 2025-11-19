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
    public class EmployeeRolesController : ControllerBase
    {
        private readonly CompanyContext _context;

        public EmployeeRolesController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeRoleDTO>>> GetEmployeeRole()
        {
            var employeeRole = await _context.EmployeeRole
                .Select(e => new EmployeeRoleDTO
                {
                    Id = e.Id,
                    RoleName = e.RoleName,
                    RoleDescription = e.RoleDescription,
                })
                .ToListAsync();

            return Ok(employeeRole);
        }

        // GET: api/EmployeeRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeRole>> GetEmployeeRole(int id)
        {
            var employeeRole = await _context.EmployeeRole.FindAsync(id);

            if (employeeRole == null)
            {
                return NotFound();
            }

            return employeeRole;
        }

        // PUT: api/EmployeeRoles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeRole(int id, EmployeeRoleDTO employeeRoleDTO)
        {
            // UPDATE EmployeeRole 
            // SET Name = @p0, Description = @p1, ...
            // WHERE Id = @id

            // if (id != employeeRole.Id)
            // {
            //     return BadRequest();
            // }
            //
            // _context.Entry(employeeRole).State = EntityState.Modified;
            //
            // try
            // {
            //     await _context.SaveChangesAsync();
            // }
            // catch (DbUpdateConcurrencyException)
            // {
            //     if (!EmployeeRoleExists(id))
            //     {
            //         return NotFound();
            //     }
            //     else
            //     {
            //         throw;
            //     }
            // }
            //
            // return NoContent();

            if (id != employeeRoleDTO.Id)
            {
                return BadRequest();
            }
            
            if(!ModelState.IsValid) return BadRequest(ModelState);
            
            var employeeRole = await _context.EmployeeRole.FirstOrDefaultAsync(e => e.Id == id);
            
            employeeRole.RoleName = employeeRoleDTO.RoleName;
            employeeRole.RoleDescription = employeeRoleDTO.RoleDescription;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeRoleExists(id))
                {
                    return NotFound($"Employee Role with id {id} not found");
                }
                else
                {
                    throw;
                }
            }
            
            return NoContent();
        }

        // POST: api/EmployeeRoles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeRole>> PostEmployeeRole(EmployeeRole employeeRole)
        {
            _context.EmployeeRole.Add(employeeRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeRole", new { id = employeeRole.Id }, employeeRole);
        }

        // DELETE: api/EmployeeRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeRole(int id)
        {
            var employeeRole = await _context.EmployeeRole.FindAsync(id);
            if (employeeRole == null)
            {
                return NotFound();
            }

            _context.EmployeeRole.Remove(employeeRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeRoleExists(int id)
        {
            return _context.EmployeeRole.Any(e => e.Id == id);
        }
    }
}
