using Domain.DTOs;
using minimalAPI.Domain.entities;
namespace minimalAPI.Domain.Interfaces;

public interface IAdministradorServico
{
    Administrador? Login(LoginDTO loginDTO);
}