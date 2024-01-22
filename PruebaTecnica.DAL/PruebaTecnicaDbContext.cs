using Microsoft.EntityFrameworkCore;

using PruebaTecnica.DAL.Model;
using PruebaTecnica.DAL.Service;
using PruebaTecnica.DAL.Interface;

namespace PruebaTecnica.DAL;
public class PruebaTecnicaDbContext : DbContext
{
    //public PruebaTecnicaDbContext() : base("name=cnnPrueba")
    //{ }

    public PruebaTecnicaDbContext(DbContextOptions<PruebaTecnicaDbContext> options) : base(options)
    { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<ExpedienteUsuario> ExpedienteUsuario { get; set; }
    public DbSet<UsuarioLogin> UsuarioLogin { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>().ToTable(nameof(Usuario));
        modelBuilder.Entity<ExpedienteUsuario>().ToTable(nameof(Usuario));
        modelBuilder.Entity<UsuarioLogin>().ToTable(nameof(UsuarioLogin));

        /*semilla*/
        var expediente = expedienteData();
        var usuario = usuarioData(expediente);

        modelBuilder.Entity<ExpedienteUsuario>().HasData(expediente);
        modelBuilder.Entity<Usuario>().HasData(usuario);
        modelBuilder.Entity<UsuarioLogin>().HasData(usuarioLoginData(usuario.Id));
    }

    public ExpedienteUsuario expedienteData()
    {
        string[] nombres = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
        string[] apellidos = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };

        Random rand = new Random();
        int rng = rand.Next(1, 7);
        string nombre = nombres[rng - 1];
        string apellido = apellidos[rng - 1];
        string login = $"pcastro".ToLower();

        return new ExpedienteUsuario
        {
            Id = Guid.NewGuid(),
            Identificativo = Guid.NewGuid().ToString().Replace("-", ""),
            Nombres = nombre,
            Apellidos = apellido,
            Direccion = "lorem ipsum",
            Correo = $"{login}@mail.com",
            Telefono = "99889368"
        };
    }

    public Usuario usuarioData(ExpedienteUsuario expedienteUsuario)
    {
        string login = expedienteUsuario.Correo.Split("@")[0];
        IEncriptarContrasena _encriptar = new EncriptarContrasena();

        return new Usuario
        {
            Id = Guid.NewGuid(),
            ExpedienteId = expedienteUsuario.Id,
            Login = login,
            Contrasena = _encriptar.Encriptar("123456789"),
            Avatar = "https://api.api-ninjas.com/v1/randomimage?category=nature",
        };
    }

    public UsuarioLogin usuarioLoginData(Guid usuarioId)
    {
        return new UsuarioLogin
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJMb2dpbklkIjoicHJmbG9yZXMiLCJuYmYiOjE3MDU4NzY4NzEsImV4cCI6MTcwNTg4MDQ3MSwiaWF0IjoxNzA1ODc2ODcxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDA0IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzAwNCJ9.fXsxn1Xt9XpaGzWOP5fAU2sIji_z7SCgQRFK0nbKJGc",
            FechaExpiracion = DateTime.Now.AddMinutes(60),
            Vigente = true
        };
    }
}