using Microsoft.EntityFrameworkCore;

namespace TableBanking.Models
{
    public class TBankContext :DbContext
    {
        public TBankContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
