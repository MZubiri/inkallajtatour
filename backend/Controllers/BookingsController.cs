using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InkallajtaAPI.Data;
using InkallajtaAPI.DTOs;

namespace InkallajtaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _db;
    public BookingsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll([FromQuery] string? status)
    {
        var query = _db.Bookings.Include(b => b.Tour).AsQueryable();
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(b => b.Status == status);

        var list = await query
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new BookingDto(b.Id, b.ClientName, b.Email, b.Phone, b.Country, b.TourId, b.Tour!.Title, b.TravelDate, b.NumberOfPeople, b.TotalPrice, b.SpecialRequests, b.Status, b.CreatedAt))
            .ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookingDto>> GetById(int id)
    {
        var b = await _db.Bookings.Include(b => b.Tour).FirstOrDefaultAsync(b => b.Id == id);
        if (b == null) return NotFound();
        return Ok(new BookingDto(b.Id, b.ClientName, b.Email, b.Phone, b.Country, b.TourId, b.Tour!.Title, b.TravelDate, b.NumberOfPeople, b.TotalPrice, b.SpecialRequests, b.Status, b.CreatedAt));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] BookingCreateDto dto)
    {
        var tour = await _db.Tours.FindAsync(dto.TourId);
        if (tour == null) return BadRequest("Tour no encontrado");

        var booking = new Models.Booking
        {
            ClientName = dto.ClientName, Email = dto.Email, Phone = dto.Phone,
            Country = dto.Country, TourId = dto.TourId, TravelDate = dto.TravelDate,
            NumberOfPeople = dto.NumberOfPeople,
            TotalPrice = tour.Price * dto.NumberOfPeople,
            SpecialRequests = dto.SpecialRequests, Status = "pending"
        };
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();
        return Ok(new { booking.Id, booking.TotalPrice });
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] BookingUpdateStatusDto dto)
    {
        var b = await _db.Bookings.FindAsync(id);
        if (b == null) return NotFound();
        b.Status = dto.Status;
        b.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var b = await _db.Bookings.FindAsync(id);
        if (b == null) return NotFound();
        _db.Bookings.Remove(b);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
