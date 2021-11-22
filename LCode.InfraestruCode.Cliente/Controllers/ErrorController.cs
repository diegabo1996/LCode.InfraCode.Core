using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCode.InfraestruCode.Cliente.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/404")]
        public IActionResult Error404()
        {
            return View();
        }
        [Route("/Error/500")]
        public IActionResult Error500()
        {
            return View();
        }
        [Route("/Error/401")]
        public IActionResult Error401()
        {
            return View();
        }
    }
}
