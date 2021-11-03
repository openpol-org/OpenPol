using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenPol.WebAPI.Models.Common;
using OpenPol.WebAPI.Models.Request;
using OpenPol.WebAPI.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenPol.WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public UserResponse Auth(AuthRequest model)
        {
            

            UserResponse userResponse = new UserResponse();
            using (var db = new OpenPol.DataLayer.OpenpolContext())
            {
                string spassword = OpenPol.Libs.Tools.Encryption.GetSHA256(model.Password);

                var webApiusers = db.WebApiusers.Where(d=>d.Email == model.Email && 
                                                    d.Password == spassword).FirstOrDefault();

                if (webApiusers != null)
                {
                    userResponse.Email = webApiusers.Email;
                    userResponse.Token = GetToken(webApiusers);
                }

                return userResponse;
            }
        }

        private string GetToken(DataLayer.WebAPI.WebApiusers webApiusers)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        { 
                            new Claim(ClaimTypes.NameIdentifier, webApiusers.WebApiusersId.ToString()),
                            new Claim(ClaimTypes.Email, webApiusers.Email)
                        }
                    ),
                    Expires = DateTime.UtcNow.AddDays(60),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
