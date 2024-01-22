using PruebaTecnica.DAL.Model;

namespace PruebaTecnica.DAL.Interface;
public interface IUsuarioService
{
    RetornoServicio obtenerUsuario(Guid usuarioId);
    RetornoServicio obtenerUsuarioLogin();
    RetornoServicio registrarUsuario(Usuario dataUsuario);
    RetornoServicioObjeto validarUsuarioLogin(string login, string jwt);
    RetornoServicioObjeto registrarUsuarioLogin(UsuarioLogin usuarioLogin);
    RetornoServicio autenticarUsuario(string login, string contrasena);
}
