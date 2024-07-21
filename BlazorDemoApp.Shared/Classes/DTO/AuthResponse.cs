using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Classes.DTO
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; } // Optional, for refresh token flow
    }

    public class AuthModel
    {
        public string Username { get; set; }
        public int UserID { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
