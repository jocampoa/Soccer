using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Soccer.Domain;
using Soccer.Backend.Models;
using Soccer.Backend.Helpers;

namespace Soccer.Backend.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Teams
        public async Task<ActionResult> Index()
        {
            var teams = db.Teams.Include(t => t.League);
            return View(await teams.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var team = await db.Teams.FindAsync(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name");
            return View();
        }

        // POST: Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TeamView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Teams";

                if (view.LogoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var team = ToTeam(view);
                team.Logo = pic;
                db.Teams.Add(team);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", view.LeagueId);
            return View(view);
        }

        private Team ToTeam(TeamView view)
        {
            return new Team
            {
                Initials = view.Initials,
                League = view.League,
                LeagueId = view.LeagueId,
                Logo = view.Logo,
                Name = view.Name,
                TeamId = view.TeamId,
            };
        }

        // GET: Teams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var team = await db.Teams.FindAsync(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            var view = ToView(team);
            return View(view);
        }

        private TeamView ToView(Team team)
        {
            return new TeamView
            {
                Initials = team.Initials,
                League = team.League,
                LeagueId = team.LeagueId,
                Logo = team.Logo,
                Name = team.Name,
                TeamId = team.TeamId,
            };
        }

        // POST: Teams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TeamView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.Logo;
                var folder = "~/Content/Teams";

                if (view.LogoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var team = ToTeam(view);
                team.Logo = pic;
                db.Entry(team).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", view.LeagueId);
            return View(view);
        }

        // GET: Teams/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Team team = await db.Teams.FindAsync(id);
            db.Teams.Remove(team);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
