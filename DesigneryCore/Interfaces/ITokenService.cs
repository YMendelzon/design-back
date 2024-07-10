namespace DesigneryCore.Interfaces
{
    public interface ITokenService
    {
        string BuildToken(string userEmail, string key, string issuer, string audience, double expiryDurationMinutes);
        bool ValidateToken(string token);
        string GetEmailFromToken(string token);
    }
}