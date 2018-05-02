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
//using FacebookConsumer.Models.FaceBook;

namespace FaceBookAPI.Controllers
{
    public class Group_PostsController : ApiController
    {
        private FacebookContext db = new FacebookContext();

        // GET: api/Group_Posts
        public IQueryable<Group_Posts> GetGroup_Posts()
        {
            return db.Group_Posts;
        }

        // GET: api/Group_Posts/5
        [ResponseType(typeof(Group_Posts))]
        public IHttpActionResult GetGroup_Posts(int id)
        {
           List<Group_Posts> group_Posts = db.Group_Posts.Where(g=>g.group_id==id).OrderByDescending(p => p.post_id).ToList<Group_Posts>();
            if (group_Posts == null)
            {
                return NotFound();
            }

            return Ok(group_Posts);
        }

        // PUT: api/Group_Posts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGroup_Posts(int id, Group_Posts group_Posts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group_Posts.post_id)
            {
                return BadRequest();
            }

            db.Entry(group_Posts).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Group_PostsExists(id))
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

        // POST: api/Group_Posts
        [ResponseType(typeof(Group_Posts))]
        public IHttpActionResult PostGroup_Posts(Group_Posts group_Posts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Group_Posts.Add(group_Posts);
            List<Group_Posts> posts = new List<Group_Posts>();
            try
            {
                db.SaveChanges();
                posts = db.Group_Posts.Where(p => p.group_id == group_Posts.group_id).ToList();
            }
            catch (DbUpdateException)
            {
                if (Group_PostsExists(group_Posts.post_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return Ok(posts);
            //return CreatedAtRoute("DefaultApi", new { id = group_Posts.post_id }, group_Posts);
        }

        // DELETE: api/Group_Posts/5
        [ResponseType(typeof(Group_Posts))]
        public IHttpActionResult DeleteGroup_Posts(int id)
        {
            Group_Posts group_Posts = db.Group_Posts.Find(id);
            if (group_Posts == null)
            {
                return NotFound();
            }

            db.Group_Posts.Remove(group_Posts);
            db.SaveChanges();

            return Ok(group_Posts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Group_PostsExists(int id)
        {
            return db.Group_Posts.Count(e => e.post_id == id) > 0;
        }
    }
}