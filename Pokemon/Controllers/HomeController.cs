using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Pokemon.Controllers
{
    public class HomeController : Controller
    {

        Entities db = new Entities();
         
        public ActionResult Index()
        {
   
            return View();
        }

        
    }
}