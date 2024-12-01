using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace minimalAPI.Domain.entities;

public class Veiculo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = default!;

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = default!;
    
    [Required]
    [StringLength(50)]
    public string Marca { get; set; } = default!;

    [Required]
    public int Ano { get; set; } = default!;

}