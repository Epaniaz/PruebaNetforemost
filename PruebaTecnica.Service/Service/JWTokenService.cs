using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using PruebaTecnica.DAL.Model;

using PruebaTecnica.Service.Interface;
using PruebaTecnica.Service.ViewModel;

namespace PruebaTecnica.Service.Service;
public class JWTokenService : IJWTokenService
{
    private readonly Configuraciones _appSetting;

    public JWTokenService(IOptions<Configuraciones> options)
    { 
        _appSetting = options.Value;
    }

    public string generarJwtToken(ListadoUsuario dataUsuario, DateTime fechaExpiracion)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSetting.JwtOptions.KeySecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {

            Expires = fechaExpiracion,
            Subject = new ClaimsIdentity(new[] {
                    new Claim("LoginId", dataUsuario.Login),
            }),
            Issuer = _appSetting.JwtOptions.Issuer,
            Audience = _appSetting.JwtOptions.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
