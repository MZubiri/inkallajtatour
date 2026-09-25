using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkallajtaAPI.Models;

public class Booking
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string ClientName { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    public int TourId { get; set; }

    [ForeignKey("TourId")]
    public Tour? Tour { get; set; }

    public DateTime TravelDate { get; set; }

    public int NumberOfPeople { get; set; } = 1;

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    [MaxLength(500)]
    public string SpecialRequests { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Status { get; set; } = "pending"; // pending, confirmed, cancelled, completed

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
