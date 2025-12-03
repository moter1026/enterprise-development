using CarRentalPoint.Api.Grpc;
using CarRentalPoint.Api.Grpc.Services;
using CarRentalPoint.Application.Mappers;
using CarRentalPoint.Domain.Contract;
using CarRentalPoint.Domain.Entities;
using CarRentalPoint.Domain.Interfaces;
using CarRentalPoint.Infrastructure.Persistence;
using CarRentalPoint.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RentalReceiverOptions>(builder.Configuration.GetSection("RentalReceiver"));
builder.Configuration.AddEnvironmentVariables();

var rpcOptions = builder.Configuration.GetSection("RentalReceiver").Get<RentalReceiverOptions>();

builder.Services.AddGrpc(options =>
{
    options.MaxReceiveMessageSize = rpcOptions?.MaxReceiveMessageSizeBytes;
    options.MaxSendMessageSize = rpcOptions?.MaxSendMessageSizeBytes;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

builder.Services.AddScoped<IRepository<Car>, EfRepository<Car>>();
builder.Services.AddScoped<IRepository<Client>, EfRepository<Client>>();
builder.Services.AddScoped<IRepository<Rental>, EfRepository<Rental>>();

builder.Services.AddScoped<RentalReceiverGrpcService>();

var app = builder.Build();

app.MapGrpcService<RentalReceiverGrpcService>();
app.MapGet("/", () => "gRPC service running");

app.Run();
