using Microsoft.EntityFrameworkCore;
using MoviesRWA.BL.DALModels;
using MoviesRWA.BL.Mapping;
using MoviesRWA.BL.Repositories;
using MoviesRWA.WEB.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<RwaMoviesContext>(options => {
    options.UseSqlServer("Name=ConnectionStrings:DefaultConnection"); 
});

builder.Services.AddAutoMapper(
    typeof(MoviesRWA.WEB.Mapping.AutomapperProfile),
    typeof(MoviesRWA.BL.Mapping.AutomapperProfile));

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
