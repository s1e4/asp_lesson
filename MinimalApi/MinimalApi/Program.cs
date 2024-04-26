using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MinimalApi.Auth;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITokenService>(new TokenService());
builder.Services.AddSingleton<IUserRepository>(new UserRepository());
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/login", [AllowAnonymous] async (HttpContext context,
    ITokenService tokenService, IUserRepository userRepository) => {
        UserDto userModel = new()
        {
            UserName = context.Request.Query["username"],
            Password = context.Request.Query["password"]
        };
        var userDto = userRepository.GetUser(userModel);
        if (userDto == null) return Results.Unauthorized();
        var token = tokenService.BuildToken(builder.Configuration["Jwt:Key"],
            builder.Configuration["Jwt:Issuer"], userDto);
        return Results.Ok(token);
    });

//app.UseHttpsRedirection();
var hotels = new List<Hotel>();

app.MapGet("/hotels", [Authorize] () => hotels);
app.MapGet("/hotels/{id}", [Authorize] (int id) => hotels.FirstOrDefault(h => h.Id == id));
app.MapPost("/hotels", [Authorize] (Hotel hotel) => hotels.Add(hotel));
app.MapPut("/hotels", [Authorize] (Hotel hotel) => {
    var index = hotels.FindIndex(h => h.Id == hotel.Id);
    if (index < 0)
    {
        throw new Exception("Not found");
    }
    hotels[index] = hotel;
});
app.MapDelete("/hotels/{id}", [Authorize] (int id) => {
    var index = hotels.FindIndex(h => h.Id == id);
    if (index < 0) throw new Exception("Not found");
    hotels.RemoveAt(index);
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}