using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AplicatieWeb.Models;
using Microsoft.AspNetCore.Authorization;
using AplicatieWeb.Controllers;

namespace AplicatieWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Rapoarte()
        {
            return View();
        }

        public IActionResult Administrare()
        {
            return View();
        }

        public IActionResult Note()
        {
            return View();
        }

        [HttpGet]
        [Route("/Testul1")]
        public ActionResult Testul1()
        {
            return View();
        }
        [HttpGet]
        [Route("/Testul2")]
        public ActionResult Testul2()
        {
            return View();
        }
        [HttpGet]
        [Route("/Testul3")]
        public ActionResult Testul3()
        {
            return View();
        }
        [HttpGet]
        [Route("/Testul4")]
        public ActionResult Testul4()
        {
            return View();
        }
    }
}
