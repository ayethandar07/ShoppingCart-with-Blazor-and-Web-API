using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopOnline.API.Data;
using ShopOnline.API.Repositories;
using ShopOnline.API.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContextPool<ShopOnlineDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection"))
//);

// Example of removing database provider configuration in Startup.cs
builder.Services.AddDbContext<ShopOnlineDbContext>(options =>
    options.UseInMemoryDatabase("ShopOnlineDB"));


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ShopOnlineDbContext>();
        context.Database.EnsureCreated(); // Ensure the database is created
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(policy =>
    policy.WithOrigins("http://localhost:5258", "https://localhost:5258/")
    .AllowAnyMethod()
    .WithHeaders(HeaderNames.ContentType)
    );

app.UseAuthorization();

app.MapControllers();

app.Run();
