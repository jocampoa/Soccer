﻿namespace Soccer.API.Models
{
    using Domain;
    using System.Collections.Generic;

    public class LeagueResponse
    {
        public int LeagueId { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public List<Team> Teams { get; set; }
    }
}