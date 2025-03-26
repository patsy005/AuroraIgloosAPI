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
    public class CustomersController : ControllerBase
    {
        private readonly CompanyContext _context;

        public CustomersController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        //[HttpGet]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomer()
        {
            //return await _context.Customer.ToListAsync();



            var customers =  await _context.Customer
                .Include(c => c.User)
                    .ThenInclude(u => u.Address)
                .Select(c => new CustomerDTO
                {
                    Id = c.Id,
                    IdUser = c.User.Id,
                    Name = c.User.Name ?? "",
                    Surname = c.User.Surname ?? "",
                    Email = c.User.Email ?? "",
                    Phone = c.User.PhoneNumber ?? "",
                    Street = c.User.Address.Street ?? "",
                    StreetNumber = c.User.Address.StreetNumber ?? "",
                    HouseNumber = c.User.Address.HouseNumber ?? "",
                    City = c.User.Address.City ?? "",
                    Country = c.User.Address.Country ?? "",
                    PostalCode = c.User.Address.PostalCode ?? "",
                })
                .ToListAsync();

            return Ok(customers);

        }

        // GET: api/Customers/5
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerDTO customerDto)
        {
            if(id != customerDto.Id) return BadRequest("Id mismatch");

            var customer = await _context.Customer
                .Include(c => c.User)
                    .ThenInclude(u => u.Address)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(customer == null) return NotFound($"Customer with id {id} not found");

            customer.User.Name = customerDto.Name;
            customer.User.Surname = customerDto.Surname;
            customer.User.Email = customerDto.Email;
            customer.User.PhoneNumber = customerDto.Phone;

            customer.User.Address.Street = customerDto.Street;
            customer.User.Address.StreetNumber = customerDto.StreetNumber;
            customer.User.Address.HouseNumber = customerDto.HouseNumber;
            customer.User.Address.City = customerDto.City;
            customer.User.Address.Country = customerDto.Country;

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

            var user = new User
            {
                Name = customerDto.Name,
                Surname = customerDto.Surname,
                Email = customerDto.Email,
                PhoneNumber = customerDto.Phone,
                IdAddress = address.Id,
                Address = address,
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            var customer = new Customer
            {
                IdUser = user.Id,
                User = user,
            };

            try
            {
                _context.Customer.Add(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customer
                .Include(c => c.User)
                    .ThenInclude(u => u.Address)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound($"Customer with id {id} not found");

            _context.Customer.Remove(customer);
            _context.User.Remove(customer.User);
            _context.Address.Remove(customer.User.Address);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}
