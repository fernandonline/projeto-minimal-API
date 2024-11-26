using System.Data.Common;
using Domain.DTOs;
using minimalAPI.Domain.entities;
using minimalAPI.Domain.Interfaces;
using minimalAPI.Infrastructure.Db;

namespace minimalAPI.Domain.Services;

public class AdministradorServico : IAdministradorServico
{

    private readonly DbContexto _contexto;

    public AdministradorServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public Administrador? Login(LoginDTO loginDTO)
    {
        var adm = _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
        return adm;
    }
}