using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InkallajtaAPI.Data;
using InkallajtaAPI.DTOs;

namespace InkallajtaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestimonialsController : ControllerBase
{
    private readonly AppDbContext _db;
    public TestimonialsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestimonialDto>>> GetAll()
    {
        var list = await _db.Testimonials
            .Include(t => t.Tour)
            .Where(t => t.IsActive)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TestimonialDto(t.Id, t.ClientName, t.Origin, t.PhotoUrl, t.Comment, t.Rating, t.TourId, t.Tour != null ? t.Tour.Title : null, t.CreatedAt))
            .ToListAsync();
        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TestimonialCreateDto dto)
    {
        var t = new Models.Testimonial { ClientName = dto.ClientName, Origin = dto.Origin, PhotoUrl = dto.PhotoUrl, Comment = dto.Comment, Rating = dto.Rating, TourId = dto.TourId };
        _db.Testimonials.Add(t);
        await _db.SaveChangesAsync();
        return Ok(new { t.Id });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var t = await _db.Testimonials.FindAsync(id);
        if (t == null) return NotFound();
        _db.Testimonials.Remove(t);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
