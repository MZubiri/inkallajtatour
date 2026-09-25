using Microsoft.EntityFrameworkCore;
using InkallajtaAPI.Models;

namespace InkallajtaAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tour>()
            .HasIndex(t => t.Slug)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Slug)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Tours Machu Picchu", Slug = "tours-machu-picchu", Description = "Paquetes completos para visitar Machu Picchu", Icon = "temple_buddhist", SortOrder = 1 },
            new Category { Id = 2, Name = "Day Tours", Slug = "day-tours", Description = "Tours de un día desde Cusco", Icon = "wb_sunny", SortOrder = 2 },
            new Category { Id = 3, Name = "Treks de Aventura", Slug = "treks-aventura", Description = "Caminatas y trekkings de varios días", Icon = "hiking", SortOrder = 3 },
            new Category { Id = 4, Name = "Tours del Perú", Slug = "tours-del-peru", Description = "Circuitos turísticos por todo el Perú", Icon = "explore", SortOrder = 4 }
        );

        // Seed Tours
        modelBuilder.Entity<Tour>().HasData(
            new Tour
            {
                Id = 1,
                Title = "Tour Machu Picchu 1 Día",
                Slug = "tour-machupicchu-1-dia",
                ShortDescription = "Visita la maravilla del mundo en un tour de día completo desde Cusco con guía profesional bilingüe.",
                Description = "Descubre la ciudadela inca de Machu Picchu en un viaje inolvidable de un día. Saldremos temprano desde Cusco hacia Ollantaytambo para tomar el tren a Aguas Calientes. Un guía experto te acompañará durante todo el recorrido por la ciudadela, explicándote la historia, arquitectura y misterios de este lugar sagrado.",
                Itinerary = "[{\"time\":\"4:30 AM\",\"title\":\"Recojo del hotel\",\"desc\":\"Transfer desde tu hotel en Cusco\"},{\"time\":\"6:30 AM\",\"title\":\"Llegada a Ollantaytambo\",\"desc\":\"Abordamos el tren hacia Aguas Calientes\"},{\"time\":\"8:00 AM\",\"title\":\"Llegada a Aguas Calientes\",\"desc\":\"Bus de subida a Machu Picchu\"},{\"time\":\"9:00 AM\",\"title\":\"Tour guiado en Machu Picchu\",\"desc\":\"Recorrido de 2.5 horas con guía profesional\"},{\"time\":\"12:00 PM\",\"title\":\"Tiempo libre\",\"desc\":\"Explora por tu cuenta y toma fotos\"},{\"time\":\"4:30 PM\",\"title\":\"Tren de retorno\",\"desc\":\"Regreso a Ollantaytambo\"},{\"time\":\"8:00 PM\",\"title\":\"Llegada a Cusco\",\"desc\":\"Transfer a tu hotel\"}]",
                Includes = "Transfer hotel-estación-hotel,Tren turístico ida y vuelta,Bus de subida y bajada a Machu Picchu,Entrada a Machu Picchu,Guía profesional bilingüe,Asistencia permanente",
                Excludes = "Alimentación,Propinas,Gastos personales",
                Price = 250,
                PriceType = "per_person",
                Duration = "1 día",
                Difficulty = "easy",
                MaxGroupSize = 16,
                Location = "Cusco - Machu Picchu",
                ImageUrl = "https://images.unsplash.com/photo-1587595431973-160d0d94add1?w=800",
                GalleryImages = "[\"https://images.unsplash.com/photo-1526392060635-9d6019884377?w=800\",\"https://images.unsplash.com/photo-1580619305218-8423a7ef79b4?w=800\",\"https://images.unsplash.com/photo-1565008447742-97f6f38c985c?w=800\"]",
                Rating = 4.8,
                ReviewCount = 245,
                CategoryId = 1,
                IsFeatured = true,
                SortOrder = 1
            },
            new Tour
            {
                Id = 2,
                Title = "Valle Sagrado Conexión a Machu Picchu 2D/1N",
                Slug = "valle-sagrado-machupicchu-2d-1n",
                ShortDescription = "Combina el Valle Sagrado de los Incas con Machu Picchu en una experiencia de 2 días inolvidable.",
                Description = "Explora los sitios arqueológicos más impresionantes del Valle Sagrado y luego maravíllate con Machu Picchu. Este tour de 2 días incluye visitas a Pisac, Ollantaytambo, pernocte en Aguas Calientes y tour completo en Machu Picchu.",
                Itinerary = "[{\"time\":\"Día 1 - 7:00 AM\",\"title\":\"Valle Sagrado\",\"desc\":\"Visita a Pisac (mercado y ruinas), almuerzo buffet, Ollantaytambo, tren a Aguas Calientes\"},{\"time\":\"Día 2 - 5:30 AM\",\"title\":\"Machu Picchu\",\"desc\":\"Bus a la ciudadela, tour guiado de 2.5h, tiempo libre, retorno a Cusco\"}]",
                Includes = "Transport privado,Guía profesional bilingüe,Entrada al Valle Sagrado,Almuerzo buffet día 1,Tren turístico,Hotel en Aguas Calientes,Bus subida/bajada MP,Entrada a Machu Picchu",
                Excludes = "Desayuno día 1,Cena,Almuerzo día 2,Propinas",
                Price = 420,
                PriceType = "per_person",
                Duration = "2 días / 1 noche",
                Difficulty = "easy",
                MaxGroupSize = 12,
                Location = "Valle Sagrado - Machu Picchu",
                ImageUrl = "https://images.unsplash.com/photo-1659554282192-9a933b30d73d?w=800",
                GalleryImages = "[\"https://images.unsplash.com/photo-1586724237569-f3d0c1dee8c6?w=800\",\"https://images.unsplash.com/photo-1533038590840-1cde6e668a91?w=800\",\"https://images.unsplash.com/photo-1587595431973-160d0d94add1?w=800\"]",
                Rating = 4.9,
                ReviewCount = 189,
                CategoryId = 1,
                IsFeatured = true,
                SortOrder = 2
            },
            new Tour
            {
                Id = 3,
                Title = "Cusco y Machu Picchu 3D/2N",
                Slug = "cusco-machupicchu-3d-2n",
                ShortDescription = "Paquete completo: City Tour, Valle Sagrado y Machu Picchu en 3 días de aventura cultural.",
                Description = "El paquete perfecto para conocer lo mejor de Cusco y Machu Picchu en 3 días. Incluye City Tour por la ciudad imperial, visita completa al Valle Sagrado y el mágico Machu Picchu.",
                Itinerary = "[{\"time\":\"Día 1\",\"title\":\"City Tour Cusco\",\"desc\":\"Sacsayhuamán, Qenqo, Puca Pucara, Tambomachay, Catedral, Koricancha\"},{\"time\":\"Día 2\",\"title\":\"Valle Sagrado\",\"desc\":\"Pisac, Ollantaytambo, tren a Aguas Calientes\"},{\"time\":\"Día 3\",\"title\":\"Machu Picchu\",\"desc\":\"Tour guiado y retorno a Cusco\"}]",
                Includes = "Transfers,Guía profesional,Entradas a todos los sitios,Hotel en Aguas Calientes,Trenes,Bus MP,Almuerzo día 2",
                Excludes = "Vuelos,Alimentación no mencionada,Propinas",
                Price = 550,
                PriceType = "per_person",
                Duration = "3 días / 2 noches",
                Difficulty = "easy",
                MaxGroupSize = 12,
                Location = "Cusco - Valle Sagrado - Machu Picchu",
                ImageUrl = "https://images.unsplash.com/photo-1580619305218-8423a7ef79b4?w=800",
                GalleryImages = "[\"https://images.unsplash.com/photo-1565008447742-97f6f38c985c?w=800\",\"https://images.unsplash.com/photo-1659554282192-9a933b30d73d?w=800\",\"https://images.unsplash.com/photo-1526392060635-9d6019884377?w=800\"]",
                Rating = 4.7,
                ReviewCount = 156,
                CategoryId = 1,
                IsFeatured = true,
                SortOrder = 3
            },
            new Tour
            {
                Id = 4,
                Title = "City Tour Cusco",
                Slug = "city-tour-cusco",
                ShortDescription = "Recorre los monumentos históricos más importantes de la ciudad imperial del Cusco en medio día.",
                Description = "Descubre la riqueza histórica y cultural de la ciudad del Cusco. Visitarás la imponente Catedral, el Templo del Sol (Koricancha), la fortaleza de Sacsayhuamán, y los sitios arqueológicos de Qenqo, Puca Pucara y Tambomachay.",
                Itinerary = "[{\"time\":\"1:00 PM\",\"title\":\"Recojo del hotel\",\"desc\":\"Inicio del tour\"},{\"time\":\"1:30 PM\",\"title\":\"Koricancha\",\"desc\":\"Templo del Sol, base del Convento de Santo Domingo\"},{\"time\":\"2:30 PM\",\"title\":\"Sacsayhuamán\",\"desc\":\"Impresionante fortaleza inca con bloques megalíticos\"},{\"time\":\"3:30 PM\",\"title\":\"Qenqo y Puca Pucara\",\"desc\":\"Centro ceremonial y puesto de vigilancia\"},{\"time\":\"4:30 PM\",\"title\":\"Tambomachay\",\"desc\":\"Templo del agua\"},{\"time\":\"5:30 PM\",\"title\":\"Catedral del Cusco\",\"desc\":\"Joya de la arquitectura colonial\"},{\"time\":\"6:30 PM\",\"title\":\"Retorno al hotel\",\"desc\":\"Fin del tour\"}]",
                Includes = "Transporte turístico,Guía profesional bilingüe,Entrada a los sitios arqueológicos",
                Excludes = "Alimentación,Entrada a Koricancha (S/. 15),Propinas",
                Price = 25,
                PriceType = "per_person",
                Duration = "Medio día (5 horas)",
                Difficulty = "easy",
                MaxGroupSize = 20,
                Location = "Cusco",
                ImageUrl = "https://images.unsplash.com/photo-1589802829985-817e51171b92?w=800",
                GalleryImages = "[\"https://images.unsplash.com/photo-1565008447742-97f6f38c985c?w=800\",\"https://images.unsplash.com/photo-1580619305218-8423a7ef79b4?w=800\",\"https://images.unsplash.com/photo-1533038590840-1cde6e668a91?w=800\"]",
                Rating = 4.6,
                ReviewCount = 312,
                CategoryId = 2,
                IsFeatured = false,
                SortOrder = 1
            },
            new Tour
            {
                Id = 5,
                Title = "Montaña de 7 Colores (Vinicunca)",
                Slug = "montana-7-colores",
                ShortDescription = "Camina hasta la espectacular Montaña Arcoíris a más de 5,000 msnm. Una maravilla natural única.",
                Description = "La Montaña de 7 Colores o Vinicunca es uno de los destinos naturales más sorprendentes del Perú. Una caminata de dificultad moderada-alta te llevará hasta los 5,036 msnm donde podrás contemplar los increíbles tonos minerales de esta montaña.",
                Itinerary = "[{\"time\":\"3:30 AM\",\"title\":\"Recojo del hotel\",\"desc\":\"Salida temprana de Cusco\"},{\"time\":\"6:30 AM\",\"title\":\"Desayuno\",\"desc\":\"Desayuno típico en Cusipata\"},{\"time\":\"7:30 AM\",\"title\":\"Inicio de caminata\",\"desc\":\"Trekking de 2.5 horas hacia la cumbre\"},{\"time\":\"10:00 AM\",\"title\":\"Montaña de 7 Colores\",\"desc\":\"Llegada a la cima, fotos panorámicas\"},{\"time\":\"11:00 AM\",\"title\":\"Descenso\",\"desc\":\"Caminata de retorno\"},{\"time\":\"1:00 PM\",\"title\":\"Almuerzo\",\"desc\":\"Almuerzo buffet en restaurante local\"},{\"time\":\"5:00 PM\",\"title\":\"Retorno a Cusco\",\"desc\":\"Llegada al hotel\"}]",
                Includes = "Transporte ida y vuelta,Desayuno y almuerzo buffet,Guía profesional,Equipo de primeros auxilios,Oxígeno de emergencia,Bastones de trekking",
                Excludes = "Entrada a la montaña (S/. 10),Caballo opcional (S/. 80),Propinas",
                Price = 30,
                PriceType = "per_person",
                Duration = "1 día (14 horas)",
                Difficulty = "hard",
                MaxGroupSize = 15,
                Location = "Cusipata - Vinicunca",
                ImageUrl = "https://images.unsplash.com/photo-1611273426858-450d8e3c9fce?w=800",
                GalleryImages = "[\"https://images.unsplash.com/photo-1544644181-1484b3fdfc62?w=800\",\"https://images.unsplash.com/photo-1509316975850-ff9c5deb0cd9?w=800\",\"https://images.unsplash.com/photo-1589182373726-e4f658ab50f0?w=800\"]",
                Rating = 4.8,
                ReviewCount = 278,
                CategoryId = 2,
                IsFeatured = true,
                SortOrder = 2
            },
            new Tour
            {
                Id = 6,
                Title = "Laguna Humantay",
                Slug = "laguna-humantay",
                ShortDescription = "Descubre la impresionante laguna turquesa al pie del nevado Humantay, un paraíso glaciar andino.",
                Description = "La Laguna Humantay es una joya escondida en los Andes cusqueños. Sus aguas turquesas rodeadas de glaciares te dejarán sin aliento. La caminata moderada de 1.5 horas te lleva a uno de los paisajes más fotogénicos del Perú.",
                Itinerary = "[{\"time\":\"4:00 AM\",\"title\":\"Recojo del hotel\",\"desc\":\"Salida de Cusco\"},{\"time\":\"7:00 AM\",\"title\":\"Desayuno\",\"desc\":\"Desayuno en Mollepata\"},{\"time\":\"8:00 AM\",\"title\":\"Soraypampa\",\"desc\":\"Inicio de caminata (1.5h)\"},{\"time\":\"9:30 AM\",\"title\":\"Laguna Humantay\",\"desc\":\"Tiempo para fotos y contemplación\"},{\"time\":\"11:00 AM\",\"title\":\"Descenso\",\"desc\":\"Retorno a Soraypampa\"},{\"time\":\"1:00 PM\",\"title\":\"Almuerzo\",\"desc\":\"Almuerzo buffet\"},{\"time\":\"5:00 PM\",\"title\":\"Retorno a Cusco\",\"desc\":\"Llegada al hotel\"}]",
                Includes = "Transporte completo,Desayuno y almuerzo,Guía profesional,Oxígeno,Bastones,Primeros auxilios",
                Excludes = "Entrada (S/. 10),Caballo (S/. 100),Propinas",
                Price = 30,
                PriceType = "per_person",
                Duration = "1 día (13 horas)",
                Difficulty = "moderate",
                MaxGroupSize = 15,
                Location = "Mollepata - Laguna Humantay",
                ImageUrl = "https://images.unsplash.com/photo-1611843467160-25afb8df1074?w=800",
                GalleryImages = "[\"https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=800\",\"https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=800\",\"https://images.unsplash.com/photo-1488646953014-85cb44e25828?w=800\"]",
                Rating = 4.9,
                ReviewCount = 203,
                CategoryId = 2,
                IsFeatured = true,
                SortOrder = 3
            },
            new Tour
            {
                Id = 7,
                Title = "Salineras de Maras y Moray",
                Slug = "maras-moray-salineras",
                ShortDescription = "Visita las espectaculares minas de sal incas y el laboratorio agrícola de Moray en medio día.",
                Description = "Un tour fascinante que combina dos sitios únicos: Moray, con sus terrazas circulares que funcionaron como laboratorio agrícola inca, y las Salineras de Maras, miles de pozas de sal que se explotan desde tiempos prehispánicos.",
                Itinerary = "[{\"time\":\"8:00 AM\",\"title\":\"Recojo del hotel\",\"desc\":\"Salida de Cusco\"},{\"time\":\"9:30 AM\",\"title\":\"Moray\",\"desc\":\"Visita al laboratorio agrícola inca\"},{\"time\":\"10:30 AM\",\"title\":\"Salineras de Maras\",\"desc\":\"Recorrido por las minas de sal\"},{\"time\":\"12:00 PM\",\"title\":\"Retorno\",\"desc\":\"Llegada a Cusco\"}]",
                Includes = "Transporte,Guía profesional bilingüe",
                Excludes = "Entradas (S/. 40 combinado),Alimentación,Propinas",
                Price = 25,
                PriceType = "per_person",
                Duration = "Medio día (5 horas)",
                Difficulty = "easy",
                MaxGroupSize = 16,
                Location = "Maras - Moray",
                ImageUrl = "https://images.unsplash.com/photo-1596401057633-54a8fe8ef647?w=800",
                GalleryImages = "[\"https://images.unsplash.com/photo-1589182373726-e4f658ab50f0?w=800\",\"https://images.unsplash.com/photo-1533038590840-1cde6e668a91?w=800\",\"https://images.unsplash.com/photo-1659554282192-9a933b30d73d?w=800\"]",
                Rating = 4.7,
                ReviewCount = 167,
                CategoryId = 2,
                SortOrder = 4
            },
            new Tour
            {
                Id = 8,
                Title = "Salkantay Trek 5D/4N a Machu Picchu",
                Slug = "salkantay-trek-5d-4n",
                ShortDescription = "La alternativa épica al Camino Inca. Cruza el paso del Salkantay a 4,630m y llega caminando a Machu Picchu.",
                Description = "El Salkantay Trek es considerado uno de los 25 mejores trekkings del mundo. Atraviesa paisajes que van desde glaciares hasta selva subtropical. 5 días de aventura pura que culminan con la visita a Machu Picchu.",
                Itinerary = "[{\"time\":\"Día 1\",\"title\":\"Cusco - Soraypampa\",\"desc\":\"Transfer a Mollepata, caminata a campamento base Salkantay\"},{\"time\":\"Día 2\",\"title\":\"Paso Salkantay\",\"desc\":\"Cruce del paso a 4,630m, descenso a Chaullay\"},{\"time\":\"Día 3\",\"title\":\"Selva nublada\",\"desc\":\"Caminata por ceja de selva hasta Santa Teresa\"},{\"time\":\"Día 4\",\"title\":\"Santa Teresa - Aguas Calientes\",\"desc\":\"Caminata por vías del tren, aguas termales\"},{\"time\":\"Día 5\",\"title\":\"Machu Picchu\",\"desc\":\"Tour guiado y retorno a Cusco\"}]",
                Includes = "Guía profesional,Cocinero y porteadores,Equipos de campamento,Alimentación completa (4D),Entrada Machu Picchu,Bus y tren de retorno,Botiquín y oxígeno",
                Excludes = "Saco de dormir (alquiler $15),Bastones (alquiler $10),Propinas,Comidas en Aguas Calientes",
                Price = 450,
                PriceType = "per_person",
                Duration = "5 días / 4 noches",
                Difficulty = "hard",
                MaxGroupSize = 10,
                Location = "Mollepata - Salkantay - Machu Picchu",
                ImageUrl = "https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=800",
                GalleryImages = "[\"https://images.unsplash.com/photo-1611843467160-25afb8df1074?w=800\",\"https://images.unsplash.com/photo-1509316975850-ff9c5deb0cd9?w=800\",\"https://images.unsplash.com/photo-1530789253388-582c481c54b0?w=800\",\"https://images.unsplash.com/photo-1587595431973-160d0d94add1?w=800\"]",
                Rating = 4.9,
                ReviewCount = 134,
                CategoryId = 3,
                IsFeatured = true,
                SortOrder = 1
            },
            new Tour
            {
                Id = 9,
                Title = "Valle Sur del Cusco",
                Slug = "valle-sur-cusco",
                ShortDescription = "Descubre Tipón, Piquillacta y la Capilla Sixtina de América en Andahuaylillas.",
                Description = "El Valle Sur es uno de los secretos mejor guardados de Cusco. Visita los increíbles andenes hidráulicos de Tipón, la ciudad pre-inca de Piquillacta y la majestuosa iglesia de Andahuaylillas, conocida como la 'Capilla Sixtina de América'.",
                Itinerary = "[{\"time\":\"8:00 AM\",\"title\":\"Recojo del hotel\",\"desc\":\"Salida de Cusco\"},{\"time\":\"9:00 AM\",\"title\":\"Tipón\",\"desc\":\"Sistema hidráulico inca\"},{\"time\":\"10:30 AM\",\"title\":\"Piquillacta\",\"desc\":\"Ciudad pre-inca Wari\"},{\"time\":\"12:00 PM\",\"title\":\"Andahuaylillas\",\"desc\":\"Iglesia - Capilla Sixtina de América\"},{\"time\":\"1:30 PM\",\"title\":\"Retorno a Cusco\",\"desc\":\"Fin del tour\"}]",
                Includes = "Transporte turístico,Guía profesional bilingüe",
                Excludes = "Entradas (S/. 40),Alimentación,Propinas",
                Price = 25,
                PriceType = "per_person",
                Duration = "Medio día (5 horas)",
                Difficulty = "easy",
                MaxGroupSize = 16,
                Location = "Tipón - Piquillacta - Andahuaylillas",
                ImageUrl = "https://images.unsplash.com/photo-1586724237569-f3d0c1dee8c6?w=800",
                GalleryImages = "[\"https://images.unsplash.com/photo-1589802829985-817e51171b92?w=800\",\"https://images.unsplash.com/photo-1565008447742-97f6f38c985c?w=800\",\"https://images.unsplash.com/photo-1580619305218-8423a7ef79b4?w=800\"]",
                Rating = 4.5,
                ReviewCount = 89,
                CategoryId = 2,
                SortOrder = 5
            },
            new Tour
            {
                Id = 10,
                Title = "Tour Cusco, Machu Picchu y Lago Titicaca 5D/4N",
                Slug = "cusco-machupicchu-titicaca-5d-4n",
                ShortDescription = "El paquete más completo del sur del Perú: Cusco, Machu Picchu y el majestuoso Lago Titicaca.",
                Description = "Vive lo mejor del sur peruano en 5 días. Desde la ciudad imperial del Cusco hasta las aguas del lago navegable más alto del mundo. Incluye Machu Picchu, Valle Sagrado, islas de Uros y Taquile.",
                Itinerary = "[{\"time\":\"Día 1\",\"title\":\"Llegada a Cusco\",\"desc\":\"Recepción, traslado al hotel y aclimatación\"},{\"time\":\"Día 2\",\"title\":\"Valle Sagrado\",\"desc\":\"Pisac, Ollantaytambo, tren a Aguas Calientes\"},{\"time\":\"Día 3\",\"title\":\"Machu Picchu\",\"desc\":\"Tour guiado, retorno a Cusco\"},{\"time\":\"Día 4\",\"title\":\"Cusco - Puno\",\"desc\":\"Ruta del Sol con visitas: Raqchi, La Raya, Pukara\"},{\"time\":\"Día 5\",\"title\":\"Lago Titicaca\",\"desc\":\"Islas de Uros y Taquile, retorno\"}]",
                Includes = "Transfers completos,Hoteles 3 estrellas,Guías profesionales,Entradas principales,Trenes y buses,Almuerzo día 2 y 4,Lancha Lago Titicaca",
                Excludes = "Vuelos,Alimentación no especificada,Propinas,Extras personales",
                Price = 677,
                PriceType = "per_person",
                Duration = "5 días / 4 noches",
                Difficulty = "easy",
                MaxGroupSize = 12,
                Location = "Cusco - Machu Picchu - Puno",
                ImageUrl = "https://images.unsplash.com/photo-1553550765-41e7dff2bd41?w=800",
                GalleryImages = "[\"https://images.unsplash.com/photo-1620417396507-9a57523d16a6?w=800\",\"https://images.unsplash.com/photo-1534447677768-be436bb09401?w=800\",\"https://images.unsplash.com/photo-1587595431973-160d0d94add1?w=800\"]",
                Rating = 4.8,
                ReviewCount = 98,
                CategoryId = 4,
                IsFeatured = true,
                SortOrder = 1
            }
        );

        // Seed Testimonials
        modelBuilder.Entity<Testimonial>().HasData(
            new Testimonial { Id = 1, ClientName = "María García", Origin = "Madrid, España", Comment = "Una experiencia increíble. El guía fue muy profesional y atento. Machu Picchu superó todas mis expectativas. ¡Totalmente recomendable!", Rating = 5.0, TourId = 1 },
            new Testimonial { Id = 2, ClientName = "James Wilson", Origin = "New York, USA", Comment = "The Salkantay trek was the highlight of our South America trip. Amazing landscapes, great food, and an incredible team. Thank you Inkallajta Tour!", Rating = 5.0, TourId = 8 },
            new Testimonial { Id = 3, ClientName = "Yuri M. Cáceres", Origin = "Lima, Perú", Comment = "Experiencia increíble; atención personalizada súper recomendable. Gracias por hacer de mis días únicos.", Rating = 4.0, TourId = 2 },
            new Testimonial { Id = 4, ClientName = "Sophie Laurent", Origin = "París, Francia", Comment = "Le trek du Salkantay était magnifique! L'équipe était très professionnelle et la nourriture délicieuse. Une aventure inoubliable!", Rating = 5.0, TourId = 8 },
            new Testimonial { Id = 5, ClientName = "Carlos Mendoza", Origin = "Ciudad de México, México", Comment = "El tour al Valle Sagrado y Machu Picchu fue espectacular. Todo muy bien organizado, el guía muy conocedor. Volveré pronto.", Rating = 4.5, TourId = 2 },
            new Testimonial { Id = 6, ClientName = "Emma Schmidt", Origin = "Berlín, Alemania", Comment = "Die Regenbogenberge waren atemberaubend! Die Wanderung war anspruchsvoll aber absolut lohnenswert. Danke an das tolle Team!", Rating = 5.0, TourId = 5 }
        );

        // Seed Admin User (password: Admin123!)
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FullName = "Administrador",
                Email = "admin@inkallajtatour.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = "admin"
            }
        );
    }
}
