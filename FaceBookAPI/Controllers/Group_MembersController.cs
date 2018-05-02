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
    public class Group_MembersController : ApiController
    {
        private FacebookContext db = new FacebookContext();

        // GET: api/Group_Members
        public IQueryable<Group_Members> GetGroup_Members()
        {
            return db.Group_Members;
        }

        // GET: api/Group_Members/5
        [ResponseType(typeof(Group_Members))]
        public List<User> GetGroup_Members(int id)
        {
            var users =
                 from User in db.Users
                 join Group_Members in db.Group_Members on User.user_id equals Group_Members.user_id
                 where Group_Members.group_id == id
                 select User;
            if (users == null)
            {
                return new List<User>();
            }

            return users.ToList<User>();
        }
    

        // PUT: api/Group_Members/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGroup_Members(int id, Group_Members group_Members)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group_Members.group_id)
            {
                return BadRequest();
            }

            db.Entry(group_Members).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Group_MembersExists(id))
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

        // POST: api/Group_Members
        [ResponseType(typeof(Group_Members))]
        public IHttpActionResult PostGroup_Members(Group_Members group_Members)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Group_Members> members = db.Group_Members.Where(m=>m.group_id==group_Members.group_id).ToList<Group_Members>();
            int flag = 0;
            foreach (var item in members)
            {
                if (item.user_id == group_Members.user_id)
                    flag = 1;
            }
            if(flag==0)
            db.Group_Members.Add(group_Members);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (Group_MembersExists(group_Members.group_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = group_Members.group_id }, group_Members);
        }

        // DELETE: api/Group_Members/5
        [ResponseType(typeof(Group_Members))]
        public IHttpActionResult DeleteGroup_Members(int id)
        {
            Group_Members group_Members = db.Group_Members.Find(id);
            if (group_Members == null)
            {
                return NotFound();
            }

            db.Group_Members.Remove(group_Members);
            db.SaveChanges();

            return Ok(group_Members);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Group_MembersExists(int id)
        {
            return db.Group_Members.Count(e => e.group_id == id) > 0;
        }
    }
}