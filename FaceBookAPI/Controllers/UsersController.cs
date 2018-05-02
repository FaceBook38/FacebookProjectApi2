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
        [ResponseType(typeof(Post))]
        [Route("api/Users/Friends/{id:int}")]
        public IHttpActionResult GetFriends(int id)
        {
            List<User> users = (from u in db.Users
                                from fu in db.User_Friends
                                where u.user_id == fu.user_friend_id && fu.user_id == id
                                select u).OrderBy(u => u.user_name).ToList();
            if (users.Count == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }


        //GET:User By Name For Search
        [Route("api/users/{id:int}/{name:alpha}")]
        [ResponseType(typeof(User))]
        [HttpGet]
        public IHttpActionResult GetUserByName(int id, string name)
        {   
            List<User> users = db.Users.Where(u => u.user_name.ToLower().Contains(name.ToLower())||u.user_email.ToLower()==name.ToLower() && u.deleted==false && u.user_id!=id).ToList<User>();
            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);

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

            return Ok(user);
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
                                select new UserPost() {user=u, post=p}).OrderByDescending(p=>p.post.post_id).ToList();
            //List < Post > post = db.Posts.Where(p => p.user_id == id && p.deleted == false).ToList();
            if (posts.Count==0)
            {
                return NotFound();
            }

            return Ok(posts);
        }

        // GET: api/Users/Friends/5
        //edit get all Friends that Request = false  and it's id ==5
        [ResponseType(typeof(Post))]
        [Route("api/Users/Friends/{id:int}")]
        public IHttpActionResult GetFriends(int id)
        {
            List<User> users = (from u in db.Users
                               from fu in db.User_Friends
                               where u.user_id == fu.user_friend_id && fu.user_id == id
                               select u).OrderBy(u=>u.user_name).ToList();
            if (users.Count == 0)
            {
                return NotFound();
            }

            return Ok(users);
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