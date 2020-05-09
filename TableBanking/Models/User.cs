using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TableBanking.Models
{
    public class User
    {
        
        public int Id { get; set; }
        public string Fisrtname { get; set; }
        public string Lastname { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
