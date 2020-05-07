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

    }
}