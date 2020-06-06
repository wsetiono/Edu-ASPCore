using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}