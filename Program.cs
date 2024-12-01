using Microsoft.EntityFrameworkCore;
using minimalAPI.Infrastructure.Db;
using Domain.DTOs;
using minimalAPI.Domain.Services;
using minimalAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using minimalAPI.Domain.ModelViews;
using minimalAPI.Domain.entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();

app.MapGet("/", () => Results.Json(new Home()));

app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) => {
    if(administradorServico.Login(loginDTO) != null)
        return Results.Ok("logado com sucesso.");
    else
        return Results.Unauthorized();
});

app.MapPost("/veiculos", ([FromBody] Veiculo veiculoDTO, IVeiculoServico veiculoServico) =>{
    var veiculo = new Veiculo {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano,
    };
    veiculoServico.Incluir(veiculo);
    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
});

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculoServico veiculoServico) =>{
    var veiculos = veiculoServico.Todos(pagina);
    return Results.Ok(veiculos);
});

app.UseSwagger();
app.UseSwaggerUI();
app.Run();