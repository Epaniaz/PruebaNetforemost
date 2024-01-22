using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.DAL.Model;

public class ExpedienteUsuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Identificativo { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string Direccion { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public bool Activo { get; set; } = true;

    public ICollection<Usuario> Usuarios { get; set; }
}
