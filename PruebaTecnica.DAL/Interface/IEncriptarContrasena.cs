namespace PruebaTecnica.DAL.Interface;
public interface IEncriptarContrasena
{
    string Encriptar(string contrasena);
    bool Verificar(string contrasena, string constrasenaHash);
}
