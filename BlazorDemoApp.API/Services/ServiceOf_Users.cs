using BlazorDemoApp.Shared.Classes.BaseClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.API.Services
{

    [BaseOfServiceV2Rank(0)]
    public class ServiceOf_Users : BaseOfServiceV2<MyAppUser>
    {
        private IMapper _mapper;
        private readonly JWT_Class _jwt;

        public ServiceOf_Users(IUnitOfWork unitOfWork, ILogger<MyAppUser> logger, IMapper mapper, IOptions<JWT_Class> jwt)
             : base(unitOfWork, logger)
        {
            _mapper = mapper;
            _jwt = jwt.Value;
        }

        public class AuthModel
        {
            public string Username { get; set; }
            public int UserID { get; set; }
            public string Token { get; set; }
            public DateTime ExpiresOn { get; set; }
        }

        private JwtSecurityToken CreateJwtToken(MyAppUser user)
        {
            //var userClaims = await _userManager.GetClaimsAsync(user);
            //var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

           

            var permissions = new List<MyAppUserPermission>() { };

            if (user.Id == 1)

                foreach (int i in Enum.GetValues(typeof(AppServices)))
                    permissions.Add(new MyAppUserPermission() { Id = i, AppServicesId = (short?)i, Can_Insert = true, Can_Delete = true, Can_search = true, Can_Update = true, Can_print = true });


            else
                permissions = _unitOfWork.GetRepository<MyAppUserPermission>()?.FindAll(a => a.userID == user.Id).ToList();


            var str_roleClaims = JsonConvert.SerializeObject(permissions, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            var claims = new[]
            {
                new Claim("permissions",str_roleClaims)
            };

           

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                //expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public ApiResponse<object> Register(Base0_MyAppUser_Register m)
        {

            if (TFindAll(a => a.UserName == m.UserName) is not null)
                return new ApiResponse<object> { Success = false, Message = "Username is already registered!" };


            if (TFindAll(a => a.UserPass == m.UserPass) is not null)
                return new ApiResponse<object> { Success = false, Message = "Email is already registered!" };

            var user = _mapper.Map<MyAppUser>(m);
            user.IsActive = true;
            user.isPasswordReset = false; //testonly

            var result = TAddEntityWithCurrentUser(user, 1);

            if (result is null)
                return new ApiResponse<object> { Success = false, Message = str_save_no };
            else
                return new ApiResponse<object> { Success = true, Message = str_save_yes };

        }

        internal ApiResponse<object> login(Base0_MyAppUser_Login m)
        {
            var rslt0 = TFindAll(a => a.UserName == m.UserName && a.UserPass == m.UserPass);
            if (rslt0.Count() ==0 ) return new ApiResponse<object> { Success = false, Message = user_NotFound };

            var rslt = rslt0.First();
            if (rslt == null)
            {
                return new ApiResponse<object> { Success = false, Message = user_NotFound };
            }

            if (rslt.IsActive == false)
            {
                return new ApiResponse<object> { Success = false, Message = user_InActive };
            }

            if (rslt.isPasswordReset == true)
            {
                return new ApiResponse<object> { Success = false, Message = user_MustChangePass };

            }
            var jwtSecurityToken =  CreateJwtToken(rslt);


            var model = new AuthModel() 
            {
                Username = rslt.UserName,
                UserID= rslt.Id,
                ExpiresOn = jwtSecurityToken.ValidTo,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                
            };

            return new ApiResponse<object> { Success = true, Message = user_MustChangePass, Data= model };
        }
    }

}