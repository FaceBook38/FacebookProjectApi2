using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using FaceBookAPI.Models.FaceBook;
using FaceBookAPI.Models.ViewModels;

namespace FaceBookAPI.Controllers
{
    public class UsersController : ApiController
    {
        private FacebookContext db = new FacebookContext();

        // GET: api/Users
        //get all users that deleted = false 
        public IQueryable<User> GetUsers()
        {
            return db.Users.Where(user => user.deleted == false && user.user_type == "user");
        }

        //Login 
        //Post: api/Users
        [HttpPost]
        [Route("api/users/login")]
        [ResponseType(typeof(User))]
        public IHttpActionResult LoginUser(LoginViewModel login)
        {
            
            User user = db.Users.FirstOrDefault(user_ => user_.user_email == login.user_email&&user_.user_password == login.user_password && user_.deleted == false && user_.user_type == "user");
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/Users/5
        // GET: api/Users/5 and deleted =false
        [ResponseType(typeof(User))]
      //  [Route("api/users/getuser")]
        public IHttpActionResult GetUser([FromUri]int id)
        {
            User user = db.Users.FirstOrDefault(user_ => user_.user_id == id && user_.deleted == false && user_.user_type == "user");
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser([FromUri]int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.user_id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.user_type = "user";
            user.deleted = false;
            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.user_id }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            user.deleted = true;
            db.SaveChanges();

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
            return db.Users.Count(e => e.user_id == id) > 0;
        }
    }
}