namespace BlazorDemoApp.API.Helpers
{
    //Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
    //Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
    //Install-Package Microsoft.EntityFrameworkCore
    //Install-Package Microsoft.EntityFrameworkCore.Design
    //Install-Package Microsoft.EntityFrameworkCore.SqlServer
    //Install-Package Microsoft.EntityFrameworkCore.Tools
    //Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design
    //Install-Package System.IdentityModel.Tokens.Jwt

    //Generate Key: https://8gwifi.org/jwsgen.jsp
    public class JWT_Class
    {

        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInDays { get; set; }

    }
}
