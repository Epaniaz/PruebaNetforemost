using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using PruebaTecnica.DAL;
using PruebaTecnica.DAL.Service;
using PruebaTecnica.Service.Controllers;
using PruebaTecnica.Service.Hubs;
using PruebaTecnica.Service.Test.Features;

namespace PruebaTecnica.Service.Test.Controllers;
[TestClass]
public class UsuarioControllerTest
{
    [TestMethod]
    public void ObtenerUsuarioConDatos()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();

            var hubContextMock = new Mock<IHubContext<UsuarioHub>>();

            var controller = new UsuarioController(null, dbContext, hubContextMock.Object);

            var resultado = controller.ObtenerUsuario();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado.Retorno, 0);
            Assert.IsTrue(resultado.Data.Count() > 0);
        }
    }

    [TestMethod]
    public void ObtenerUsuarioSinDatos()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            var hubContextMock = new Mock<IHubContext<UsuarioHub>>();

            var controller = new UsuarioController(null, dbContext, hubContextMock.Object);

            var resultado = controller.ObtenerUsuario();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado.Retorno, 0);
            Assert.IsTrue(resultado.Data.Count() == 0);
        }
    }

    [TestMethod]
    public void RegistrarUsuario()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        GlobalConfiguration.Configuration.UseMemoryStorage();

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            var genericData = new GenericData();
            var usuario = genericData.obtenerUsuario();

            var clientMock = new Mock<IHubClients>();
            var clientProxyMock = new Mock<IClientProxy>();
            var hubContextMock = new Mock<IHubContext<UsuarioHub>>();
            var backgroundJobMock = new Mock<IBackgroundJobClient>();
            clientMock.Setup(clients => clients.All).Returns(clientProxyMock.Object);
            hubContextMock.Setup(x => x.Clients).Returns(() => clientMock.Object);

            var controller = new UsuarioController(null, dbContext, hubContextMock.Object);

            var resultado = controller.RegistrarUsuario(usuario);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado.Retorno, 0);
            Assert.AreEqual(resultado.Data.Login, usuario.Usuario);
        }
    }

    [TestMethod]
    public void RegistrarUsuarioDuplicado()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();
            var genericData = new GenericData();
            var usuario = genericData.obtenerUsuario();

            var clientMock = new Mock<IHubClients>();
            var clientProxyMock = new Mock<IClientProxy>();
            var hubContextMock = new Mock<IHubContext<UsuarioHub>>(); 
            clientMock.Setup(clients => clients.All).Returns(clientProxyMock.Object);
            hubContextMock.Setup(x => x.Clients).Returns(() => clientMock.Object);

            var controller = new UsuarioController(null, dbContext, hubContextMock.Object);

            usuario.Usuario = "pcastro";
            var resultado = controller.RegistrarUsuario(usuario);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado.Retorno, 1);
        }
    }
}
