using HLTV_API.Application.Interfaces;
using HLTV_API.Application.Repositories;
using HLTV_API.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");
string connString = builder.Configuration.GetConnectionString("Hltv")!;
builder.Services.AddDbContextPool<HltvContext>(options =>
    options.UseLazyLoadingProxies()
           .UseMySql(connString, ServerVersion.AutoDetect(connString)));

builder.Services.AddScoped<Webscraper>();
builder.Services.AddScoped<IMatchesRepo, MatchesRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
