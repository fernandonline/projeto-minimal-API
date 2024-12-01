using Microsoft.EntityFrameworkCore;
using minimalAPI.Domain.entities;
namespace minimalAPI.Infrastructure.Db;
public class DbContexto : DbContext
{
    private readonly IConfiguration _configAppSettings;
    public DbContexto(IConfiguration configAppSettings)
    {
        _configAppSettings = configAppSettings;
    }
    public DbSet<Administrador> Administradores {get; set; } = default!;
    public DbSet<Veiculo> Veiculos {get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>().HasData(
            new Administrador {
                Id = 1,
                Email = "admin@admin.com",
                Senha = "admin1",
                Perfil = "adm"
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {  
        if(!optionsBuilder.IsConfigured)
        {
            var StringConexao = _configAppSettings.GetConnectionString("mysql")?.ToString();
            if(!string.IsNullOrEmpty(StringConexao))
            {
                optionsBuilder.UseMySql(StringConexao, ServerVersion.AutoDetect(StringConexao));
            }
        }
    }
}