using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Chipper.Models;
using System.Text;
using Chipper.Controllers.Parsing;
using PagedList;

namespace Chipper.Controllers
{
    public class HomeController : Controller
    {
        private static List<Chip> chips;
        private int pageSize = 15;

        [HttpGet]
        public ActionResult StartPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StartPage(string request)
        {
            
            chips = ParsingFactory.GetAllChips(request);
            return View("RequestedChipsPage", chips.ToPagedList(1, pageSize));          
        }

        [HttpPost]
        public ActionResult RequestedChipsPage(string request)
        {
            chips = ParsingFactory.GetAllChips(request);
            return View("RequestedChipsPage",chips.ToPagedList(1, pageSize));
        }

        public ActionResult RequestedChipsPage(int? page)
        {            
            int pageNumber = (page ?? 1);
            return View( chips.ToPagedList(pageNumber, pageSize));
        }
    }
}