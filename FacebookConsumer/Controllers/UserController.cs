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

    }
}
