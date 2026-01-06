using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuroraIgloosAPI.Models;
using AuroraIgloosAPI.Models.Contexts;
using AuroraIgloosAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AuroraIgloosAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CompanyContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public CustomersController(CompanyContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: api/Customers
        //[HttpGet]
        [Authorize(Roles = "Admin,Staff,ReadOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomer()
        {

            var customers =  await _context.Customer
                .Include(c => c.Person)
                    .ThenInclude(u => u.Address)
                .Include(c => c.User)
                .Select(c => new CustomerDTO
                {
                    Id = c.Id,
                    IdPerson = c.Person.Id,
                    Name = c.Person.Name ?? "",
                    Surname = c.Person.Surname ?? "",
                    Email = c.Person.Email ?? "",
                    Phone = c.Person.PhoneNumber ?? "",
                    Street = c.Person.Address.Street ?? "",
                    StreetNumber = c.Person.Address.StreetNumber ?? "",
                    HouseNumber = c.Person.Address.HouseNumber ?? "",
                    City = c.Person.Address.City ?? "",
                    Country = c.Person.Address.Country ?? "",
                    PostalCode = c.Person.Address.PostalCode ?? "",
                    IdUser = c.IdUser,
                    Login = c.User != null ? c.User.Login : null,
                    UserRoleId = c.User != null ? c.User.UserRoleId : null,
                    UserTypeId = c.User != null ? c.User.UserTypeId : null,
                })
                .ToListAsync();

            return Ok(customers);

        }

        // GET: api/Customers/5
        [Authorize(Roles = "Admin,Staff,ReadOnly")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerDTO customerDto)
        {
            if(id != customerDto.Id) return BadRequest("Id mismatch");
            
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var customer = await _context.Customer
                .Include(c => c.Person)
                    .ThenInclude(u => u.Address)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(customer == null) return NotFound($"Customer with id {id} not found");
            
            var isStaffOrAdmin = role == "Admin" ||  role == "Staff";
            var isOwner = customer.IdUser == userId;
            
            if(!isStaffOrAdmin && !isOwner) return Forbid();

            customer.Person.Name = customerDto.Name;
            customer.Person.Surname = customerDto.Surname;
            customer.Person.Email = customerDto.Email;
            customer.Person.PhoneNumber = customerDto.Phone;

            customer.Person.Address.Street = customerDto.Street;
            customer.Person.Address.StreetNumber = customerDto.StreetNumber;
            customer.Person.Address.HouseNumber = customerDto.HouseNumber;
            customer.Person.Address.City = customerDto.City;
            customer.Person.Address.Country = customerDto.Country;

            if (customer.User != null)
            {
                if (!string.IsNullOrWhiteSpace((customerDto.Login)))
                {
                    customer.User.Login = customerDto.Login;
                }

                if (!string.IsNullOrWhiteSpace((customerDto.Password)))
                {
                    customer.User.PasswordHash = _passwordHasher.HashPassword(customer.User,customerDto.Password);
                }
                
            }
            try
            {
                await _context.SaveChangesAsync();
            } 
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomerDTO customerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var address = new Address
            {
                Street = customerDto.Street,
                StreetNumber = customerDto.StreetNumber,
                HouseNumber = customerDto.HouseNumber,
                City = customerDto.City,
                PostalCode = customerDto.PostalCode,
                Country = customerDto.Country
            };

            _context.Address.Add(address);
            await _context.SaveChangesAsync();

            var person = new Person
            {
                Name = customerDto.Name,
                Surname = customerDto.Surname,
                Email = customerDto.Email,
                PhoneNumber = customerDto.Phone,
                IdAddress = address.Id,
                Address = address,
            };

            _context.Person.Add(person);
            await _context.SaveChangesAsync();

            var customer = new Customer
            {
                IdPerson = person.Id,
                Person = person,
            };

            if (customerDto.CreateUser)
            {
                if (string.IsNullOrWhiteSpace(customerDto.Login) || string.IsNullOrWhiteSpace(customerDto.Password))
                {
                    return BadRequest("Login and Password are required when CreateUser is true.");
                }

                var user = new User
                {
                    Login = customerDto.Login,
                    // UserTypeId = 2
                    UserRoleId = customerDto.UserRoleId ?? 3,
                    UserTypeId = customerDto.UserTypeId ?? 2,
                    
                };
                
                user.PasswordHash = _passwordHasher.HashPassword(user, customerDto.Password);
                
                _context.User.Add(user);
                await _context.SaveChangesAsync();
                
                customer.IdUser = user.Id;
                await _context.SaveChangesAsync();
            }

            try
            {
                _context.Customer.Add(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, new CustomerDTO
            {
                Id = customer.Id,
                IdPerson = person.Id,
                Name = person.Name ?? "",
                Surname = person.Surname ?? "",
                Email = person.Email ?? "",
                Phone = person.PhoneNumber ?? "",
                Street = address.Street ?? "",
                StreetNumber = address.StreetNumber ?? "",
                HouseNumber = address.HouseNumber ?? "",
                City = address.City ?? "",
                Country = address.Country ?? "",
                PostalCode = address.PostalCode ?? "",

                IdUser = customer.IdUser,
                Login = customer.IdUser != null ? customerDto.Login : null,
                UserRoleId = customerDto.CreateUser ? (customerDto.UserRoleId ?? 3) : null,
                UserTypeId = customerDto.CreateUser ? (customerDto.UserTypeId ?? 2) : null,
                CreateUser = customerDto.CreateUser
            });
        }

        // DELETE: api/Customers/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customer
                .Include(c => c.Person)
                    .ThenInclude(u => u.Address)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound($"Customer with id {id} not found");
            
            if(customer.User != null) _context.User.Remove(customer.User);

            _context.Customer.Remove(customer);
            _context.Person.Remove(customer.Person);
            _context.Address.Remove(customer.Person.Address);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        // GET for logged in customer
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<Customer>> GetMyCustomerProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var customer = await _context.Customer
                .Include(c => c.Person).ThenInclude(p => p.Address)
                .FirstOrDefaultAsync(c => c.IdUser == userId);
            
            if (customer == null) return NotFound($"Customer with id {userId} not found");
            
            return customer;
        }
        
        [Authorize(Roles="Admin")]
        [HttpPost("{id}/create-user")]
        public async Task<IActionResult> CreateUserForCustomer(int id, CreateCustomerUserDTO dto)
        {
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null) return NotFound("Customer not found");

            if (customer.IdUser != null) return Conflict("Customer already has an account.");

            var loginTaken = await _context.User.AnyAsync(u => u.Login == dto.Login);
            if (loginTaken) return Conflict("Login already exists.");

            var user = new User
            {
                Login = dto.Login,
                UserRoleId = dto.UserRoleId,
                UserTypeId = dto.UserTypeId
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            customer.IdUser = user.Id;
            await _context.SaveChangesAsync();

            return Ok(new { userId = user.Id });
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
        
    }
    
    
}
