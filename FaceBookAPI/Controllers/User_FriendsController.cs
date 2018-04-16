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

namespace FaceBookAPI.Controllers
{
    public class User_FriendsController : ApiController
    {
        
        private FacebookContext db = new FacebookContext();

        // GET: api/User_Friends
        public IQueryable<User_Friends> GetUser_Friends()
        {
            return db.User_Friends;
        }

        // GET: api/User_Friends/5
        [ResponseType(typeof(User_Friends))]
        public IHttpActionResult GetUser_Friends(int id)
        {
            User_Friends user_Friend = db.User_Friends.FirstOrDefault(u => u.user_friend_id == id && u.request==true);
            if (user_Friend == null)
            {
                return NotFound();
            }

            return Ok(user_Friend);
        }

        // PUT: api/User_Friends/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser_Friends(int id, User_Friends user_Friends)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user_Friends.user_id)
            {
                return BadRequest();
            }

            db.Entry(user_Friends).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!User_FriendsExists(id))
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

        // POST: api/User_Friends
        [ResponseType(typeof(User_Friends))]
        public IHttpActionResult PostUser_Friends(User_Friends user_Friends)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User_Friends.Add(user_Friends);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (User_FriendsExists(user_Friends.user_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = user_Friends.user_id }, user_Friends);
        }

        // DELETE: api/User_Friends/5
        [Route("api/user_friend/{Friendid:int}/{Userid:int}")]
        [ResponseType(typeof(User_Friends))]
        public IHttpActionResult DeleteUser_Friends(int Friendid,int Userid)
        {
            //i should replace 1 by the id of the current user
           
            User_Friends user_Friends = db.User_Friends.Where(u=>u.user_id==Userid ).FirstOrDefault(p=>p.user_friend_id==Friendid);
            if (user_Friends == null)
            {
                return NotFound();
            }

            db.User_Friends.Remove(user_Friends);
            db.SaveChanges();

            return Ok(user_Friends);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool User_FriendsExists(int id)
        {
            return db.User_Friends.Count(e => e.user_id == id) > 0;
        }
    }
}