using Moq;
using Microsoft.Extensions.Options;

using PruebaTecnica.Service.Service;
using PruebaTecnica.Service.ViewModel;
using PruebaTecnica.Service.Test.Features;

namespace PruebaTecnica.Service.Test.Service;
[TestClass]
public class JWTokenServiceTest
{
    [TestMethod]
    public void generarJwtTokenCorrecto()
    {
        var genericData = new GenericData();
        var configuraciones = genericData.obtenerConfiguraciones();

        var optionsMock = new Mock<IOptions<Configuraciones>>();
        optionsMock.Setup(x => x.Value).Returns(configuraciones);

        var jwTokenService = new JWTokenService(optionsMock.Object);

        var resultado = jwTokenService.generarJwtToken(new DAL.Model.ListadoUsuario { Login = "pcastro" }, DateTime.Now.AddDays(1));

        Assert.IsNotNull(resultado);
    }
}
