using Microsoft.EntityFrameworkCore;

using PruebaTecnica.DAL.Model;
using PruebaTecnica.DAL.Service;

namespace PruebaTecnica.DAL.Test.Service;

[TestClass]
public class UsuarioServiceTest
{


    [TestMethod]
    public void resgistrarUsuarioCorrecto()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            var usuarioServicio = new UsuarioService(dbContext);
            var featureExpediente = dbContext.expedienteData();
            var featureUsuario = dbContext.usuarioData(featureExpediente);

            featureUsuario.ExpedienteUsuario = featureExpediente;
            var respuesta = usuarioServicio.registrarUsuario(featureUsuario);

            Assert.IsNotNull(respuesta);
            Assert.AreEqual(respuesta.Retorno, 0);
            Assert.AreEqual(respuesta.Data.Count(), 1);

            var respuestaObjeto = respuesta.Data.FirstOrDefault();

            Assert.AreEqual(featureUsuario.Id, respuestaObjeto.Id);
        }
    }

    [TestMethod]
    public void registrarUsuarioError()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            var usuarioServicio = new UsuarioService(dbContext);

            var respuesta = usuarioServicio.registrarUsuario(new Usuario());

            Assert.IsNotNull(respuesta);
            Assert.IsNull(respuesta.Data);
            Assert.AreEqual(respuesta.Retorno, -1);
        }
    }

    [TestMethod]
    public void resgistrarUsuarioDuplicado()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();
            var usuarioServicio = new UsuarioService(dbContext);
            var featureExpediente = dbContext.expedienteData();
            var featureUsuario = dbContext.usuarioData(featureExpediente);

            featureUsuario.Login = "pcastro";
            featureUsuario.ExpedienteUsuario = featureExpediente;
            var respuesta = usuarioServicio.registrarUsuario(featureUsuario);

            Assert.IsNotNull(respuesta);
            Assert.IsNull(respuesta.Data);
            Assert.AreEqual(respuesta.Retorno, 1);
        }
    }

    [TestMethod]
    public void autenticarUsuarioCorrecto()
    {
        var login = "pcastro";
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();
            var usuarioServicio = new UsuarioService(dbContext);

            var respuesta = usuarioServicio.autenticarUsuario(login, "123456789");

            Assert.IsNotNull(respuesta);
            Assert.AreEqual(respuesta.Retorno, 0);
            Assert.AreEqual(respuesta.Data.FirstOrDefault().Login, login);
        }
    }

    [TestMethod]
    public void autenticarUsuarioIncorrecto()
    {
        var login = "pcastro";
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();
            var usuarioServicio = new UsuarioService(dbContext);

            var respuesta = usuarioServicio.autenticarUsuario(login, "12345678");

            Assert.IsNotNull(respuesta);
            Assert.IsNull(respuesta.Data);
            Assert.AreEqual(respuesta.Retorno, 1);
        }
    }

    [TestMethod]
    public void registrarUsuarioLoginCorrecto()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();
            var usuarioServicio = new UsuarioService(dbContext);
            var usuario = dbContext.Usuarios.Where(u => u.Login == "pcastro").FirstOrDefault();
            var usuarioLogin = dbContext.usuarioLoginData(usuario.Id);

            var respuesta = usuarioServicio.registrarUsuarioLogin(usuarioLogin);

            Assert.IsNotNull(respuesta);
            Assert.IsNotNull(respuesta.Data);
            Assert.AreEqual(respuesta.Retorno, 0);
        }
    }

    [TestMethod]
    public void registrarUsuarioLoginInCorrecto()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();
            var usuarioServicio = new UsuarioService(dbContext);

            var respuesta = usuarioServicio.registrarUsuarioLogin(new UsuarioLogin());

            Assert.IsNotNull(respuesta);
            Assert.IsNull(respuesta.Data);
            Assert.AreEqual(respuesta.Retorno, -1);
        }
    }

    [TestMethod]
    public void validarUsuarioLoginCorrecto()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();
            var usuarioLogin = dbContext.usuarioLoginData(Guid.NewGuid());
            var usuarioServicio = new UsuarioService(dbContext);

            var respuesta = usuarioServicio.validarUsuarioLogin("pcastro", usuarioLogin.Token);

            Assert.IsNotNull(respuesta);
            Assert.IsNotNull(respuesta.Data);
            Assert.AreEqual(respuesta.Retorno, 0);
        }
    }

    [TestMethod]
    public void validarUsuarioLoginInCorrecto()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();
            var usuarioLogin = dbContext.usuarioLoginData(Guid.NewGuid());
            var usuarioServicio = new UsuarioService(dbContext);

            var respuesta = usuarioServicio.validarUsuarioLogin("castro", usuarioLogin.Token);

            Assert.IsNotNull(respuesta);
            Assert.IsNull(respuesta.Data);
            Assert.AreEqual(respuesta.Retorno, 1);
        }
    }

    [TestMethod]
    public void obtenerUsuarioCorrecto()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();
            var usuarioServicio = new UsuarioService(dbContext);
            var usuarioData = dbContext.Usuarios.FirstOrDefault();

            var respuesta = usuarioServicio.obtenerUsuario(usuarioData.Id);

            Assert.IsNotNull(respuesta);
            Assert.AreEqual(respuesta.Retorno, 0);
            Assert.AreEqual(usuarioData.Id, respuesta.Data.FirstOrDefault().Id);
        }
    }

    [TestMethod]
    public void obtenerUsuarioCorrectoIncorrecto()
    {
        var options = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        using (var dbContext = new PruebaTecnicaDbContext(options))
        {
            dbContext.Database.EnsureCreated();
            var usuarioServicio = new UsuarioService(dbContext);

            var respuesta = usuarioServicio.obtenerUsuarioLogin();

            Assert.IsNotNull(respuesta);
            Assert.AreEqual(respuesta.Retorno, 0);
            Assert.AreEqual(respuesta.Data.Count(), 1);
        }
    }
}