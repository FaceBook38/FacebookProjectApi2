using FaceBookAPI.Models.FaceBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FaceBookAPI.Controllers
{
    public class UserGroupsController : ApiController
    {
        private FacebookContext db = new FacebookContext();
        public IQueryable<Group> GetGroups()
        {
            return db.Groups.Where(g => g.deleted == false);
        }

        // GET: api/UserGroups/5
        [ResponseType(typeof(Group))]
        public List<Group> GetGroup(int id)
        {
            // var groups = db.Groups.Join(db.Group_Members,g=>g.group_id,m=>m.group_id,group=>group);
            var groups =
                  from Group in db.Groups
                  join Group_Members in db.Group_Members on Group.group_id equals Group_Members.group_id
                  where Group_Members.user_id == id || Group.group_admin==Group.User.user_id
                  //where Group_Members.@join =true
                  select  Group ; 
            if (groups == null)
            {
                return new List<Group>();
            }

            return groups.ToList<Group>();
        }
     
    }
}
