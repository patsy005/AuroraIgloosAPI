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
using AutoMapper;
using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.BussinessLogic;
using Microsoft.AspNetCore.Authorization;

namespace AuroraIgloosAPI.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "Admin,Staff,ReadOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBooking()
        {

            var booking = await _context.Booking
                .Include(b => b.Customer)
                    .ThenInclude(c => c.Person)
                    .ThenInclude(u => u.Address)
                .Include(b => b.Customer)
                    .ThenInclude(c => c.User)
                .Include(b => b.Igloo)
                .Include(b => b.PaymentMethod)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.Season)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.LevelOfDifficulty)
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    IdIgloo = b.IdIgloo,
                    IdCustomer = b.IdCustomer,
                    // CreatedById = b.CreatedById,
                    BookingDate = b.BookingDate,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    Amount = b.Amount,
                    CustomerName = b.Customer.Person.Name,
                    CustomerSurname = b.Customer.Person.Surname,
                    CustomerEmail = b.Customer.Person.Email,
                    CustomerPhone = b.Customer.Person.PhoneNumber,
                    IglooName = b.Igloo.Name ?? "",
                    PaymentMethodName = b.PaymentMethod.Name,
                    PaymentMethodId = b.PaymentMethodId,
                    TripId = b.TripId,
                    TripName = b.Trip.Name ?? "",
                    TripDate = b.TripDate,
                    Guests = b.Guests,
                    EarlyCheckInRequest = b.EarlyCheckInRequest,
                    LateCheckOutRequest = b.LateCheckOutRequest,

                })
                .ToListAsync();

            return Ok(booking);
        }

        // GET: api/Bookings/5
        [Authorize(Roles = "Admin,Staff,ReadOnly")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Booking
                .Include(b => b.Customer)
                    .ThenInclude(c => c.Person)
                    .ThenInclude(u => u.Address)
                .Include(b => b.Customer)
                    .ThenInclude(c => c.User)
                .Include(b => b.Igloo)
                .Include(b => b.PaymentMethod)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.Season)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.LevelOfDifficulty)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, BookingFormDTO bookingDto)
        {
            if (id != bookingDto.Id)
            {
                return BadRequest("Id does not match");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var validation = await BookingCheck(bookingDto, ignoreBookingId: id);
            if (validation != null)
                return validation;
            
            var totalAmount = await CalcBookingPrice(bookingDto);

            var booking = await _context.Booking
                .Include(b => b.Customer)
                    .ThenInclude(c => c.Person)
                .Include(b => b.Igloo)
                .Include(b => b.PaymentMethod)
                .Include(b => b.Trip)
                .Include(b => b.Trip)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound($"Booking with id {id} not found");
            
            var now = DateOnly.FromDateTime(DateTime.Now);

            booking.IdIgloo = bookingDto.IdIgloo;
            booking.IdCustomer = bookingDto.IdCustomer;
            booking.BookingDate = bookingDto.BookingDate ?? booking.BookingDate;
            booking.CheckIn = bookingDto.CheckIn;
            booking.CheckOut = bookingDto.CheckOut;
            booking.Amount = bookingDto.Amount;
            booking.EarlyCheckInRequest = bookingDto.EarlyCheckInRequest;
            booking.LateCheckOutRequest = bookingDto.LateCheckOutRequest;
            booking.PaymentMethodId = bookingDto.PaymentMethodId;
            booking.Amount = totalAmount ?? booking.Amount;
            booking.TripId = bookingDto.TripId;
            booking.UpdateDate = now;
            booking.Guests = bookingDto.Guests;
            booking.TripDate = bookingDto.TripDate;

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
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BookingDTO>> PostBooking(BookingFormDTO bookingDto)

        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validation = await BookingCheck(bookingDto, null);
            if (validation != null)
                return validation;


            var totalAmount = await CalcBookingPrice(bookingDto);

            var booking = new Booking
            {
                IdIgloo = bookingDto.IdIgloo ?? null,
                IdCustomer = bookingDto.IdCustomer,
                // CreatedById = bookingDto.CreatedById,
                CheckIn = bookingDto.CheckIn ?? null,
                CheckOut = bookingDto.CheckOut ?? null,
                PaymentMethodId = bookingDto.PaymentMethodId,
                Amount = totalAmount ?? 0.0m,
                EarlyCheckInRequest = bookingDto.EarlyCheckInRequest ?? null,
                LateCheckOutRequest = bookingDto.LateCheckOutRequest ?? null,
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                TripId = bookingDto.TripId ?? null,
                Guests = bookingDto.Guests,
                TripDate = bookingDto.TripDate,

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
            
            var created = await _context.Booking
                .Where(b => b.Id == booking.Id)
                .Include(b => b.Customer).ThenInclude(c => c.Person)
                .Include(b => b.Igloo)
                .Include(b => b.PaymentMethod)
                .Include(b => b.Trip)
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    IdIgloo = b.IdIgloo,
                    IdCustomer = b.IdCustomer,
                    BookingDate = b.BookingDate,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    Amount = b.Amount,
                    CustomerName = b.Customer.Person.Name,
                    CustomerSurname = b.Customer.Person.Surname,
                    CustomerEmail = b.Customer.Person.Email,
                    CustomerPhone = b.Customer.Person.PhoneNumber,
                    IglooName = b.Igloo != null ? b.Igloo.Name ?? "" : "",
                    PaymentMethodName = b.PaymentMethod.Name,
                    PaymentMethodId = b.PaymentMethodId,
                    TripId = b.TripId,
                    TripName = b.Trip != null ? b.Trip.Name ?? "" : "",
                    Guests = b.Guests,
                    TripDate = b.TripDate,
                    EarlyCheckInRequest = b.EarlyCheckInRequest,
                    LateCheckOutRequest = b.LateCheckOutRequest
                })
                .FirstAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = created.Id }, created);
        }

        // DELETE: api/Bookings/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {

            var booking = await _context.Booking
                .Include(b => b.Customer)
                    .ThenInclude(c => c.Person)
                .Include(b => b.Igloo)
                .Include(b => b.PaymentMethod)
                .Include(b => b.Trip)
                .FirstOrDefaultAsync(b => b.Id == id);

            if(booking == null) return NotFound($"Booking with id {id} not found");

            _context.Booking.Remove(booking);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetMyBookings()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
            
            var customer = await _context.Customer
                .Include(c => c.Person).ThenInclude(p => p.Address)
                .FirstOrDefaultAsync(c => c.IdUser == userId);
            
            if (customer == null) return NotFound();
            
            var bookings = await _context.Booking
                .Where(b => b.IdCustomer == customer.Id)
                .Include(b => b.Igloo)
                .Include((b => b.PaymentMethod))
                .Include(b => b.Trip).ThenInclude(t => t.Season)
                .Include(b => b.Trip).ThenInclude(t => t.LevelOfDifficulty)
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    IdIgloo = b.IdIgloo,
                    IdCustomer = b.IdCustomer,
                    BookingDate = b.BookingDate,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    Amount = b.Amount,
                    
                    CustomerName = customer.Person.Name,
                    CustomerSurname = customer.Person.Surname,
                    CustomerEmail = customer.Person.Email,
                    CustomerPhone = customer.Person.PhoneNumber,
                    
                    IglooName = b.Igloo.Name ?? "",
                    PaymentMethodName = b.PaymentMethod.Name ?? "",
                    PaymentMethodId = b.PaymentMethodId,
                    TripId = b.TripId,
                    TripName = b.Trip.Name ?? "",
                    Guests = b.Guests,
                    TripDate = b.TripDate ?? null,
                    EarlyCheckInRequest = b.EarlyCheckInRequest ?? false,
                    LateCheckOutRequest = b.LateCheckOutRequest ?? false,
                })
                .ToListAsync();
            
            return bookings;
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }

        private async Task<ActionResult?> IglooBookCheck(BookingFormDTO bookingDto)
        {
            
            if (!bookingDto.IdIgloo.HasValue) return null;
            
            var iglooExists = await _context.Igloo.AnyAsync(b => b.Id == bookingDto.IdIgloo.Value);
            if(!iglooExists) return BadRequest("Igloo not found");
            
            if(!bookingDto.CheckIn.HasValue) return BadRequest("Check in date is required");
            if(!bookingDto.CheckOut.HasValue) return BadRequest("Checkout date is required");

            if (iglooExists)
            {
                var igloo = await _context.Igloo.FindAsync(bookingDto.IdIgloo);
                if(igloo == null) return BadRequest("Igloo not found");
                if(igloo.Capacity < bookingDto.Guests) return BadRequest("Igloo's capacity is less than guests");
            }
            
            return null;
        }

        private async Task<ActionResult?> TripBookCheck(BookingFormDTO bookingDto)
        {
            if (!bookingDto.TripId.HasValue) return null;
            
            var tripExists = await _context.Trip.AnyAsync(t => t.Id == bookingDto.TripId.Value);
            if(!tripExists) return BadRequest("Trip not found");
            
            if(!bookingDto.TripDate.HasValue) return BadRequest("Trip date is required");
            
            return null;
        }

        private async Task<ActionResult?> BookingCheck(BookingFormDTO bookingDto, int? ignoreBookingId )
        {
            if (bookingDto.IdIgloo == null && bookingDto.TripId == null)
            {
                return BadRequest("Booking must contain at leat an Igloo or a Trip");
            }

            if (bookingDto.Guests <= 0)
            {
                return BadRequest("Booking must contain at least one guest");
            }

            var customer = await _context.Customer.FindAsync(bookingDto.IdCustomer);

            if (customer == null)
            {
                return BadRequest("Customer not found");
            }
            
            var paymentMethod = await _context.PaymentMethod.FindAsync(bookingDto.PaymentMethodId);
            if (paymentMethod == null) return BadRequest("Payment method not found");

            var iglooValidation = await IglooBookCheck(bookingDto);
            if (iglooValidation != null) return iglooValidation;
            
            var tripValidation = await TripBookCheck(bookingDto);
            if (tripValidation != null) return tripValidation;
            
            var iglooAvailability = await IglooAvailabilityCheck(bookingDto, ignoreBookingId);
            if (iglooAvailability != null) return iglooAvailability;

            var guideAvailability = await GuideAvailability(bookingDto, ignoreBookingId);
            if (guideAvailability != null) return guideAvailability;

            return null;
        }

        private async Task<ActionResult?> IglooAvailabilityCheck(BookingFormDTO bookingDto, int? ignoreBookingId)
        {
            if(!bookingDto.IdIgloo.HasValue) return null;

            var conflict = await _context.Booking
                .Where(b => b.IdIgloo == bookingDto.IdIgloo)
                .Where(b => ignoreBookingId == null || b.IdIgloo == ignoreBookingId)
                .AnyAsync(b =>
                    bookingDto.CheckIn < b.CheckOut &&
                    bookingDto.CheckOut > b.CheckIn
                    );
            
            return conflict ? Conflict("Igloo already booked in selecred period") : null;
        }

        private async Task<ActionResult?> GuideAvailability(BookingFormDTO bookingDto, int? ignoreBookingId)
        {
            // sprawdzamy trip, więc wymagamy TripId i TripDate
            if (!bookingDto.TripId.HasValue) return null;
            if (!bookingDto.TripDate.HasValue) return null;

            var trip = await _context.Trip.FindAsync(bookingDto.TripId.Value);
            if (trip?.GuideId == null) return null;

            var conflict = await _context.Booking
                .Where(b => b.TripDate == bookingDto.TripDate)
                .Where(b => ignoreBookingId == null || b.Id != ignoreBookingId) 
                .Join(
                    _context.Trip,
                    b => b.TripId!.Value,
                    t => t.Id,
                    (b, t) => new { b, t }
                )
                .AnyAsync(x => x.t.GuideId == trip.GuideId);

            return conflict ? Conflict("Guide already booked in selected period") : null;
        }


        private async Task<decimal?> CalcBookingPrice(BookingFormDTO bookingDto)
        {
            decimal? iglooPrice = 0;
            decimal? tripPrice = 0;
            
            var bookingsLogic = new BookingsLogic(_context);

            if (bookingDto.IdIgloo.HasValue)
            {
                iglooPrice = bookingsLogic.CalculateBookingTotalAmount(bookingDto.IdIgloo, bookingDto.CheckIn, bookingDto.CheckOut, bookingDto.BookingDate);
            }

            if (bookingDto.TripId.HasValue)
            {
                var trip = await _context.Trip.FirstOrDefaultAsync(t => t.Id == bookingDto.TripId.Value);
                tripPrice = trip?.PricePerPerson * bookingDto.Guests;
            }
            
            decimal? totalPrice = iglooPrice + tripPrice;
            
            return totalPrice;
        }
    }
}
