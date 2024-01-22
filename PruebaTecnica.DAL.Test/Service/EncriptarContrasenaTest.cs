using PruebaTecnica.DAL.Service;

namespace PruebaTecnica.DAL.Test.Service;
[TestClass]
public class EncriptarContrasenaTest
{
    [TestMethod]
    public void Encriptar()
    {
        var encriptarService = new EncriptarContrasena();
        var contrasena = encriptarService.Encriptar("123456789");

        Assert.IsNotNull(contrasena);
        Assert.IsTrue(encriptarService.Verificar("123456789", contrasena));
    }

    [TestMethod]
    public void EncriptarValidarTrue()
    {
        var encriptarService = new EncriptarContrasena();
        var contrasena = encriptarService.Verificar("123456789", "6AAD8A4AA69C21AD21900479BA9CC004C047DBC8D5FD66254B6F69DAB192C0DF:F41BB52FCF8BCDA9B9F2BBC2B5C027C3:50000:SHA256");

        Assert.IsTrue(contrasena);
    }

    [TestMethod]
    public void EncriptarValidarFalse()
    {
        var encriptarService = new EncriptarContrasena();
        var contrasena = encriptarService.Verificar("1234567", "6AAD8A4AA69C21AD21900479BA9CC004C047DBC8D5FD66254B6F69DAB192C0DF:F41BB52FCF8BCDA9B9F2BBC2B5C027C3:50000:SHA256");

        Assert.IsFalse(contrasena);
    }
}
