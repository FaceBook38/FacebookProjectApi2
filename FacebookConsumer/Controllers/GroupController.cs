using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FacebookConsumer.Models;
using FacebookConsumer.Models.FaceBook;
using Newtonsoft.Json;

namespace FacebookConsumer.Controllers
{
    public class GroupController : Controller
    {
        string baseURL = "http://localhost:54555";
        // GET: Group
        public async Task<ActionResult> Index()
        {
            List<Group> lstGroup = new List<Group>();
            using (var client =new HttpClient())
            {
                //Passing service base url
                client.BaseAddress =new Uri(baseURL);
                client.DefaultRequestHeaders.Clear();
                //define formate of request 
                client.DefaultRequestHeaders.Accept.Add(new  MediaTypeWithQualityHeaderValue("application/json"));
              HttpResponseMessage  response = await client.GetAsync("/api/groups");

                if (response.IsSuccessStatusCode)
                {
                  var  result = response.Content.ReadAsStringAsync().Result;
                    //decserlizes
                    lstGroup = JsonConvert.DeserializeObject<List<Group>>(result);
                }
            }
            
            return View(lstGroup);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Group group,HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
               // string path = System.IO.Path.Combine(Server.MapPath("~/images/profile"), pic);
              //  file.SaveAs(pic);
 
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    group.group_image = ms.GetBuffer();
                }

            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:54555");
                group.group_admin =Convert.ToInt32(Session["user_id"]);
                group.deleted = false;
                var postTask = client.PostAsJsonAsync<Group>("/api/Groups", group);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View();
        }


        public async Task<ActionResult> DisplayMyGroupAsync()
        {
            List<Group> lstGroup = new List<Group>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("/api/UserGroups/"+Session["user_id"]);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    //decserlizes
                    lstGroup = JsonConvert.DeserializeObject<List<Group>>(result);
                }
            }

            return View(lstGroup);
        }

        //[Route("/group/GroupHomeAsync")]
        public  ActionResult GroupHomeAsync(int? id)
        {
            if (Session["group_id"] != null && id==null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Response = client.GetAsync("/api/Groups/" +Session["group_id"]).Result;
                    if (Response.IsSuccessStatusCode)
                    {

                        Group group = Response.Content.ReadAsAsync<Group>().Result;

                        return View(group);

                    }
                }
            }
            Session["group_id"] = id;
            if (id == null)
            {
            return View(new Group());
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Response = client.GetAsync("/api/Groups/" + id).Result;
                    if (Response.IsSuccessStatusCode)
                    {

                        Group group = Response.Content.ReadAsAsync<Group>().Result;

                        return View(group);

                    }
                }
            }
            return View(new Group());
        }


        public ActionResult GroupMembers(int? id)
        {
            if (id == null)
            {
                using (var client = new HttpClient())
                {
                    return PartialView(new  List<User>());
                }
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Response = client.GetAsync("/api/Group_Members/"+ id).Result;
                if (Response.IsSuccessStatusCode)
                {

                    List<User> users = Response.Content.ReadAsAsync<List<User>>().Result;

                    return PartialView(users);

                }
            }
            return PartialView(new List<User>());
        }

 
        public ActionResult AddMember(int id)
        {
            Group_Members group_member = new Group_Members();
            if(Session["group_id"] != null)
            {
                group_member.group_id =Convert.ToInt32(Session["group_id"]);
                group_member.user_id = id;
                group_member.join = true;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:54555");
                var postTask = client.PostAsJsonAsync<Group_Members>("api/Group_Members", group_member);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("GroupHomeAsync");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return RedirectToAction("GroupHomeAsync");
        }


        public ActionResult GetMyFriends()
        {
            if (Session["user_id"] == null)
            {

                return PartialView(new List<User>());

            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Response = client.GetAsync("/api/Users/Friends/" + Session["user_id"]).Result;
                if (Response.IsSuccessStatusCode)
                {

                    List<User> friends = Response.Content.ReadAsAsync<List<User>>().Result;

                    return PartialView(friends);

                }
            }
            return PartialView(new List<User>());
        }

        [HttpGet]
        public ActionResult createPost()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<ActionResult> createPost(Group_Posts post)
        {
            post.user_id =Convert.ToInt32(Session["user_id"]);
            post.deleted= false;
            post.group_id= Convert.ToInt32(Session["group_id"]);
            //api/Group_Posts

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:54555/");
                var postTask = client.PostAsJsonAsync<Group_Posts>("api/Group_Posts", post).Result;
                // postTask.Wait();
                //var result = postTask.Result;
                string groupPosts = await postTask.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<List<Group_Posts>>(groupPosts);

                if (postTask.IsSuccessStatusCode)
                {
                   
                    return RedirectToAction("GroupHomeAsync",Convert.ToInt32(Session["group_id"]));
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View();
        }

        public ActionResult getPosts() {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Response = client.GetAsync("/api/Group_Posts/" + Session["group_id"]).Result;
                if (Response.IsSuccessStatusCode)
                {

                    List<Group_Posts> posts = Response.Content.ReadAsAsync<List<Group_Posts>>().Result;

                    return PartialView(posts);

                }
            }
            return PartialView(new List<Group_Posts>());

        }

    }
      
}
