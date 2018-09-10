namespace Soccer.Backend.Models
{
    using Soccer.Domain;

    public class LocalDataContext : DataContext
    {
        public System.Data.Entity.DbSet<Soccer.Domain.UserType> UserTypes { get; set; }

        public System.Data.Entity.DbSet<Soccer.Domain.User> Users { get; set; }

        public System.Data.Entity.DbSet<Soccer.Domain.Status> Status { get; set; }
    }
}