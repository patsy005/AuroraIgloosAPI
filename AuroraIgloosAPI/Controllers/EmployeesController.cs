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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace AuroraIgloosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly CompanyContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public EmployeesController(CompanyContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployee()
        {
            var employees = await _context.Employee
                .Include(e => e.Person)
                .Include(e => e.EmployeeRole)
                .Include(e => e.User)
                .Select(e => new EmployeeDTO
                {
                    Id = e.Id,
                    RoleId = e.RoleId,
                    Name = e.Person.Name ?? "",
                    Surname = e.Person.Surname ?? "",
                    Email = e.Person.Email ?? "",
                    PhoneNumber = e.Person.PhoneNumber ?? "",
                    Street = e.Person.Address.Street ?? "",
                    StreetNumber = e.Person.Address.StreetNumber ?? "",
                    HouseNumber = e.Person.Address.HouseNumber ?? "",
                    City = e.Person.Address.City ?? "",
                    Country = e.Person.Address.Country ?? "",
                    PostalCode = e.Person.Address.PostalCode ?? "",
                    Role = e.EmployeeRole.RoleName ?? "",
                    PhotoUrl = e.PhotoUrl ?? "",
                    IdPerson = e.IdPerson,
                    Login = e.User.Login ?? "",
                    UserTypeId = e.User.UserTypeId,
                    UserRoleId = e.User.UserRoleId,
                    Password = e.User.PasswordHash
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
        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromForm] EmployeeFormDTO employeeDto)
        {
            if (id != employeeDto.Id) return BadRequest($"Id mismatch");
            
            var employee = await _context.Employee
                .Include(e => e.Person)
                    .ThenInclude(u => u.Address)
                .Include(e => e.EmployeeRole)
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if(employee == null) return NotFound($"Employee with id {id} not found");

            employee.Person.Name = employeeDto.Name ?? employee.Person.Name;
            employee.Person.Surname = employeeDto.Surname ?? employee.Person.Surname;
            employee.Person.Email = employeeDto.Email ?? employee.Person.Email;
            employee.Person.PhoneNumber = employeeDto.PhoneNumber ?? employee.Person.PhoneNumber;

            employee.Person.Address.Street = employeeDto.Street ?? employee.Person.Address.Street;
            employee.Person.Address.StreetNumber = employeeDto.StreetNumber ?? employee.Person.Address.StreetNumber;
            employee.Person.Address.HouseNumber = employeeDto.HouseNumber ?? employee.Person.Address.HouseNumber;
            employee.Person.Address.City = employeeDto.City ?? employee.Person.Address.City;
            employee.Person.Address.PostalCode = employeeDto.PostalCode ?? employee.Person.Address.PostalCode;
            employee.Person.Address.Country = employeeDto.Country ?? employee.Person.Address.Country;

            employee.RoleId = employeeDto.RoleId ?? employee.RoleId;
            
            employee.User.Login = employeeDto.Login ?? employee.User.Login;
            employee.User.UserTypeId = employeeDto.UserTypeId ?? employee.User.UserTypeId;
            employee.User.UserRoleId = employeeDto.UserRoleId ?? employee.User.UserRoleId;
            
            if (!string.IsNullOrWhiteSpace(employeeDto.Password))
            {
                employee.User.PasswordHash =
                    _passwordHasher.HashPassword(employee.User, employeeDto.Password);
            }

            if (employeeDto.PhotoFile != null && employeeDto.PhotoFile.Length > 0)
            {
                var uploadsPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "contents",
                    "images",
                    "employees"
                );
                Directory.CreateDirectory(uploadsPath);
                
                var originalFileName = Path.GetFileName(employeeDto.PhotoFile.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}-{originalFileName}";
                
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await employeeDto.PhotoFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrWhiteSpace(employee.PhotoUrl))
                {
                    var oldFilePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        employee.PhotoUrl.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                
                var relativePath = Path.Combine("contents", "images", "employees", uniqueFileName).Replace('\\', '/');
                
                employee.PhotoUrl = relativePath;
            }

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
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee([FromForm] EmployeeFormDTO employeeDto)
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

            var person = new Person
            {
                Name = employeeDto.Name ?? "",
                Surname = employeeDto.Surname ?? "",
                Email = employeeDto.Email ?? "",
                PhoneNumber = employeeDto.PhoneNumber ?? "",
                Address = address
            };

            _context.Person.Add(person);
            await _context.SaveChangesAsync();

            var role = _context.EmployeeRole.FirstOrDefault(r => r.Id == employeeDto.RoleId);
            if (role == null) return BadRequest("Role not found");

            if (string.IsNullOrWhiteSpace((employeeDto.Login)) || (string.IsNullOrWhiteSpace(employeeDto.Password)))
            {
                return BadRequest("Login and Password are required");
            }
            
            var user = new User
            {
                Login = employeeDto.Login ?? "",
                // UserTypeId = 1 // Employee
                UserTypeId = employeeDto.UserTypeId ?? 1,
                UserRoleId = employeeDto.UserRoleId ?? 4,
            };
            
            user.PasswordHash = _passwordHasher.HashPassword(user, employeeDto.Password);
            
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            string? photoPath = null;

            if (employeeDto.PhotoFile != null && employeeDto.PhotoFile.Length > 0)
            {
                var uploadsPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "contents",
                    "images",
                    "employees"
                );
                Directory.CreateDirectory(uploadsPath);
                
                var originalFileName = Path.GetFileName(employeeDto.PhotoFile.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}-{originalFileName}";
                
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await employeeDto.PhotoFile.CopyToAsync(stream);
                }
                
                photoPath = Path.Combine("contents", "images", "employees", uniqueFileName).Replace('\\', '/');
            }

            var employee = new Employee
            {
                IdPerson = person.Id,
                RoleId = employeeDto.RoleId ?? 0,
                PhotoUrl = photoPath ?? "",
                Person = person,
                EmployeeRole = role,
                User = user,
                IdUser = user.Id,
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
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employee
                .Include(e => e.Person)
                    .ThenInclude(u => u.Address)
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound($"Employee with id {id} not found");
            }

            _context.Employee.Remove(employee);
            _context.User.Remove(employee.User);
            _context.Person.Remove(employee.Person);
            _context.Address.Remove(employee.Person.Address);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
