using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

using PruebaTecnica.DAL;
using PruebaTecnica.DAL.Model;
using PruebaTecnica.DAL.Service;
using PruebaTecnica.DAL.Interface;

using PruebaTecnica.Service.Service;
using PruebaTecnica.Service.ViewModel;
using PruebaTecnica.Service.Middleware;
using PruebaTecnica.Service.Interface;

namespace PruebaTecnica.Service.Controllers;
[Route("api/v1/autenticacion")]
[ApiController]
public class SeguridadController : ControllerBase
{
    private readonly ICorreoService _correoService;
    private readonly IJWTokenService _jwtService;
    private readonly IUsuarioService _usuarioService;
    private readonly ILogger<SeguridadController> _logger;

    public SeguridadController(
        ILogger<SeguridadController> logger,
        PruebaTecnicaDbContext dbContext,
        IJWTokenService jwtService
    )
    {
        _logger = logger;
        _jwtService = jwtService;
        _correoService = new CorreoService();
        _usuarioService = new UsuarioService(dbContext);
    }

    [HttpPost, AllowAnonymous, Route("login")]
    [ServiceFilter(typeof(ValidarFormularioActionFilter))]
    public AppLogin Login([FromBody] Login usuario)
    {
        string token = string.Empty;
        ListadoUsuario dataUsuario = new ListadoUsuario();
        DateTime fechaExpiracion = DateTime.UtcNow.AddMinutes(60);
        var result = _usuarioService.autenticarUsuario(usuario.Usuario, usuario.Contrasena);

        if (result.Retorno == 0)
        {
            dataUsuario = result.Data.FirstOrDefault();
            token = _jwtService.generarJwtToken(dataUsuario, fechaExpiracion);
            var obj = new UsuarioLogin
            {
                Token = token,
                UsuarioId = dataUsuario.Id,
                FechaExpiracion = fechaExpiracion,
                FechaRegistro = DateTime.Now,
                Vigente = true
            };

            var actualizarToken = _usuarioService.registrarUsuarioLogin(obj);

            if (actualizarToken.Retorno != 0)
                return new AppLogin
                {
                    Retorno = actualizarToken.Retorno,
                    Mensaje = actualizarToken.Mensaje,
                    Data = null
                };

            BackgroundJob.Enqueue(() => _correoService.EnvioCorreo(dataUsuario.Correo, "Advertencia de inicio de sesión", "Se informa de una nuevo ingreso en su cuenta"));
        }

        return new AppLogin
        {
            Retorno = result.Retorno,
            Mensaje = result.Mensaje,
            Data = new DataLogin
            {
                Token = token,
                FechaExpiracion= fechaExpiracion,
                Usuario = dataUsuario,
            }
        };
    }
}
