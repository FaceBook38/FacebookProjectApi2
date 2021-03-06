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
//using FacebookConsumer.Models.FaceBook;

namespace FaceBookAPI.Controllers
{
    public class PostsController : ApiController
    {
        private FacebookContext db = new FacebookContext();

        // GET: api/Posts
        //edit get all posts that deleted = false 
        public IQueryable<Post> GetPosts()
        {
            return db.Posts.Where(p => p.deleted == false).OrderByDescending(p=>p.post_id);
        }

        // GET: api/Posts/5
        //edit get all posts that deleted = false  and it's id ==5
        [ResponseType(typeof(Post))]
        public IHttpActionResult GetPost(int id)
        {
            Post post = db.Posts.FirstOrDefault(p => p.post_id == id && p.deleted == false);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // PUT: api/Posts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPost(int id, Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != post.post_id)
            {
                return BadRequest();
            }

            db.Entry(post).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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

        // POST: api/Posts
        [ResponseType(typeof(Post))]
        public IHttpActionResult PostPost(Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Posts.Add(post);
            db.SaveChanges();
            List<UserPost> posts = (from p in db.Posts
                                    from u in db.Users
                                    where p.user_id == post.user_id && p.deleted == false && p.user_id == u.user_id
                                    select new UserPost() { user = u, post = p }).OrderByDescending(p=>p.post.post_id).ToList();
            if (posts.Count == 0)
            {
                return NotFound();
            }
            return Ok(posts);
            //return CreatedAtRoute("DefaultApi", new { id = post.post_id }, post);
        }

        // DELETE: api/Posts/5
        [ResponseType(typeof(Post))]
        public IHttpActionResult DeletePost(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            post.deleted = true;
            db.SaveChanges();

            return Ok(post);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostExists(int id)
        {
            return db.Posts.Count(e => e.post_id == id) > 0;
        }
    }
}