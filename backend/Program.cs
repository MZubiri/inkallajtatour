using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using InkallajtaAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// MySQL
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(connStr, new MySqlServerVersion(new Version(8, 0, 36)), mySqlOpt =>
        mySqlOpt.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null)));

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// CORS - allow Angular dev server and production domains
var allowedOriginsConfig = builder.Configuration["Cors:AllowedOrigins"];
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAngular", policy =>
    {
        if (!string.IsNullOrWhiteSpace(allowedOriginsConfig) && allowedOriginsConfig != "*")
        {
            var origins = allowedOriginsConfig.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            policy.WithOrigins(origins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
        else
        {
            policy.SetIsOriginAllowed(_ => true)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

var app = builder.Build();

// Auto-migrate database & sync curated images
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var retries = 10;
    while (retries > 0)
    {
        try
        {
            await db.Database.EnsureCreatedAsync();
            break;
        }
        catch (Exception ex)
        {
            retries--;
            if (retries == 0) throw;
            Console.WriteLine($"Esperando a que MySQL esté listo... ({retries} intentos restantes): {ex.Message}");
            await Task.Delay(3000);
        }
    }

    var toursToUpdate = new Dictionary<int, (string ImageUrl, string GalleryImages)>
    {
        { 1, ("https://images.unsplash.com/photo-1587595431973-160d0d94add1?w=800", "[\"https://images.unsplash.com/photo-1526392060635-9d6019884377?w=800\",\"https://images.unsplash.com/photo-1580619305218-8423a7ef79b4?w=800\",\"https://images.unsplash.com/photo-1565008447742-97f6f38c985c?w=800\"]") },
        { 2, ("https://images.unsplash.com/photo-1659554282192-9a933b30d73d?w=800", "[\"https://images.unsplash.com/photo-1586724237569-f3d0c1dee8c6?w=800\",\"https://images.unsplash.com/photo-1533038590840-1cde6e668a91?w=800\",\"https://images.unsplash.com/photo-1587595431973-160d0d94add1?w=800\"]") },
        { 3, ("https://images.unsplash.com/photo-1580619305218-8423a7ef79b4?w=800", "[\"https://images.unsplash.com/photo-1565008447742-97f6f38c985c?w=800\",\"https://images.unsplash.com/photo-1659554282192-9a933b30d73d?w=800\",\"https://images.unsplash.com/photo-1526392060635-9d6019884377?w=800\"]") },
        { 4, ("https://images.unsplash.com/photo-1589802829985-817e51171b92?w=800", "[\"https://images.unsplash.com/photo-1565008447742-97f6f38c985c?w=800\",\"https://images.unsplash.com/photo-1580619305218-8423a7ef79b4?w=800\",\"https://images.unsplash.com/photo-1533038590840-1cde6e668a91?w=800\"]") },
        { 5, ("https://images.unsplash.com/photo-1545330785-15356daae141?w=800", "[\"https://images.unsplash.com/photo-1544644181-1484b3fdfc62?w=800\",\"https://images.unsplash.com/photo-1509316975850-ff9c5deb0cd9?w=800\",\"https://images.unsplash.com/photo-1589182373726-e4f658ab50f0?w=800\"]") },
        { 6, ("https://images.unsplash.com/photo-1611843467160-25afb8df1074?w=800", "[\"https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=800\",\"https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=800\",\"https://images.unsplash.com/photo-1488646953014-85cb44e25828?w=800\"]") },
        { 7, ("https://images.unsplash.com/photo-1596401057633-54a8fe8ef647?w=800", "[\"https://images.unsplash.com/photo-1589182373726-e4f658ab50f0?w=800\",\"https://images.unsplash.com/photo-1533038590840-1cde6e668a91?w=800\",\"https://images.unsplash.com/photo-1659554282192-9a933b30d73d?w=800\"]") },
        { 8, ("https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=800", "[\"https://images.unsplash.com/photo-1611843467160-25afb8df1074?w=800\",\"https://images.unsplash.com/photo-1509316975850-ff9c5deb0cd9?w=800\",\"https://images.unsplash.com/photo-1530789253388-582c481c54b0?w=800\",\"https://images.unsplash.com/photo-1587595431973-160d0d94add1?w=800\"]") },
        { 9, ("https://images.unsplash.com/photo-1586724237569-f3d0c1dee8c6?w=800", "[\"https://images.unsplash.com/photo-1589802829985-817e51171b92?w=800\",\"https://images.unsplash.com/photo-1565008447742-97f6f38c985c?w=800\",\"https://images.unsplash.com/photo-1580619305218-8423a7ef79b4?w=800\"]") },
        { 10, ("https://images.unsplash.com/photo-1553550765-41e7dff2bd41?w=800", "[\"https://images.unsplash.com/photo-1620417396507-9a57523d16a6?w=800\",\"https://images.unsplash.com/photo-1534447677768-be436bb09401?w=800\",\"https://images.unsplash.com/photo-1587595431973-160d0d94add1?w=800\"]") }
    };

    bool hasChanges = false;
    foreach (var (id, (img, gallery)) in toursToUpdate)
    {
        var tour = await db.Tours.FindAsync(id);
        if (tour != null && (tour.ImageUrl != img || tour.GalleryImages != gallery))
        {
            tour.ImageUrl = img;
            tour.GalleryImages = gallery;
            hasChanges = true;
        }
    }
    if (hasChanges)
    {
        await db.SaveChangesAsync();
    }
}

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
