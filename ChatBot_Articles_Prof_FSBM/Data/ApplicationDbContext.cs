using ChatBot_Articles_Prof_FSBM.Models;
using Microsoft.EntityFrameworkCore;


namespace ChatBot_Articles_Prof_FSBM.Data
{
    public class ApplicationDbContext : DbContext
    {
        //ctor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // prop; Category is the property and name's Categories.

        public DbSet<Etudiant> Etudiant { get; set; }
        public DbSet<Prof> Prof { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Historique> Historique { get; set; }

    }
}
