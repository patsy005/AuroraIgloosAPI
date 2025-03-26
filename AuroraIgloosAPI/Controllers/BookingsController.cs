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
using AuroraIgloosAPI.BussinessLogic;

namespace AuroraIgloosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public BookingsController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBooking()
        {

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
                    PaymentMethodId = b.PaymentMethodId

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
        public async Task<IActionResult> PutBooking(int id, BookingDTO bookingDto)
        {
            if (id != bookingDto.Id)
            {
                return BadRequest("Id does not match");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customer.FindAsync(bookingDto.IdCustomer);

            if (customer == null)
            {
                return BadRequest("Customer not found");
            }

            var igloo = await _context.Igloo.FindAsync(bookingDto.IdIgloo);
            if (igloo == null) return BadRequest("Igloo not found");

            var employee = await _context.Employee.FindAsync(bookingDto.CreatedById);
            if (employee == null) return BadRequest("Employee not found");

            var paymentMethod = await _context.PaymentMethod.FindAsync(bookingDto.PaymentMethodId);
            if (paymentMethod == null) return BadRequest("Payment method not found");

            var bookingsLogic = new BookingsLogic(_context);
            var totalAmount = bookingsLogic.CalculateBookingTotalAmount(bookingDto.IdIgloo, bookingDto.CheckIn, bookingDto.CheckOut);

            var booking = await _context.Booking
                .Include(b => b.Customer)
                    .ThenInclude(c => c.User)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.User)
                .Include(b => b.Igloo)
                .Include(b => b.PaymentMethod)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound($"Booking with id {id} not found");

            booking.IdIgloo = bookingDto.IdIgloo;
            booking.IdCustomer = bookingDto.IdCustomer;
            booking.CreatedById = bookingDto.CreatedById;
            booking.BookingDate = bookingDto.BookingDate ?? booking.BookingDate;
            booking.CheckIn = bookingDto.CheckIn;
            booking.CheckOut = bookingDto.CheckOut;
            booking.Amount = bookingDto.Amount;
            booking.EarlyCheckInRequest = bookingDto.EarlyCheckInRequest;
            booking.LateCheckOutRequest = bookingDto.LateCheckOutRequest;
            booking.PaymentMethodId = bookingDto.PaymentMethodId;
            booking.Customer = customer;
            booking.Igloo = igloo;
            booking.Employee = employee;
            booking.PaymentMethod = paymentMethod;
            booking.Amount = totalAmount ?? booking.Amount;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Booking.Any(e => e.Id == id))
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
        public async Task<ActionResult<Booking>> PostBooking(BookingDTO bookingDto)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customer.FindAsync(bookingDto.IdCustomer);

            if(customer == null)
            {
                return BadRequest("Customer not found");
            }

            var igloo = await _context.Igloo.FindAsync(bookingDto.IdIgloo);
            if(igloo == null) return BadRequest("Igloo not found");

            var employee = await _context.Employee.FindAsync(bookingDto.CreatedById);
            if (employee == null) return BadRequest("Employee not found");

            var paymentMethod = await _context.PaymentMethod.FindAsync(bookingDto.PaymentMethodId);
            if (paymentMethod == null) return BadRequest("Payment method not found");

            var bookingsLogic = new BookingsLogic(_context);
            var totalAmount = bookingsLogic.CalculateBookingTotalAmount(bookingDto.IdIgloo, bookingDto.CheckIn, bookingDto.CheckOut);

            var booking = new Booking
            {
                IdIgloo = bookingDto.IdIgloo,
                IdCustomer = bookingDto.IdCustomer,
                CreatedById = bookingDto.CreatedById,
                CheckIn = bookingDto.CheckIn,
                CheckOut = bookingDto.CheckOut,
                PaymentMethodId = bookingDto.PaymentMethodId,
                Amount = totalAmount ?? 0.0m,
                EarlyCheckInRequest = bookingDto.EarlyCheckInRequest,
                LateCheckOutRequest = bookingDto.LateCheckOutRequest,
                BookingDate = bookingDto.BookingDate ?? DateOnly.FromDateTime(DateTime.Now),
                Customer = customer ,
                Igloo = igloo,
                PaymentMethod = paymentMethod,
                Employee = employee
            };

            try
            {
                _context.Booking.Add(booking);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }

            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {

            var booking = await _context.Booking
                .Include(b => b.Customer)
                    .ThenInclude(c => c.User)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.User)
                .Include(b => b.Igloo)
                .Include(b => b.PaymentMethod)
                .FirstOrDefaultAsync(b => b.Id == id);

            if(booking == null) return NotFound($"Booking with id {id} not found");

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
