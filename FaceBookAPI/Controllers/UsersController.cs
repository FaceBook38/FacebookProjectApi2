﻿using System;
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
using FacebookConsumer.Models.FaceBook;

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

        // GET: api/Users/5
        // GET: api/Users/5 and deleted =false
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
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
        public IHttpActionResult PutUser(int id, User user)
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
        // GET: api/Users/Posts/5
        //edit get all posts that deleted = false  and it's id ==5
        [ResponseType(typeof(Post))]
        [Route("api/Users/Posts/{id:int}")]
        public IHttpActionResult GetPost(int id)
        {
           List<UserPost> posts = (from p in db.Posts
                                from u in db.Users
                                where p.user_id == id && p.deleted == false && p.user_id==u.user_id
                                select new UserPost() {user=u, post=p}).ToList();
            List < Post > post = db.Posts.Where(p => p.user_id == id && p.deleted == false).ToList();
            if (posts.Count==0)
            {
                return NotFound();
            }

            return Ok(posts);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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