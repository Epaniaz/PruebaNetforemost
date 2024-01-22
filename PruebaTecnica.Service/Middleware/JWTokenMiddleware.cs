using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using PruebaTecnica.DAL.Service;
using PruebaTecnica.DAL.Interface;

using PruebaTecnica.Service.ViewModel;
using PruebaTecnica.DAL;

namespace PruebaTecnica.Service.Middleware;
public class JWTokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Configuraciones _appSetting;
    private IUsuarioService _usuarioService;

    public JWTokenMiddleware(
        RequestDelegate next,
        IOptions<Configuraciones> appSetting
    )
    {
        _next = next;
        _appSetting = appSetting.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        var resultado = new AppLogin { Retorno = 1, Mensaje = "Usuario no autorizado" };
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            resultado = validarTokenUsuarioContext(context, token);

            if (resultado.Retorno != 0)
            {
                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(JsonSerializer.Serialize(resultado));

                return;
            }
        }

        await _next(context);
    }

    private AppLogin validarTokenUsuarioContext(HttpContext context, string token)
    {
        try
        {
            var dbContext = context.RequestServices.GetRequiredService<PruebaTecnicaDbContext>();
            _usuarioService = new UsuarioService(dbContext);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSetting.JwtOptions.KeySecret);
            
            tokenHandler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var usuarioId = (string)(jwtToken.Claims.First(x => x.Type == "LoginId").Value);

            context.Request.Headers.Add("UsuarioId", usuarioId);
            
            var user = _usuarioService.validarUsuarioLogin(usuarioId, token);

            return new AppLogin {
                Retorno = user.Retorno,
                Mensaje = user.Mensaje
            };
        }
        catch (Exception ex)
        {
            return new AppLogin {
                Retorno = -1,
                Mensaje = "Ocurrio un error en la autenticación"
            };
        }
    }
}
