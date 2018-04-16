using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FacebookConsumer.Models.FaceBook;
using FacebookConsumer.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace FacebookConsumer.Controllers
{
    public class UserController : Controller
    {
       public HttpClient UserClient = new HttpClient();
      string baseURL = "http://localhost:54555";

        public UserController()
        {
            //port number is not static
            UserClient.BaseAddress = new Uri("http://localhost:54555");
        }

        // GET: Login Page User
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            try
            {
                // TODO: Add insert logic here
                if (LoginUser(login).IsSuccessStatusCode)
                    return RedirectToAction("Profile");
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseURL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("api/users/"+id).Result;
            if (response.IsSuccessStatusCode)
            {
                var users = response.Content.ReadAsAsync<User>().Result;
                return View(users);
            }

            return View (new User());


        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            try
            {
                // TODO: Add insert logic here
                if (RegisterUser(user).IsSuccessStatusCode)
                    return RedirectToAction("Login");
                else
                {
                    return RedirectToAction("Create");
                }
            }
            catch
            {
                return View();
            }
        }


        

        // GET: User/Edit/5
        public  ActionResult EditAsync(int id)
        {
        //  User user = new    User();
          List<User> user = new   List<User>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                 HttpResponseMessage   Response =client.GetAsync("api/Users/"+id).Result;
                if (Response.IsSuccessStatusCode)
                {
                  
                var users = Response.Content.ReadAsAsync<User>().Result;

                    return View(users);

                }
            }
            return View(new User());
        }

        // POST: User/Edit/5
        [HttpPost]
        public async Task<ActionResult> EditAsync(int id, User newUser)
        {
                using (var client = new HttpClient())
                {
                    newUser.deleted = false;
                    newUser.user_type = "user";
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Response = await client.PutAsJsonAsync("api/Users/"+id, newUser);
                    if (Response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("EditAsync");
                    }
                    return View("nothing");
                }
            

             
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
         HttpResponseMessage RegisterUser(User user)
        {
            HttpResponseMessage response =  UserClient.PostAsJsonAsync("/api/users", user).Result; 
            response.EnsureSuccessStatusCode();
            // return URI of the created resource.
            return response;
        }
        HttpResponseMessage LoginUser(LoginViewModel login)
        {
            HttpResponseMessage response = UserClient.PostAsJsonAsync("/api/users/login", login).Result;
            response.EnsureSuccessStatusCode();
            // return URI of the created resource.
            return response;
        }

    }
}
