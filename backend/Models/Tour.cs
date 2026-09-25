using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkallajtaAPI.Models;

public class Tour
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(500)]
    public string ShortDescription { get; set; } = string.Empty;

    [Column(TypeName = "text")]
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "text")]
    public string Itinerary { get; set; } = string.Empty;

    [Column(TypeName = "text")]
    public string Includes { get; set; } = string.Empty;

    [Column(TypeName = "text")]
    public string Excludes { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [MaxLength(20)]
    public string PriceType { get; set; } = "per_person"; // per_person, per_group

    [Column(TypeName = "decimal(10,2)")]
    public decimal? DiscountPrice { get; set; }

    [MaxLength(50)]
    public string Duration { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Difficulty { get; set; } = "moderate"; // easy, moderate, hard, extreme

    public int MaxGroupSize { get; set; } = 12;

    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    [MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [Column(TypeName = "text")]
    public string GalleryImages { get; set; } = "[]"; // JSON array of image URLs

    public double Rating { get; set; } = 4.5;

    public int ReviewCount { get; set; } = 0;

    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsFeatured { get; set; } = false;

    public int SortOrder { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
