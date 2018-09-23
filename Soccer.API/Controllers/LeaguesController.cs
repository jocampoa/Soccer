using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Soccer.API.Models;
using Soccer.Domain;

namespace Soccer.API.Controllers
{
    [Authorize]
    public class LeaguesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Leagues
        public async Task<IHttpActionResult> GetLeagues()
        {
            var leagues = await db.Leagues.ToArrayAsync();
            var list = new List<LeagueResponse>();
            foreach (var league in leagues)
            {
                list.Add(new LeagueResponse
                {
                    LeagueId = league.LeagueId,
                    Logo = league.Logo,
                    Name = league.Name,
                    Teams = league.Teams.ToList(),
                });
            }

            return Ok(list);
        }

        // GET: api/Leagues/5
        [ResponseType(typeof(League))]
        public IHttpActionResult GetLeague(int id)
        {
            League league = db.Leagues.Find(id);
            if (league == null)
            {
                return NotFound();
            }

            return Ok(league);
        }

        // PUT: api/Leagues/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLeague(int id, League league)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != league.LeagueId)
            {
                return BadRequest();
            }

            db.Entry(league).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeagueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Leagues
        [ResponseType(typeof(League))]
        public IHttpActionResult PostLeague(League league)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Leagues.Add(league);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = league.LeagueId }, league);
        }

        // DELETE: api/Leagues/5
        [ResponseType(typeof(League))]
        public IHttpActionResult DeleteLeague(int id)
        {
            League league = db.Leagues.Find(id);
            if (league == null)
            {
                return NotFound();
            }

            db.Leagues.Remove(league);
            db.SaveChanges();

            return Ok(league);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LeagueExists(int id)
        {
            return db.Leagues.Count(e => e.LeagueId == id) > 0;
        }
    }
}