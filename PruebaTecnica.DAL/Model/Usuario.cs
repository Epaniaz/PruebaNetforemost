using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.DAL.Model;
public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid ExpedienteId { get; set; }
    public string Login { get; set; }
    public string Contrasena { get; set; }
    public string Avatar { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    public DateTime? FechaBaja { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public bool Activo { get; set; } = true;

    [ForeignKey(nameof(ExpedienteId))]
    public ExpedienteUsuario ExpedienteUsuario { get; set; }
    public ICollection<UsuarioLogin> UsuarioLogin { get; set; }
}
