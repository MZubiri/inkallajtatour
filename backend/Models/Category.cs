using System.ComponentModel.DataAnnotations;

namespace InkallajtaAPI.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Icon { get; set; } = string.Empty;

    public int SortOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
