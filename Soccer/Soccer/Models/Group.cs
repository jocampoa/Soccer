using System;
using System.Collections.Generic;
using System.Text;

namespace Soccer.Models
{
    public class Group
    {
        public int TournamentGroupId { get; set; }

        public string Name { get; set; }

        public int TournamentId { get; set; }
    }
}
