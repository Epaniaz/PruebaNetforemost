using PruebaTecnica.Service.Service;

namespace PruebaTecnica.Service.Test.Service;

[TestClass]
public class CorreoServiceTest
{
    [TestMethod]
    public void EnvioCorreo()
    {
        var correoService = new CorreoService();

        try
        {
            correoService.EnvioCorreo("correo@mail.com", "Testing", "probando los testing");

            Assert.IsTrue(true);
        }catch(Exception ex) 
        {
            Assert.IsTrue(false);
            Assert.Fail(ex.Message);
        }
    }
}