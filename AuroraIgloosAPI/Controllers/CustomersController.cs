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
        //public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        //{
        //    //return await _context.Customer.ToListAsync();
        //    return await _context.Customer
        //        .Include(c => c.User)
        //        .ToListAsync();
        //}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomer()
        {
            //return await _context.Customer.ToListAsync();



            var customers =  await _context.Customer
                .Include(c => c.User)
                .Select(c => new CustomerDTO
                {
                    Id = c.Id,
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
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

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
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}
