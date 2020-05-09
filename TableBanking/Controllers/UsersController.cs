using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TableBanking.Models;

namespace TableBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private TBankContext _tbankContext;
        private JWTSettings _jwtsettings;

        public UsersController(TBankContext context, IOptions<JWTSettings> jwtsettings)
        { 
            _tbankContext = context;
            _jwtsettings = jwtsettings.Value;
        }

        //Login(authenticate user)
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<UserWithToken>> Login([FromBody] User user)
        {
            user = await _tbankContext.Users.Where(u => u.Email == user.Email
                                && user.Password == user.Password).FirstOrDefaultAsync();

            UserWithToken userWithToken = new UserWithToken(user);

            if (userWithToken == null)
            {
                return NotFound();
            }

            //sign your token here here..
            userWithToken.AccessToken = GenerateAccessToken(user.Id);

            return userWithToken;
        }

        //Create User
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Post([FromBody]User user)
        {
            //check if user is supplied
            if (user == null)
            {
                return NotFound("User data is not supplied");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tbankContext.Users.AddAsync(user);
            await _tbankContext.SaveChangesAsync();
            return Ok(user);
        }

        //Get all Users
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return _tbankContext.Users.ToList();
        }

        //Get User by Id
        [HttpGet]
        [Route("getbyid/{id}")]
        public ActionResult<User> GetById(int? id)
        {
            if (id <= 0)
            {
                return NotFound("User id must greater than zero");
            }

            User user = _tbankContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound("No User found with that Id");
            }

            return Ok(user);
        }
        //Update User
        [HttpPut]
        public async Task<ActionResult> Update([FromBody]User user)
        {
            if (user == null)
            {
                return NotFound("User data is not supplied");                
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User existingUser = _tbankContext.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
            {
                return NotFound("User does not exist");
            }

            existingUser.Fisrtname = user.Fisrtname;
            existingUser.Lastname = user.Lastname;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            _tbankContext.Attach(existingUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _tbankContext.SaveChangesAsync();
            return Ok(existingUser);
        }

        //Delete method
        [HttpDelete]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("Id is not supplied");
            }
            User user = _tbankContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound("No User found with that Id");
            }

            _tbankContext.Users.Remove(user);
            await _tbankContext.SaveChangesAsync();
            return Ok("User deleted successfully");
        }
        private string GenerateAccessToken(int Id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(Id))
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        ~UsersController()
        {
            _tbankContext.Dispose();
        }

    }
}