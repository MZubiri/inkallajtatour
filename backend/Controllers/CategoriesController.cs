using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InkallajtaAPI.Data;
using InkallajtaAPI.DTOs;

namespace InkallajtaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;
    public CategoriesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var cats = await _db.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .Select(c => new CategoryDto(c.Id, c.Name, c.Slug, c.Description, c.ImageUrl, c.Icon, c.SortOrder, c.Tours.Count(t => t.IsActive)))
            .ToListAsync();
        return Ok(cats);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<CategoryDto>> GetBySlug(string slug)
    {
        var cat = await _db.Categories
            .Where(c => c.Slug == slug && c.IsActive)
            .Select(c => new CategoryDto(c.Id, c.Name, c.Slug, c.Description, c.ImageUrl, c.Icon, c.SortOrder, c.Tours.Count(t => t.IsActive)))
            .FirstOrDefaultAsync();
        if (cat == null) return NotFound();
        return Ok(cat);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CategoryCreateDto dto)
    {
        var cat = new Models.Category { Name = dto.Name, Slug = dto.Slug, Description = dto.Description, ImageUrl = dto.ImageUrl, Icon = dto.Icon, SortOrder = dto.SortOrder };
        _db.Categories.Add(cat);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBySlug), new { slug = cat.Slug }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryCreateDto dto)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat == null) return NotFound();
        cat.Name = dto.Name; cat.Slug = dto.Slug; cat.Description = dto.Description;
        cat.ImageUrl = dto.ImageUrl; cat.Icon = dto.Icon; cat.SortOrder = dto.SortOrder;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat == null) return NotFound();
        _db.Categories.Remove(cat);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
