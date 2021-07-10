using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace cisep.Controllers
{
    public class WebSiteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}