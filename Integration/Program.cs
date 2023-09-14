using Integration.Models;
using Integration.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesRWA.Integration.BLModels;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<RwaMoviesContext>(options => {
    options.UseSqlServer("Name=ConnectionStrings:DefaultConnection");
});

builder.Services.AddAutoMapper(typeof(Integration.Mapping.AutomapperProfile));


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => {
        var jwtKey = builder.Configuration["JWT:Key"];
        var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKey);
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtKeyBytes),
            ValidateLifetime = true,
        };
    });


builder.Services.Configure<ApiBehaviorOptions>(options => {
        options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.Configure<SmptConfig>(builder.Configuration.GetSection("SmtpConfiguration"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
