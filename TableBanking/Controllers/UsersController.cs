using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TableBanking.Models;

namespace TableBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private TBankContext _tbankContext;

        public UsersController(TBankContext context)
        {
            _tbankContext = context;
        }

        //Get all Users
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return _tbankContext.Users.ToList();
        }

        //Create User
        [HttpPost]
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

        ~UsersController()
        {
            _tbankContext.Dispose();
        }

    }
}