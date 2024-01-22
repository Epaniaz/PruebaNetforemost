using PruebaTecnica.DAL.Model;

namespace PruebaTecnica.Service.ViewModel;
public class AppUsuario : AppResultado
{
    public IList<ListadoUsuario> Data { get; set; }
}
