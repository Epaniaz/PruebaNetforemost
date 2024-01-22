using PruebaTecnica.Service.ViewModel;

namespace PruebaTecnica.Service.Test.Features;
public class GenericData
{
    public Configuraciones obtenerConfiguraciones()
    {
        return new Configuraciones { 
            JwtOptions = new JwtOption {
                Issuer = "https://localhost:7004",
                Audience = "https://localhost:7004",
                SigningKey = "some-signing-key-here",
                ExpirationSeconds = 3600,
                KeySecret = "81A98791-FB24-418E-BADB-95576295E1FE"
            } 
        };
    }

    public Usuario obtenerUsuario()
    {
        return new Usuario {
            Usuario = "prflores",
            Contrasena = "123456789",
            Identificativo = "987654321",
            Nombres = "Pedro Antonio",
            Apellidos = "Flores",
            Correo = "pflores@mail.com",
            Telefono = "22336655",
            Direccion = "Nicaragua",
            Avatar = "https://api.api-ninjas.com/v1/randomimage?category=nature"
        };
    }
}
