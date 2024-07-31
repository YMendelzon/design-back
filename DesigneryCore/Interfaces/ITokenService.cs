namespace DesigneryCore.Interfaces
{
    public interface ITokenService
    {
        string BuildAccessToken(string role, string userEmail);
        string BuildRefreshToken();
        bool ValidateAccessToken(string token);
        bool ValidateRefreshToken(string token);
        string GetEmailFromAccessToken(string token);
        Task SaveRefreshToken(string email, string refreshToken);
        string GetRefreshToken(string email);
    }
}