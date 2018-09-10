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
using Newtonsoft.Json.Linq;
using Soccer.API.Models;
using Soccer.Domain;

namespace Soccer.API.Controllers
{
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private DataContext db = new DataContext();

        [HttpPost]
        [Route("GetUserByEmail")]
        public async Task<IHttpActionResult> GetUserByEmail(JObject form)
        {
            var email = string.Empty;
            dynamic jsonObject = form;

            try
            {
                email = jsonObject.Email.Value;
            }
            catch
            {
                return BadRequest("Incorrect call");
            }

            var user = await db.Users
                .Where(u => u.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            if (user.GroupUsers.Count == 0)
            {
                await AddUserToGroup(user.UserId);
            }

            var userResponse = ToUserResponse(user);
            return Ok(userResponse);
        }

        private async Task AddUserToGroup(int userId)
        {
            var groups = await db.Groups.ToListAsync();
            var radom = new Random();

            var groupUser = new GroupUser
            {
                GroupId = groups[radom.Next(groups.Count)].GroupId,
                IsAccepted = true,
                IsBlocked = false,
                Points = 0,
                UserId = userId,
            };

            db.GroupUsers.Add(groupUser);
            await db.SaveChangesAsync();
        }

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userResponse = ToUserResponse(user);
            return Ok(userResponse);
        }

        private UserResponse ToUserResponse(User user)
        {
            return new UserResponse
            {
                Email = user.Email,
                FavoriteTeam = user.FavoriteTeam,
                FavoriteTeamId = user.FavoriteTeamId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NickName = user.NickName,
                Picture = user.Picture,
                Points = user.Points,
                UserId = user.UserId,
                UserType = user.UserType,
                UserTypeId = user.UserTypeId,
            };
        }



        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}