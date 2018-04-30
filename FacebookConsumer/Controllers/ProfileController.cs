using System;
using System.Collections.Generic;
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
                HttpResponseMessage Res = client.GetAsync("api/Users/Posts/" + 1).Result;

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
            post.user_id = 1;
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
                return PartialView("GetPosts", obj);
            }
        }
   
        public async Task<ActionResult> GetInfo()
        {
            User user = new User();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllPostloyees using HttpClient  
                HttpResponseMessage Res = client.GetAsync("api/Users/" + 1).Result;

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var PostResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Postloyee list  
                    user = JsonConvert.DeserializeObject<User>(PostResponse);

                }
                return PartialView(user);
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
                HttpResponseMessage Res = client.GetAsync("api/Users/Friends/" + 1).Result;

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

    }
}
