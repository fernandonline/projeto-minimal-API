using Microsoft.EntityFrameworkCore;
using minimalAPI.Infrastructure.Db;
using Domain.DTOs;
using minimalAPI.DTOs;
using minimalAPI.Domain.Services;
using minimalAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using minimalAPI.Domain.ModelViews;
using minimalAPI.Domain.entities;
using minimalAPI.Domain.Enuns;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administradores
app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) => {
    if(administradorServico.Login(loginDTO) != null)
        return Results.Ok("logado com sucesso.");
    else
        return Results.Unauthorized();
}).WithTags("Administradores");

app.MapPost("/administradores", ([FromQuery] int? pagina, IAdministradorServico administradorServico) => {
    return Results.Ok(administradorServico.Todos(pagina));
}).WithTags("Administradores");

app.MapGet("/administradores/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) => {
    var administrador = administradorServico.BuscaPorId(id);
    if(administrador == null) return Results.NotFound();

    return Results.Ok(administrador);
}).WithTags("Administradores");

app.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) => {
    var validacao = new ErroDeValidacao{
        Mensagens = new List<string>()
    };

    if(string.IsNullOrEmpty(administradorDTO.Email))
        validacao.Mensagens.Add("O Email não deve estar vazio");
    if(string.IsNullOrEmpty(administradorDTO.Senha))
        validacao.Mensagens.Add("A Senha não deve estar em branco");
    if(administradorDTO.Perfil == null)
        validacao.Mensagens.Add("O Perfil não deve ser vazio");
    if(validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    var administrador = new Administrador{
        Email = administradorDTO.Email,
        Senha = administradorDTO.Senha,
        Perfil = administradorDTO.Perfil.ToString() ?? Perfil.editor.ToString(),
    };
    administradorServico.Incluir(administrador);

    return Results.Created($"/administrador/{administrador.Id}", administrador);
}).WithTags("Administradores");
#endregion

static ErroDeValidacao validaDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ErroDeValidacao{
        Mensagens = []
    };

    if(string.IsNullOrEmpty(veiculoDTO.Nome))
        validacao.Mensagens.Add("O Nome não deve estar vazio");

    if(string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagens.Add("A Marca não deve estar em branco");

    if(veiculoDTO.Ano < 1950)
        validacao.Mensagens.Add("Veiculo muito antigo");

    return validacao;
}

app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>{

    var validacao = validaDTO(veiculoDTO);
    if(validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);
    
    var veiculo = new Veiculo {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano,
    };
    veiculoServico.Incluir(veiculo);
    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).WithTags("veiculos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculoServico veiculoServico) =>{
    var veiculos = veiculoServico.Todos(pagina);
    return Results.Ok(veiculos);
}).WithTags("veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoServico veiculoServico) =>{
    var veiculo = veiculoServico.BuscaPorId(id);
    if(veiculo == null) return Results.NotFound();

    return Results.Ok(veiculo);
}).WithTags("veiculos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>{
    
    var validacao = validaDTO(veiculoDTO);
    if(validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);
    
    var veiculo = veiculoServico.BuscaPorId(id);
    if(veiculo == null) return Results.NotFound();

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    veiculoServico.Atualizar(veiculo);
    return Results.Ok(veiculo);
}).WithTags("veiculos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculoServico veiculoServico) =>{
    var veiculo = veiculoServico.BuscaPorId(id);
    if(veiculo == null) return Results.NotFound();

    veiculoServico.Apagar(veiculo);
    return Results.NoContent();
}).WithTags("veiculos");

app.UseSwagger();
app.UseSwaggerUI();
app.Run();