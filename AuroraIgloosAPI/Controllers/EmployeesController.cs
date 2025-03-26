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
    public class EmployeesController : ControllerBase
    {
        private readonly CompanyContext _context;

        public EmployeesController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployee()
        {
            var employees = await _context.Employee
                .Include(e => e.User)
                .Include(e => e.EmployeeRole)
                .Select(e => new EmployeeDTO
                {
                    Id = e.Id,
                    RoleId = e.RoleId,
                    Name = e.User.Name ?? "",
                    Surname = e.User.Surname ?? "",
                    Email = e.User.Email ?? "",
                    PhoneNumber = e.User.PhoneNumber ?? "",
                    Street = e.User.Address.Street ?? "",
                    StreetNumber = e.User.Address.StreetNumber ?? "",
                    HouseNumber = e.User.Address.HouseNumber ?? "",
                    City = e.User.Address.City ?? "",
                    Country = e.User.Address.Country ?? "",
                    PostalCode = e.User.Address.PostalCode ?? "",
                    Role = e.EmployeeRole.RoleName ?? "",
                    PhotoUrl = e.PhotoUrl ?? "",
                    IdUser = e.IdUser
                })
                .ToListAsync();

            return Ok(employees);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeDTO employeeDto)
        {
            if(id != employeeDto.Id) return BadRequest("Id mismatch");

            var employee = await _context.Employee
                .Include(e => e.User)
                    .ThenInclude(u => u.Address)
                .Include(e => e.EmployeeRole)
                .FirstOrDefaultAsync(e => e.Id == id);

            if(employee == null) return NotFound($"Employee with id {id} not found");

            employee.User.Name = employeeDto.Name ?? employee.User.Name;
            employee.User.Surname = employeeDto.Surname ?? employee.User.Surname;
            employee.User.Email = employeeDto.Email ?? employee.User.Email;
            employee.User.PhoneNumber = employeeDto.PhoneNumber ?? employee.User.PhoneNumber;

            employee.User.Address.Street = employeeDto.Street ?? employee.User.Address.Street;
            employee.User.Address.StreetNumber = employeeDto.StreetNumber ?? employee.User.Address.StreetNumber;
            employee.User.Address.HouseNumber = employeeDto.HouseNumber ?? employee.User.Address.HouseNumber;
            employee.User.Address.City = employeeDto.City ?? employee.User.Address.City;
            employee.User.Address.PostalCode = employeeDto.PostalCode ?? employee.User.Address.PostalCode;
            employee.User.Address.Country = employeeDto.Country ?? employee.User.Address.Country;

            employee.RoleId = employeeDto.RoleId ?? employee.RoleId;

            employee.PhotoUrl = employeeDto.PhotoUrl ?? employee.PhotoUrl;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound($"Employee with id {id} not found");
                }
                else
                {
                    throw;
                }
            }

                return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeDTO employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var address = new Address
            {
                Street = employeeDto.Street ?? "",
                StreetNumber = employeeDto.StreetNumber ?? "",
                HouseNumber = employeeDto.HouseNumber ?? "",
                City = employeeDto.City ?? "",
                PostalCode = employeeDto.PostalCode ?? "",
                Country = employeeDto.Country ?? "",
            };

            _context.Address.Add(address);
            await _context.SaveChangesAsync();

            var user = new User
            {
                Name = employeeDto.Name ?? "",
                Surname = employeeDto.Surname ?? "",
                Email = employeeDto.Email ?? "",
                PhoneNumber = employeeDto.PhoneNumber ?? "",
                Address = address
                           };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            var role = _context.EmployeeRole.FirstOrDefault(r => r.Id == employeeDto.RoleId);
            if (role == null) return BadRequest("Role not found");

            var employee = new Employee
            {
                IdUser = user.Id,
                RoleId = employeeDto.RoleId ?? 0,
                PhotoUrl = employeeDto.PhotoUrl ?? "",
                User = user,
                EmployeeRole = role
            };



            try
            {
                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employee
                .Include(e => e.User)
                    .ThenInclude(u => u.Address)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound($"Employee with id {id} not found");
            }

            _context.Employee.Remove(employee);
            _context.User.Remove(employee.User);
            _context.Address.Remove(employee.User.Address);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
