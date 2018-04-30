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
using FacebookConsumer.Models.FaceBook;

namespace FaceBookAPI.Controllers
{
    public class GroupsController : ApiController
    {
        private FacebookContext db = new FacebookContext();

        // GET: api/Groups
        //edit get all group that it's daleted = false 
        public IQueryable<Group> GetGroups()
        {
            return db.Groups.Where(g => g.deleted == false);
        }

        // GET: api/Groups/5
        [ResponseType(typeof(Group))]
        public IHttpActionResult GetGroup(int id)
        {
            Group group = db.Groups.FirstOrDefault(g => g.deleted == false && g.group_id == id);
            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        // PUT: api/Groups/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGroup(int id, Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group.group_id)
            {
                return BadRequest();
            }

            db.Entry(group).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
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

        // POST: api/Groups
        [ResponseType(typeof(Group))]
        public IHttpActionResult PostGroup(Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Groups.Add(group);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = group.group_id }, group);
        }

        // DELETE: api/Groups/5
        [ResponseType(typeof(Group))]
        public IHttpActionResult DeleteGroup(int id)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            group.deleted = true;
            db.SaveChanges();

            return Ok(group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroupExists(int id)
        {
            return db.Groups.Count(e => e.group_id == id) > 0;
        }
    }
}