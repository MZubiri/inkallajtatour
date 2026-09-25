using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InkallajtaAPI.Data;
using InkallajtaAPI.DTOs;

namespace InkallajtaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToursController : ControllerBase
{
    private readonly AppDbContext _db;
    public ToursController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TourListDto>>> GetAll(
        [FromQuery] int? categoryId,
        [FromQuery] bool? featured,
        [FromQuery] string? search)
    {
        var query = _db.Tours
            .Include(t => t.Category)
            .Where(t => t.IsActive)
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);

        if (featured == true)
            query = query.Where(t => t.IsFeatured);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t => t.Title.Contains(search) || t.ShortDescription.Contains(search));

        var tours = await query
            .OrderBy(t => t.SortOrder)
            .Select(t => new TourListDto(
                t.Id, t.Title, t.Slug, t.ShortDescription,
                t.Price, t.PriceType, t.DiscountPrice,
                t.Duration, t.Difficulty, t.Location,
                t.ImageUrl, t.Rating, t.ReviewCount,
                t.CategoryId, t.Category!.Name, t.IsFeatured
            ))
            .ToListAsync();

        return Ok(tours);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<TourDetailDto>> GetBySlug(string slug)
    {
        var tour = await _db.Tours
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Slug == slug && t.IsActive);

        if (tour == null) return NotFound();

        return Ok(new TourDetailDto(
            tour.Id, tour.Title, tour.Slug, tour.ShortDescription, tour.Description,
            tour.Itinerary, tour.Includes, tour.Excludes,
            tour.Price, tour.PriceType, tour.DiscountPrice,
            tour.Duration, tour.Difficulty, tour.MaxGroupSize, tour.Location,
            tour.ImageUrl, tour.GalleryImages,
            tour.Rating, tour.ReviewCount,
            tour.CategoryId, tour.Category!.Name, tour.IsFeatured
        ));
    }

    [HttpGet("by-id/{id:int}")]
    public async Task<ActionResult<TourDetailDto>> GetById(int id)
    {
        var tour = await _db.Tours
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tour == null) return NotFound();

        return Ok(new TourDetailDto(
            tour.Id, tour.Title, tour.Slug, tour.ShortDescription, tour.Description,
            tour.Itinerary, tour.Includes, tour.Excludes,
            tour.Price, tour.PriceType, tour.DiscountPrice,
            tour.Duration, tour.Difficulty, tour.MaxGroupSize, tour.Location,
            tour.ImageUrl, tour.GalleryImages,
            tour.Rating, tour.ReviewCount,
            tour.CategoryId, tour.Category!.Name, tour.IsFeatured
        ));
    }

    [HttpPost]
    public async Task<ActionResult<TourDetailDto>> Create([FromBody] TourCreateDto dto)
    {
        var tour = new Models.Tour
        {
            Title = dto.Title, Slug = dto.Slug, ShortDescription = dto.ShortDescription,
            Description = dto.Description, Itinerary = dto.Itinerary,
            Includes = dto.Includes, Excludes = dto.Excludes,
            Price = dto.Price, PriceType = dto.PriceType, DiscountPrice = dto.DiscountPrice,
            Duration = dto.Duration, Difficulty = dto.Difficulty, MaxGroupSize = dto.MaxGroupSize,
            Location = dto.Location, ImageUrl = dto.ImageUrl, GalleryImages = dto.GalleryImages,
            Rating = dto.Rating, ReviewCount = dto.ReviewCount,
            CategoryId = dto.CategoryId, IsFeatured = dto.IsFeatured,
            IsActive = dto.IsActive, SortOrder = dto.SortOrder
        };
        _db.Tours.Add(tour);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = tour.Id }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TourCreateDto dto)
    {
        var tour = await _db.Tours.FindAsync(id);
        if (tour == null) return NotFound();

        tour.Title = dto.Title; tour.Slug = dto.Slug; tour.ShortDescription = dto.ShortDescription;
        tour.Description = dto.Description; tour.Itinerary = dto.Itinerary;
        tour.Includes = dto.Includes; tour.Excludes = dto.Excludes;
        tour.Price = dto.Price; tour.PriceType = dto.PriceType; tour.DiscountPrice = dto.DiscountPrice;
        tour.Duration = dto.Duration; tour.Difficulty = dto.Difficulty; tour.MaxGroupSize = dto.MaxGroupSize;
        tour.Location = dto.Location; tour.ImageUrl = dto.ImageUrl; tour.GalleryImages = dto.GalleryImages;
        tour.Rating = dto.Rating; tour.ReviewCount = dto.ReviewCount;
        tour.CategoryId = dto.CategoryId; tour.IsFeatured = dto.IsFeatured;
        tour.IsActive = dto.IsActive; tour.SortOrder = dto.SortOrder;
        tour.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tour = await _db.Tours.FindAsync(id);
        if (tour == null) return NotFound();
        _db.Tours.Remove(tour);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Admin: get all tours including inactive
    [HttpGet("admin/all")]
    public async Task<ActionResult<IEnumerable<TourListDto>>> GetAllAdmin()
    {
        var tours = await _db.Tours
            .Include(t => t.Category)
            .OrderBy(t => t.SortOrder)
            .Select(t => new TourListDto(
                t.Id, t.Title, t.Slug, t.ShortDescription,
                t.Price, t.PriceType, t.DiscountPrice,
                t.Duration, t.Difficulty, t.Location,
                t.ImageUrl, t.Rating, t.ReviewCount,
                t.CategoryId, t.Category!.Name, t.IsFeatured
            ))
            .ToListAsync();

        return Ok(tours);
    }
}
