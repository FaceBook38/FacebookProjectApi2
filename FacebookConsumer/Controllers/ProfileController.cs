using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FaceBookAPI.Models.FaceBook;
using FacebookConsumer.Models.FaceBook;
using Newtonsoft.Json;


namespace FacebookConsumer.Controllers
{
    public class ProfileController : Controller
    {
        FacebookContext context = new FacebookContext();
        string Baseurl = "http://localhost:54555/";
        // GET: Profile
        public async Task<ActionResult> GetPosts()
        {
            List<UserPost> PostInfo = new List<UserPost>();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllPostloyees using HttpClient  
                HttpResponseMessage Res = client.GetAsync("api/Users/Posts/" + int.Parse(Session["user_id"].ToString())).Result;

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var PostResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Postloyee list  
                    PostInfo = JsonConvert.DeserializeObject<List<UserPost>>(PostResponse);

                }
                //returning the Postloyee list to view  
                return PartialView(PostInfo);
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreatePost()
        {
            return PartialView(new Post());
        }
        [HttpPost]
        public async Task<ActionResult> CreatePost(Post post)
        {
            post.deleted = false;
            post.user_id = int.Parse(Session["user_id"].ToString());
            using (var client = new HttpClient())
            {
                var myContent = JsonConvert.SerializeObject(post);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = client.PostAsync("api/Posts", byteContent).Result;
                string pla = await result.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<List<UserPost>>(pla);
                var u = obj;
                return PartialView("GetPosts", obj);
            }
        }
   
        [HttpGet]
        public async Task<ActionResult> GetInfo()
        {
           
                return PartialView(getUser());

        }
        
        [HttpGet]
        public async Task<ActionResult> UpdateInfo()
        {
            return PartialView(getUser());
        }

        [HttpPut]
        public async Task<ActionResult> UpdateInfo(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = client.PutAsJsonAsync("api/Users/" + int.Parse(Session["user_id"].ToString()), user).Result;
                string pla = await result.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<User>(pla);
                var u = obj;
                return PartialView("GetInfo", obj);
            }
        } public async Task<ActionResult> UpdatePic()
        {
            return PartialView(getUser());
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePic(User user, HttpPostedFileBase uploadFile)
        {
            if (uploadFile != null)
            {
                string pic = System.IO.Path.GetFileName(uploadFile.FileName);
                using (MemoryStream ms = new MemoryStream())
                {
                    uploadFile.InputStream.CopyTo(ms);
                    user.profile_image = ms.GetBuffer();
                }

            }
                using (var client = new HttpClient())
            {
                var myContent = JsonConvert.SerializeObject(user);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = client.PutAsync("api/Users/" + int.Parse(Session["user_id"].ToString()), byteContent).Result;
                string pla = await result.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<User>(pla);
                var u = obj;
                //return PartialView("GetInfo", obj);
                return RedirectToAction("Index", "Profile");
            }
        }
        public async Task<ActionResult> GetFriends()
        {
            List<User> users = new List<User>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllPostloyees using HttpClient  
                HttpResponseMessage Res = client.GetAsync("api/Users/Friends/" + int.Parse(Session["user_id"].ToString())).Result;

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var PostResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Postloyee list  
                    users = JsonConvert.DeserializeObject<List<User>>(PostResponse);

                }
                return PartialView(users);
            }

        }

        public User getUser()
        {
            User user = new User();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllPostloyees using HttpClient  
                HttpResponseMessage Res = client.GetAsync("api/Users/" + int.Parse(Session["user_id"].ToString())).Result;
                int x = int.Parse(Session["user_id"].ToString());
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var PostResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Postloyee list  
                    user = JsonConvert.DeserializeObject<User>(PostResponse);

                }
                return user;
            }
        }

    }
}
