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

        public DbSet<League> Leagues { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<TournamentGroup> TournamentGroups { get; set; }

        public DbSet<Date> Dates { get; set; }

        public DbSet<TournamentTeam> TournamentTeams { get; set; }
    }
}
