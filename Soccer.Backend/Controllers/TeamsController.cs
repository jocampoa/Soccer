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
            Team team = await db.Teams.FindAsync(id);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TeamId,Name,Logo,Initials,LeagueId")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", team.LeagueId);
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", team.LeagueId);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TeamId,Name,Logo,Initials,LeagueId")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", team.LeagueId);
            return View(team);
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
