namespace PruebaTecnica.Service.ViewModel;
public class Configuraciones
{
    public JwtOption JwtOptions { get; set; }
}

public class JwtOption
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SigningKey { get; set; }
    public int ExpirationSeconds { get; set; }
    public string KeySecret { get; set; }
}