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
                client.BaseAddress = new Uri("http://localhost:54555/api/Groups");
                group.group_admin = 1;
                group.deleted = false;
                var postTask = client.PostAsJsonAsync<Group>("groups", group);
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
         
        }
    }
