using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InkallajtaAPI.Data;
using InkallajtaAPI.DTOs;

namespace InkallajtaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _db;
    public DashboardController(AppDbContext db) => _db = db;

    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStatsDto>> GetStats()
    {
        var stats = new DashboardStatsDto(
            TotalTours: await _db.Tours.CountAsync(t => t.IsActive),
            TotalBookings: await _db.Bookings.CountAsync(),
            PendingBookings: await _db.Bookings.CountAsync(b => b.Status == "pending"),
            TotalMessages: await _db.ContactMessages.CountAsync(),
            UnreadMessages: await _db.ContactMessages.CountAsync(m => !m.IsRead),
            TotalRevenue: await _db.Bookings.Where(b => b.Status == "confirmed").SumAsync(b => b.TotalPrice)
        );
        return Ok(stats);
    }
}
