var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Olá Mundo!");

app.MapPost("/login", (Domain.DTOs.LoginDTO loginDTO) => {
    if(loginDTO.Email == "admin@admin.com" && loginDTO.Senha == "admin")
        return Results.Ok("logado com sucesso.");
    else
        return Results.Unauthorized();
});

app.Run();