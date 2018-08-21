namespace Soccer.Backend.Models
{
    using Soccer.Domain;

    public class LocalDataContext : DataContext
    {
        public System.Data.Entity.DbSet<Soccer.Domain.Tournament> Tournaments { get; set; }
    }
}