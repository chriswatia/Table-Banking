using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TableBanking.Models
{
    public class UserWithToken : User
    {
        public string AccessToken { get; set; }

        public UserWithToken(User user)
        {
            this.Id = user.Id;
            this.Fisrtname = user.Fisrtname;
            this.Lastname = user.Lastname;
            this.Phone = user.Phone;
            this.Email = user.Email;
            this.Password = user.Password;
        }
    }
}
