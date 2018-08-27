namespace Soccer.Backend.Models
{
    using Domain;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [NotMapped]
    public class TournamentTeamView : TournamentTeam
    {
        [Display(Name = "League")]
        public int LeagueId { get; set; }
    }
}