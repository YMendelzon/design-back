namespace DesigneryCore.Interfaces
{
    public interface ITokenService
    {
        string BuildToken(string role, string userEmail);
        bool ValidateToken(string token);
        string GetEmailFromToken(string token);
    }
}