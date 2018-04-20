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
                var usrResult = LoginUser(login);


                // TODO: Add insert logic here
                if (usrResult.IsSuccessStatusCode)
                {
                    var userLogged = usrResult.Content.ReadAsAsync<User>().Result;
                    if (userLogged != null)
                    {
                        Session["user_id"] = userLogged.user_id;
                        Session["user_type"] = userLogged.user_type;
                        if (userLogged.user_type == "user")
                        {
                            return RedirectToAction("Profile");
                        }
                        else //if the user is admin 
                        {
                            //Admin page
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return View();
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



        //GET:View Friend or not Friend Profile 
        //user/ViewUserProfile/3
        [HttpGet]
        public  ActionResult ViewUserProfile(int id)
        {
            HttpResponseMessage response =  UserClient.GetAsync("api/users/" +id).Result;
            User u =  response.Content.ReadAsAsync<User>().Result;

            int myid =(int)Session["user_id"];
            HttpResponseMessage response1 = UserClient.GetAsync("api/users/"+myid).Result;
            User me = response1.Content.ReadAsAsync<User>().Result;

            if (response.IsSuccessStatusCode && response1.IsSuccessStatusCode)
            {
                
                foreach (var item in me.User_Friends)
                {
                    if (item.user_friend_id == id)
                        return View("FriendProfile", u);

                }

            }
            
            return View("NonFriendProfile",u);
        }

        //GET:Search About User
        //user/search/had
        [HttpGet]
        public  ActionResult Search(String Searchstr)
        {
            //bool flag = false;

            HttpResponseMessage response = UserClient.GetAsync($"api/users/{(int)Session["user_id"]}/" + Searchstr).Result;
            
            if (response.IsSuccessStatusCode)
            {
                List<User> users =  response.Content.ReadAsAsync<List<User>>().Result;

                HttpResponseMessage MyResponse = UserClient.GetAsync($"api/users/{Session["user_id"]}").Result;
                User me = MyResponse.Content.ReadAsAsync<User>().Result;
                List<User> Unblockeduser = new List<User>();
                foreach (var item in users)
                {
                    Blocked_Users blockeduser = me.Blocked_Users.FirstOrDefault(p => p.user_block_id == item.user_id);
                    if (blockeduser == null)
                    {
                        Unblockeduser.Add(item);
                        
                    }

                }
                if(Unblockeduser.Count<=0)
                    return View("NoResult");
                else
                    return View("SearchResult", Unblockeduser);
  
            }
            else
                return View("NoResult");

        }

        //user/unfriend/2
        public ActionResult UnFriend(int id)
        {
            try
            {
                HttpResponseMessage response = UserClient.GetAsync($"api/users/{Session["user_id"]}").Result;
                User me = response.Content.ReadAsAsync<User>().Result;
               HttpResponseMessage DeleteResponse = UserClient.DeleteAsync($"api/user_friends/{(int)Session["user_id"]}/{id}").Result;
                if (DeleteResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                    return View("error");

                

            }
            catch (Exception)
            {

                return View("error");
                
            }
            
 
        }

        //user/addfriend/2

        public ActionResult AddFriend(int id)
        {
            try
            {
                HttpResponseMessage response = UserClient.GetAsync($"api/users/{Session["user_id"]}").Result;
                User me = response.Content.ReadAsAsync<User>().Result;

                HttpResponseMessage FriendResponse = UserClient.GetAsync("api/users/" + id).Result;
                User Friend = FriendResponse.Content.ReadAsAsync<User>().Result;

                if(response.IsSuccessStatusCode)
                {                   
                    User_Friends MeAsUserFriend = new User_Friends { user_id = (int)Session["user_id"], user_friend_id = id,request=false };
                    //Add Relation Between Users In User_Friends Table
                    HttpResponseMessage Postresponse = UserClient.PostAsJsonAsync($"api/user_friends",MeAsUserFriend).Result;
                }
                else
                {
                    return View("AddFriendError");
                }


                if(FriendResponse.IsSuccessStatusCode)
                {
                    User_Friends FriendAsUserFriend = new User_Friends { user_id = id, user_friend_id = (int)Session["user_id"],request=false  };

                    //Add Relation Between Users In User_Friends Table
                    HttpResponseMessage Postresponse1 = UserClient.PostAsJsonAsync($"api/user_friends", FriendAsUserFriend).Result;
                }
                else
                {
                    return View("AddFriendError");
                }

                return View("Index");

                //return RedirectToAction("ViewUserProfile", "User", id);

            }
            catch (Exception)
            {

                return View("Error");
            }
        }

        //user/block/1
        public ActionResult Block(int id)
        {
            HttpResponseMessage MYResponse = UserClient.GetAsync($"api/users/{Session["user_id"]}").Result;

            if (MYResponse.IsSuccessStatusCode)
            {
                User Me = MYResponse.Content.ReadAsAsync<User>().Result;
                Blocked_Users blocked_user= new Blocked_Users { user_id = (int)Session["user_id"], user_block_id = id};
                HttpResponseMessage res = UserClient.PostAsJsonAsync($"api/blocked_users",blocked_user).Result;
                if(res.IsSuccessStatusCode)
                {
                    foreach (var item in Me.User_Friends)
                    {
                        if(item.user_friend_id==id)
                        {
                            HttpResponseMessage DeleteUserFriend = UserClient.DeleteAsync($"api/user_friends/{(int)Session["user_id"]},{id}").Result;
                        }

                    }
                    return View();
                }
                else
                    return View("Error");
            }
            else
                return View("Error");

        }
    }
}
