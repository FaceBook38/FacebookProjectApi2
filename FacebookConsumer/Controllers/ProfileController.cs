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
                HttpResponseMessage Res = await client.GetAsync("api/Users/Posts/"+1);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var PostResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Postloyee list  
                    PostInfo = JsonConvert.DeserializeObject<List<UserPost>>(PostResponse);

                }
                //returning the Postloyee list to view  
                return View(PostInfo);
            }
        }
    }
}