namespace InkallajtaAPI.DTOs;

// ---- Tour DTOs ----
public record TourListDto(
    int Id, string Title, string Slug, string ShortDescription,
    decimal Price, string PriceType, decimal? DiscountPrice,
    string Duration, string Difficulty, string Location,
    string ImageUrl, double Rating, int ReviewCount,
    int CategoryId, string CategoryName, bool IsFeatured
);

public record TourDetailDto(
    int Id, string Title, string Slug, string ShortDescription, string Description,
    string Itinerary, string Includes, string Excludes,
    decimal Price, string PriceType, decimal? DiscountPrice,
    string Duration, string Difficulty, int MaxGroupSize, string Location,
    string ImageUrl, string GalleryImages,
    double Rating, int ReviewCount,
    int CategoryId, string CategoryName, bool IsFeatured
);

public record TourCreateDto(
    string Title, string Slug, string ShortDescription, string Description,
    string Itinerary, string Includes, string Excludes,
    decimal Price, string PriceType, decimal? DiscountPrice,
    string Duration, string Difficulty, int MaxGroupSize, string Location,
    string ImageUrl, string GalleryImages,
    double Rating, int ReviewCount,
    int CategoryId, bool IsFeatured, bool IsActive, int SortOrder
);

// ---- Category DTOs ----
public record CategoryDto(int Id, string Name, string Slug, string Description, string ImageUrl, string Icon, int SortOrder, int TourCount);
public record CategoryCreateDto(string Name, string Slug, string Description, string ImageUrl, string Icon, int SortOrder);

// ---- Testimonial DTOs ----
public record TestimonialDto(int Id, string ClientName, string Origin, string PhotoUrl, string Comment, double Rating, int? TourId, string? TourName, DateTime CreatedAt);
public record TestimonialCreateDto(string ClientName, string Origin, string PhotoUrl, string Comment, double Rating, int? TourId);

// ---- Booking DTOs ----
public record BookingDto(int Id, string ClientName, string Email, string Phone, string Country, int TourId, string TourName, DateTime TravelDate, int NumberOfPeople, decimal TotalPrice, string SpecialRequests, string Status, DateTime CreatedAt);
public record BookingCreateDto(string ClientName, string Email, string Phone, string Country, int TourId, DateTime TravelDate, int NumberOfPeople, string SpecialRequests);
public record BookingUpdateStatusDto(string Status);

// ---- Contact DTOs ----
public record ContactMessageDto(int Id, string Name, string Email, string Phone, string Subject, string Message, bool IsRead, DateTime CreatedAt);
public record ContactCreateDto(string Name, string Email, string Phone, string Subject, string Message);

// ---- Auth DTOs ----
public record LoginDto(string Email, string Password);
public record LoginResponseDto(string Token, string FullName, string Email, string Role);

// ---- Dashboard DTOs ----
public record DashboardStatsDto(int TotalTours, int TotalBookings, int PendingBookings, int TotalMessages, int UnreadMessages, decimal TotalRevenue);
