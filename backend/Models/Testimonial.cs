using System.ComponentModel.DataAnnotations;

namespace InkallajtaAPI.Models;

public class Testimonial
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string ClientName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Origin { get; set; } = string.Empty;

    [MaxLength(500)]
    public string PhotoUrl { get; set; } = string.Empty;

    [Required, MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;

    public double Rating { get; set; } = 5.0;

    public int? TourId { get; set; }

    public Tour? Tour { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
