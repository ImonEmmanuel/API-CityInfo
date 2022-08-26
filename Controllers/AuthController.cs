using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CItyInfo.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        public class AuthControllerBody
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }    
        }

        private class CityInfoUser
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }

            public string LastName { get; set; }
            public string City { get; set; }

            public CityInfoUser(int userId, string userName, string firstName, string lastName, string city)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }
        }
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost("autheticate")]
        public ActionResult<string> Autheticate([FromBody]AuthControllerBody authbody)
        {
            //Validate the user using it username and password
            var user = ValidateUserCredentials(authbody.UserName, authbody.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            //Create a Token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Authetication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //The Claims List 
            var userClaim = new List<Claim>();
            userClaim.Add(new Claim("sub", user.UserId.ToString()));
            userClaim.Add(new Claim("user_name", user.UserName));
            userClaim.Add(new Claim("given_name", user.FirstName));
            userClaim.Add(new Claim("family_name", user.LastName));
            userClaim.Add(new Claim("city", user.City));

            var jwtSecurityToken = new JwtSecurityToken(
                _config["Authetication:Issuer"],
                _config["Authetication:Audience"],
                userClaim,
                DateTime.UtcNow, DateTime.UtcNow.AddHours(1),
                signingCredentials
                );

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken); 

            //jwt.io can be used to Decode a JWT token hence a token is not Encrypted but only encoded 
            //a jwt is splitted into Header (Algorithm), Payload(data), Signature

            return Ok(tokenToReturn);  
        }

        private CityInfoUser ValidateUserCredentials(string? userName, string? password)
        {
            //If a Db is provided that should contains the User Information
            // Checking the Username and Password against the Database should return the User Information

            return new CityInfoUser(
                1,"ImonEmmanuel", 
                "Emmanuel","Imonmion",
                "Yaba"
            );
        }
    }
}
