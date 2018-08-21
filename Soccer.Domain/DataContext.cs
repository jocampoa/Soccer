namespace Soccer.Domain
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<Soccer.Domain.League> Leagues { get; set; }

        public System.Data.Entity.DbSet<Soccer.Domain.Team> Teams { get; set; }
    }
}
