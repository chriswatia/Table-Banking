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
    }
}