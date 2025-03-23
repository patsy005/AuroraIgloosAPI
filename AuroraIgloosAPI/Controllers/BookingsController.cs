using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuroraIgloosAPI.Models;
using AuroraIgloosAPI.Models.Contexts;
using AutoMapper;
using AuroraIgloosAPI.DTOs;

namespace AuroraIgloosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly CompanyContext _context;
        //private readonly IMapper _mapper;

        public BookingsController(CompanyContext context)
        {
            _context = context;
            //_mapper = mapper;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBooking()
        {
            //return await _context.Booking.ToListAsync();
            //return await _context.Booking
            //    .Include(b => b.Customer)
            //        .ThenInclude(c => c.User)
            //    .Include(b => b.Employee)
            //        .ThenInclude(e => e.User)
            //    .Include(b => b.Igloo)
            //    .Include(b => b.PaymentMethod)
            //    .ToListAsync();

            var booking = await _context.Booking
                .Include(b => b.Customer)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.Address)
                .Include(b => b.Igloo)
                .Include(b => b.PaymentMethod)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.User)
                    .ThenInclude(u => u.Address)
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    IdIgloo = b.IdIgloo,
                    IdCustomer = b.IdCustomer,
                    CreatedById = b.CreatedById,
                    BookingDate = b.BookingDate,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    Amount = b.Amount,
                    CustomerName = b.Customer.User.Name,
                    CustomerSurname = b.Customer.User.Surname,
                    CustomerEmail = b.Customer.User.Email,
                    CustomerPhone = b.Customer.User.PhoneNumber,
                    EmployeeName = b.Employee.User.Name,
                    EmployeeSurname = b.Employee.User.Surname,
                    IglooName = b.Igloo.Name,
                    PaymentMethodName = b.PaymentMethod.Name,

                })
                .ToListAsync();

            return Ok(booking);




        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Booking
                .Include(b => b.Customer)
                    .ThenInclude(c => c.User)
                .Include(b => b.Igloo)
                .Include(b => b.PaymentMethod)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            var existingBooking = await _context.Booking
                .Include(b => b.Customer)
                    .ThenInclude(c => c.User)
                .Include(b => b.Igloo)
                .Include(b => b.PaymentMethod)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existingBooking == null) return NotFound();

            _context.Entry(existingBooking).CurrentValues.SetValues(booking);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            if(booking.Customer != null && booking.Customer.Id > 0)
            {
                _context.Entry(booking.Customer).State = EntityState.Unchanged;
            }

            _context.Booking.Add(booking);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }
    }
}
