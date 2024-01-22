using Microsoft.EntityFrameworkCore;

using PruebaTecnica.DAL.Model;
using PruebaTecnica.DAL.Interface;

namespace PruebaTecnica.DAL.Service;
public class UsuarioService : IUsuarioService
{
    IEncriptarContrasena _encriptar;
    private readonly PruebaTecnicaDbContext _context;

    public UsuarioService(
        PruebaTecnicaDbContext context
    ) {
        _context = context;
        _encriptar = new EncriptarContrasena();
    }

    public RetornoServicio registrarUsuario(Usuario dataUsuario) 
    {
        try
        {
            if (!_context.Usuarios.Any(u => u.Login == dataUsuario.Login))
            {
                dataUsuario.Contrasena = _encriptar.Encriptar(dataUsuario.Contrasena);

                var expediente = dataUsuario.ExpedienteUsuario;

                _context.ExpedienteUsuario.Add(expediente);
                _context.SaveChanges();

                dataUsuario.ExpedienteId = expediente.Id;
                _context.Usuarios.Add(dataUsuario);
                _context.SaveChanges();

                return obtenerUsuario(dataUsuario.Id);
            }

            return new RetornoServicio
            {
                Retorno = 1,
                Mensaje = $"El usuario {dataUsuario.Login} ya esta registrado"
            };
        }catch (Exception ex) {
            return new RetornoServicio
            {
                Retorno = -1,
                Mensaje = "No se logró registrar el usuario"
            };
        }
    }

    public RetornoServicio autenticarUsuario(string login, string contrasena) 
    {
        try
        {
            var usuario = _context.Usuarios.Where(u => u.Login == login).Include(u => u.ExpedienteUsuario).FirstOrDefault();
            
            if (usuario != null)
            {
                if(_encriptar.Verificar(contrasena, usuario.Contrasena))
                    return new RetornoServicio
                    {
                        Retorno = 0,
                        Mensaje = $"El usuario fue autenticado correctamente {login}",
                        Data = new List<ListadoUsuario> {
                            new ListadoUsuario {
                                Id = usuario.Id,
                                Login = usuario.Login,
                                Avatar = usuario.Avatar,
                                Identificativo = usuario.ExpedienteUsuario.Identificativo,
                                Nombres = usuario.ExpedienteUsuario.Nombres,
                                Apellidos = usuario.ExpedienteUsuario.Apellidos,
                                Correo = usuario.ExpedienteUsuario.Correo,
                                Telefono = usuario.ExpedienteUsuario.Telefono,
                                Direccion = usuario.ExpedienteUsuario.Direccion
                            }
                        }
                    };
            }

            return new RetornoServicio
            {
                Retorno = 1,
                Mensaje = $"Usuario o contraseña invalido"
            };
        }
        catch(Exception ex) 
        {
            return new RetornoServicio
            {
                Retorno = -1,
                Mensaje = "No se logró registrar la autenticación del usuario"
            };
        }
    }

    public RetornoServicioObjeto registrarUsuarioLogin(UsuarioLogin usuarioLogin)
    {
        try
        {
            var logins = _context.UsuarioLogin.Where(ul => ul.UsuarioId == usuarioLogin.UsuarioId && ul.Vigente == true).ToList();

            if(logins.Count() < 0)
                foreach(var login in logins)
                {
                    login.Vigente = false;
                    _context.UsuarioLogin.Update(login);
                }

            _context.UsuarioLogin.Add(usuarioLogin);
            _context.SaveChanges();

            return new RetornoServicioObjeto
            {
                Retorno = 0,
                Mensaje = $"Usuario o contraseña invalido",
                Data = usuarioLogin
            };
        }
        catch (Exception ex)
        {
            return new RetornoServicioObjeto
            {
                Retorno = -1,
                Mensaje = "No se logro autenticar el usuario"
            };
        }
    }

    public RetornoServicioObjeto validarUsuarioLogin(string login, string jwt)
    {
        try
        {
            var usuarioLogin = (from ul in _context.UsuarioLogin
                                join uu in _context.Usuarios on ul.UsuarioId equals uu.Id
                                where uu.Login == login
                                    && ul.Token == jwt
                                    && ul.Vigente == true
                                select new {
                                    uu.Id,
                                    uu.Login,
                                    ul.Token,
                                    ul.FechaExpiracion
                                }).FirstOrDefault();

            if (usuarioLogin == null)
                return new RetornoServicioObjeto
                {
                    Retorno = 1,
                    Mensaje = "No se logró validar el usuario"
                };

            if(usuarioLogin.FechaExpiracion < DateTime.Now)
                return new RetornoServicioObjeto
                {
                    Retorno = 1,
                    Mensaje = "El token ha expirado"
                };

            return new RetornoServicioObjeto
            {
                Retorno = 0,
                Mensaje = "Autenticación correcta",
                Data = usuarioLogin
            };
        }
        catch (Exception ex)
        {
            return new RetornoServicioObjeto
            {
                Retorno = -1,
                Mensaje = "No se logró validar el usuario"
            };
        }
    }

    public RetornoServicio obtenerUsuario(Guid usuarioId)
    {
        try
        {
            var usuarios = (from uu in _context.Usuarios
                            join el in _context.ExpedienteUsuario on uu.ExpedienteId equals el.Id
                            where uu.Id == usuarioId
                            select new ListadoUsuario {
                                Id = uu.Id,
                                Login = uu.Login,
                                Avatar = uu.Avatar,
                                Identificativo = uu.ExpedienteUsuario.Identificativo,
                                Nombres = uu.ExpedienteUsuario.Nombres,
                                Apellidos = uu.ExpedienteUsuario.Apellidos,
                                Correo = uu.ExpedienteUsuario.Correo,
                                Telefono = uu.ExpedienteUsuario.Telefono,
                                Direccion = uu.ExpedienteUsuario.Direccion,
                            }).ToList();

            return new RetornoServicio
            {
                Retorno = 0,
                Mensaje = $"Consulta ejecutada con exito",
                Data = usuarios
            };
        }
        catch (Exception ex)
        {
            return new RetornoServicio
            {
                Retorno = -1,
                Mensaje = "No se logró obtener el listado de usuarios"
            };
        }
    }

    public RetornoServicio obtenerUsuarioLogin()
    {
        try
        {
            var usuarios = (from ul in _context.UsuarioLogin
                            join uu in _context.Usuarios on ul.UsuarioId equals uu.Id
                            join eu in _context.ExpedienteUsuario on uu.ExpedienteId equals eu.Id
                            where ul.Vigente == true
                            select new ListadoUsuario {
                                Id = uu.Id,
                                Login = uu.Login,
                                Avatar = uu.Avatar,
                                Identificativo = eu.Identificativo,
                                Nombres = eu.Nombres,
                                Apellidos = eu.Apellidos,
                                Correo = eu.Correo,
                                Telefono = eu.Telefono,
                                Direccion = eu.Direccion,
                            }).ToList();

            return new RetornoServicio
            {
                Retorno = 0,
                Mensaje = $"Consulta ejecutada con exito",
                Data = usuarios
            };
        }
        catch (Exception ex)
        {
            return new RetornoServicio
            {
                Retorno = -1,
                Mensaje = "No se logró obtener el listado de usuarios"
            };
        }
    }
}
