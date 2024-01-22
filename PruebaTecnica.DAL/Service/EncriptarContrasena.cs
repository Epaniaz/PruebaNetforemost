using System.Security.Cryptography;

using PruebaTecnica.DAL.Interface;

namespace PruebaTecnica.DAL.Service;
public class EncriptarContrasena : IEncriptarContrasena
{
    private const int _saltSize = 16; // 128 bits
    private const int _keySize = 32; // 256 bits
    private const int _iterations = 50000;
    private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;

    private const char segmentDelimiter = ':';

    public string Encriptar(string contrasena)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            contrasena,
            salt,
            _iterations,
            _algorithm,
            _keySize
        );

        return string.Join(
            segmentDelimiter,
            Convert.ToHexString(hash),
            Convert.ToHexString(salt),
            _iterations,
            _algorithm
        );
    }

    public bool Verificar(string contrasena, string contrasenaHash)
    {
        string[] segments = contrasenaHash.Split(segmentDelimiter);
        byte[] hash = Convert.FromHexString(segments[0]);
        byte[] salt = Convert.FromHexString(segments[1]);
        int iterations = int.Parse(segments[2]);
        HashAlgorithmName algorithm = new HashAlgorithmName(segments[3]);
        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
            contrasena,
            salt,
            iterations,
            algorithm,
            hash.Length
        );

        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }
}
