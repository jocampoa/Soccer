namespace Soccer.Backend.Models
{
    using Domain;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    [NotMapped]
    public class LeagueView : League
    {
        [Display(Name = "Logo")]
        public HttpPostedFileBase LogoFile { get; set; }
    }
}