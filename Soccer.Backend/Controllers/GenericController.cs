namespace Soccer.Backend.Controllers
{
    using Backend.Models;
    using System.Linq;
    using System.Web.Mvc;

    public class GenericController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        public JsonResult GetTeams(int leagueId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var teams = db.Teams.Where(t => t.LeagueId == leagueId);
            return Json(teams);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}