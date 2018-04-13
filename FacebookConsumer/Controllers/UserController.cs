using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FacebookConsumer.Models.FaceBook;
using FacebookConsumer.Models.ViewModels;
using Microsoft.Ajax.Utilities;

namespace FacebookConsumer.Controllers
{
    public class UserController : Controller
    {
       public HttpClient UserClient = new HttpClient();

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
        public  ActionResult Login(LoginViewModel login)
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
                        else
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
            catch(Exception exception)
            {
                Console.WriteLine(exception);
            }
            
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
            HttpResponseMessage response1 =  UserClient.GetAsync("api/User_Friends/" +u.user_id).Result;
            if (response.IsSuccessStatusCode)
            {
                List<User_Friends> Friends =  response1.Content.ReadAsAsync<List<User_Friends>>().Result;
                foreach (var item in Friends)
                {
                    if (item.user_id == id)
                        return View("FriendProfile", u);

                }

            }
            return View("NonFriendProfile",u);
        }

        //GET:Search About User
        //user/search/had
        [HttpGet]
        public async Task<ActionResult>Search(String Searchstr)
        {
            HttpResponseMessage response = await UserClient.GetAsync("api/users/" + Searchstr);
            if (response.IsSuccessStatusCode)
            {
                List<User> users = await response.Content.ReadAsAsync<List<User>>();
                if(users.Count==0)
                    return View("NoResult");
                else
                    return View("SearchResult", users);
            }
            else
                return View("NoResult");

        }

        //user/unfriend/2
        public ActionResult Unfriend(int id)
        {
            HttpResponseMessage res = UserClient.DeleteAsync("api/user_friends/"+u.user_id+"/"+id).Result;
            return View("Index");
        }

        //user/addfriend/2
        public ActionResult Addfriend(int id)
        {
                      
            User_Friends userFriend = new User_Friends { user_id = u.user_id, user_friend_id = id, request = false };
            HttpResponseMessage response =  UserClient.PostAsJsonAsync("api/user_friends/",userFriend).Result;
            return View("Index");
        }

        //user/block/1
        public ActionResult Block(int id)
        {
            Blocked_Users BlockedUser = new Blocked_Users { user_id = u.user_id, user_block_id = id };
            HttpResponseMessage response = UserClient.PostAsJsonAsync("api/blocked_user/", BlockedUser).Result;
            return View("Index");
        }


    }
}
