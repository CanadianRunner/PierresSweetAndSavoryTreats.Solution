using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bakery.Controllers
{
    public class HomeController : Controller
    {
      private readonly BakeryContext _db;
      public HomeController(BakeryContext db)
      {
        _db = db;
      }
      [HttpGet("/")]
      public ActionResult Index()
      {
        List<Treat> treatList = _db.Treats.ToList();
        ViewBag.Flavours = _db.Flavours.ToList();
        return View();
      }

    }
}