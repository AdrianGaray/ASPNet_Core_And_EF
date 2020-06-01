using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebNetCore3.Areas.Principal.Controllers
{
    public class PrincipalController : Controller
    {
        [Area("Principal")]
        public IActionResult Index()
        {
            return View();
        }
    }
}