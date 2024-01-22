using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

using PruebaTecnica.Service.Hubs;
using PruebaTecnica.Service.ViewModel;
using PruebaTecnica.Service.Middleware;

using PruebaTecnica.DAL;
using PruebaTecnica.DAL.Model;
using PruebaTecnica.DAL.Service;
using PruebaTecnica.DAL.Interface;
using Hangfire;
using PruebaTecnica.Service.Interface;
using PruebaTecnica.Service.Service;

namespace PruebaTecnica.Service.Controllers
{
    [ApiController]
    [Route("api/v1/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly ICorreoService _correoService;
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;
        private readonly IHubContext<UsuarioHub> _hubContext;

        public UsuarioController(
            ILogger<UsuarioController> logger,
            PruebaTecnicaDbContext dbContext,
            IHubContext<UsuarioHub> hubContext
        )
        {
            _logger = logger;
            _hubContext = hubContext;
            _correoService = new CorreoService();
            _usuarioService = new UsuarioService(dbContext);
        }

        [HttpGet("vigente"), Authorize]
        public AppUsuario ObtenerUsuario()
        {
            var listado = _usuarioService.obtenerUsuarioLogin();

            return new AppUsuario
            {
                Retorno = listado.Retorno,
                Mensaje = listado.Mensaje,
                Data = listado.Data
            };
        }

        [HttpPost("registrar"), AllowAnonymous]
        [ServiceFilter(typeof(ValidarFormularioActionFilter))]
        public AppRegistrarUsuario RegistrarUsuario(ViewModel.Usuario usuario)
        {
            var obj = new ListadoUsuario();
            var datos = new DAL.Model.Usuario {
                Login = usuario.Usuario,
                Contrasena = usuario.Contrasena,
                Avatar = usuario.Avatar,
                ExpedienteUsuario = new ExpedienteUsuario
                {
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    Correo = usuario.Correo,
                    Telefono = usuario.Telefono,
                    Direccion = usuario.Direccion,
                    Identificativo = usuario.Identificativo
                }
            };
            var resultado = _usuarioService.registrarUsuario(datos);

            if (resultado.Retorno == 0)
            { 
                obj = resultado.Data.FirstOrDefault();

                _hubContext.Clients.All.SendAsync("RecibeUsuario", obj);
                BackgroundJob.Enqueue(() => _correoService.EnvioCorreo(obj.Correo, "Advertencia de inicio de sesión", "Se informa de una nuevo ingreso en su cuenta"));
            }

            return new AppRegistrarUsuario { 
                Retorno = resultado.Retorno, 
                Mensaje = resultado.Mensaje,
                Data = obj
            };
        }
    }
}