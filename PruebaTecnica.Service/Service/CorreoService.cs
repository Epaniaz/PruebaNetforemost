using System.Net;
using System.Net.Mail;

using PruebaTecnica.Service.Interface;

namespace PruebaTecnica.Service.Service;
public class CorreoService : ICorreoService
{
    public void EnvioCorreo(string para, string asunto, string correo)
    {
        var correoEnvio = new MailAddress("ost.desk2019@gmail.com", "Prueba tecnica");
        var correoPara = new MailAddress(para, "prueba");
        string password = "alexander?2012";

        var smtp = new SmtpClient { 
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(correoEnvio.Address, password)
        };

        using (var mensaje = new MailMessage(correoEnvio, correoPara) { Subject = asunto, Body = correo })
        {
            smtp.Send(mensaje);
        }
    }
}
