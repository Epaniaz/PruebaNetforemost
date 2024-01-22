namespace PruebaTecnica.DAL.Model;
public class ListadoUsuario
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Avatar { get; set; }
    public string Identificativo { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string Direccion { get; set; }
}