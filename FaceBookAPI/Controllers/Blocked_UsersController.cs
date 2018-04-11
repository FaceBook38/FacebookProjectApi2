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
    // batee5 bel betengaaan
    // da m4 btee5 
    //jkjk
    public class Blocked_UsersController : ApiController
    {
        private FacebookContext db = new FacebookContext();

        // GET: api/Blocked_Users
        public IQueryable<Blocked_Users> GetBlocked_Users()
        {
            return db.Blocked_Users;
        }

        // GET: api/Blocked_Users/5
        [ResponseType(typeof(Blocked_Users))]
        public IHttpActionResult GetBlocked_Users(int id)
        {
            Blocked_Users blocked_Users = db.Blocked_Users.Find(id);
            if (blocked_Users == null)
            {
                return NotFound();
            }
            //lll//
            return Ok(blocked_Users);
        }

        // PUT: api/Blocked_Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBlocked_Users(int id, Blocked_Users blocked_Users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blocked_Users.user_id)
            {
                return BadRequest();
            }

            db.Entry(blocked_Users).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Blocked_UsersExists(id))
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

        // POST: api/Blocked_Users
        [ResponseType(typeof(Blocked_Users))]
        public IHttpActionResult PostBlocked_Users(Blocked_Users blocked_Users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Blocked_Users.Add(blocked_Users);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (Blocked_UsersExists(blocked_Users.user_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = blocked_Users.user_id }, blocked_Users);
        }

        // DELETE: api/Blocked_Users/5
        [ResponseType(typeof(Blocked_Users))]
        public IHttpActionResult DeleteBlocked_Users(int id)
        {
            Blocked_Users blocked_Users = db.Blocked_Users.Find(id);
            if (blocked_Users == null)
            {
                return NotFound();
            }

            db.Blocked_Users.Remove(blocked_Users);
            db.SaveChanges();

            return Ok(blocked_Users);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Blocked_UsersExists(int id)
        {
            return db.Blocked_Users.Count(e => e.user_id == id) > 0;
        }
    }
}