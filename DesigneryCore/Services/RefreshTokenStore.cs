using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace DesigneryCore.Services
{
    public class RefreshTokenStore
    {
        private readonly ConcurrentDictionary<string, string> _refreshTokens = new ConcurrentDictionary<string, string>();

        public void SaveRefreshToken(string email, string refreshToken)
        {
            _refreshTokens[email] = refreshToken;
        }

        public string GetRefreshToken(string email)
        {
            _refreshTokens.TryGetValue(email, out var refreshToken);
            return refreshToken;
        }
    }
}
