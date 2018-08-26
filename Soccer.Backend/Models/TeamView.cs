namespace Soccer.Backend.Models
{
    using Domain;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class TeamView : Team
    {
        [Display(Name = "Logo")]
        public HttpPostedFileBase LogoFile { get; set; }
    }
}