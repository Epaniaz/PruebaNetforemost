using Microsoft.AspNetCore.SignalR;

using PruebaTecnica.DAL;
using PruebaTecnica.DAL.Service;
using PruebaTecnica.DAL.Interface;

namespace PruebaTecnica.Service.Hubs;
public class UsuarioHub : Hub
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioHub(PruebaTecnicaDbContext context)
    {
        _usuarioService = new UsuarioService(context);
    }

    public async Task UsuarioConectado()
    {
         var usuarios = _usuarioService.obtenerUsuarioLogin();

        await Clients.All.SendAsync("RecibeUsuario", usuarios);
    }
}
