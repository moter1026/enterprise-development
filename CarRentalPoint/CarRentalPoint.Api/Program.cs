using CarRentalPoint.Application.Mappers;
using CarRentalPoint.Domain.Contract;
using CarRentalPoint.Domain.Entities;
using CarRentalPoint.Domain.Interfaces;
using CarRentalPoint.Infrastructure.Persistence;
using CarRentalPoint.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<AppDbContext>(connectionName: "DefaultConnection");

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddScoped<IRepository<CarModel>, EfRepository<CarModel>>();
builder.Services.AddScoped<IRepository<ModelGeneration>, EfRepository<ModelGeneration>>();
builder.Services.AddScoped<IRepository<Car>, EfRepository<Car>>();
builder.Services.AddScoped<IRepository<Client>, EfRepository<Client>>();
builder.Services.AddScoped<IRepository<Rental>, EfRepository<Rental>>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await DbSeeder.SeedAllAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
