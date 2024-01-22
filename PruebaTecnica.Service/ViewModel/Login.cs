using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Service.ViewModel;
public class Login
{
    [Required]
    [MinLength(5), MaxLength(12)]
    public string Usuario { get; set; }
    [Required]
    [MaxLength(12), MinLength(8)]
    public string Contrasena { get; set; }
}
