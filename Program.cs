using HLTV_API.Application.Interfaces;
using HLTV_API.Application.Repositories;
using HLTV_API.Infrastructure;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");
string connString = builder.Configuration.GetConnectionString("Hltv")!;
builder.Services.AddDbContextPool<HltvContext>(options =>
    options.UseLazyLoadingProxies()
           .UseMySql(connString, ServerVersion.AutoDetect(connString)));

// DependencyInjection
builder.Services.AddScoped<Webscraper>();
builder.Services.AddScoped<IMatchesRepo, MatchesRepo>();
builder.Services.AddScoped<IGuildsRepo, GuildsRepo>();
builder.Services.AddScoped<ILiveMatchAlertRepo, LiveMatchAlertRepo>();

// TODO: REMOVE after development
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAny",
    builder => builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

// Certificate stuff
builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Certificate stuff
app.UseAuthentication();

app.MapControllers();

app.Run();
