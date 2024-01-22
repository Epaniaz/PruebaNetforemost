using PruebaTecnica.DAL.Model;

namespace PruebaTecnica.Service.ViewModel;
public class AppLogin : AppResultado
{
    public DataLogin Data { get; set; }
}

public class DataLogin
{
    public string Token { get; set; }
    public DateTime FechaExpiracion { get; set; }
    public ListadoUsuario Usuario { get; set; }
}