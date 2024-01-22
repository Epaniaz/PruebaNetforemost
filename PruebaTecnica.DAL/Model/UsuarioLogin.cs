using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.DAL.Model;
public class UsuarioLogin
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string Token { get; set; }
    public DateTime FechaExpiracion { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    public bool Vigente { get; set; } = true;

    [ForeignKey(nameof(UsuarioId))]
    public Usuario Usuario { get; set; }
}
