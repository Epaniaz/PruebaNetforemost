using PruebaTecnica.DAL.Model;

namespace PruebaTecnica.Service.Interface;
public interface IJWTokenService
{
    string generarJwtToken(ListadoUsuario dataUsuario, DateTime fechaExpiracion);
}
