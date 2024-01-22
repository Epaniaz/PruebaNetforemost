using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using PruebaTecnica.DAL;
using PruebaTecnica.Service.Controllers;
using PruebaTecnica.Service.Service;
using PruebaTecnica.Service.Test.Features;
using PruebaTecnica.Service.ViewModel;

namespace PruebaTecnica.Service.Test.Controllers;
[TestClass]
public class SeguridadControllerTest
{
    [TestMethod]
    public void LoginCorrecto()
    {
        GlobalConfiguration.Configuration.UseMemoryStorage();

        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        var genericData = new GenericData();
        var configuraciones = genericData.obtenerConfiguraciones();

        var optionsMock = new Mock<IOptions<Configuraciones>>();
        var backgroundJobMock = new Mock<IBackgroundJobClient>();
        optionsMock.Setup(x => x.Value).Returns(configuraciones);

        var jwTokenService = new JWTokenService(optionsMock.Object);

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();

            var controller = new SeguridadController(null, dbContext, jwTokenService);

            var login = new Login { Usuario = "pcastro", Contrasena = "123456789" };
            var resultado = controller.Login(login);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado.Retorno, 0);
            Assert.IsNotNull(resultado.Data.Token);
            Assert.AreEqual(resultado.Data.Usuario.Login, login.Usuario);
        }
    }

    [TestMethod]
    public void LoginInCorrecto()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        var genericData = new GenericData();
        var configuraciones = genericData.obtenerConfiguraciones();

        var optionsMock = new Mock<IOptions<Configuraciones>>();
        optionsMock.Setup(x => x.Value).Returns(configuraciones);

        var jwTokenService = new JWTokenService(optionsMock.Object);

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();

            var controller = new SeguridadController(null, dbContext, jwTokenService);

            var login = new Login { Usuario = "pcastro", Contrasena = "12345678" };
            var resultado = controller.Login(login);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado.Retorno, 1);
            Assert.IsTrue(string.IsNullOrWhiteSpace(resultado.Data.Token));
        }
    }
}
