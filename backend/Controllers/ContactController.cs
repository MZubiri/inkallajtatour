using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InkallajtaAPI.Data;
using InkallajtaAPI.DTOs;

namespace InkallajtaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly AppDbContext _db;
    public ContactController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactMessageDto>>> GetAll()
    {
        var list = await _db.ContactMessages
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new ContactMessageDto(c.Id, c.Name, c.Email, c.Phone, c.Subject, c.Message, c.IsRead, c.CreatedAt))
            .ToListAsync();
        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ContactCreateDto dto)
    {
        var msg = new Models.ContactMessage { Name = dto.Name, Email = dto.Email, Phone = dto.Phone, Subject = dto.Subject, Message = dto.Message };
        _db.ContactMessages.Add(msg);
        await _db.SaveChangesAsync();
        return Ok(new { msg.Id });
    }

    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkRead(int id)
    {
        var msg = await _db.ContactMessages.FindAsync(id);
        if (msg == null) return NotFound();
        msg.IsRead = true;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var msg = await _db.ContactMessages.FindAsync(id);
        if (msg == null) return NotFound();
        _db.ContactMessages.Remove(msg);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
