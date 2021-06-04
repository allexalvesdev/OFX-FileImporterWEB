using ConciliacaoBancariaAuvo.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConciliacaoBancariaAuvo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Extrato> Extratos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
    }
}
