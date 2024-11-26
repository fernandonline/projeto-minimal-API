using Microsoft.EntityFrameworkCore;
using minimalAPI.Infrastructure.Db;
using Domain.DTOs;
using minimalAPI.Domain.Services;
using minimalAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();

builder.Services.AddDbContext<DbContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();
app.MapGet("/", () => "Olá Mundo!");
app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) => {
    if(administradorServico.Login(loginDTO) != null)
        return Results.Ok("logado com sucesso.");
    else
        return Results.Unauthorized();
});
app.Run();