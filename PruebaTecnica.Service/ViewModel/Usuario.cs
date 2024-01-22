using PruebaTecnica.DAL.Model;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Service.ViewModel;
public class Usuario : Login
{
    public string? Avatar { get; set; }
    [Required]
    [MaxLength(30)]
    public string Identificativo { get; set; }
    [Required]
    [MaxLength(255)]
    public string Nombres { get; set; }
    [Required]
    [MaxLength(255)]
    public string Apellidos { get; set; }
    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Correo { get; set; }
    [Required]
    [MaxLength(20)]
    public string Telefono { get; set; }
    [Required]
    [MaxLength(1000)]
    public string? Direccion { get; set; }
}
