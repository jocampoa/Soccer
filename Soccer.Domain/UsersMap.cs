﻿using System.Data.Entity.ModelConfiguration;

namespace Soccer.Domain
{
    internal class UsersMap : EntityTypeConfiguration<User>
    {
        public UsersMap()
        {
            HasRequired(o => o.FavoriteTeam)
                .WithMany(m => m.Fans)
                .HasForeignKey(m => m.FavoriteTeamId);
        }
    }
}