using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Bakery.Controllers
{
  public class FlavoursController : Controller
  {
    private readonly BakeryContext _db;
    private readonly UserIdTracker<UserId> _userIdTracker;

    public FlavoursController(UserIdTracker<UserId> UserIdTracker, BakeryContext db)
    {
      _userIdTracker = userIdTracker;
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      return View(_db.Flavours.ToList());
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Flavour flavour)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      flavour.User = currentUser;
      _db.Flavours.Add(flavour);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      var thisFlavour = _db.Flavours
      .Include(flavour => flavour.JoinEntities)
      .ThenInclude(join => join.Flavour)
      .FirstOrDefault(flavour => flavour.FlavourId == id);
      return View(thisFlavour);
    }
    public ActionResult Edit(int id)
    {
      var thisFlavour = _db.Flavours.FirstOrDefault(flavour => flavour.FlavourId == id);
      return View(thisFlavour);
    }

    [HttpPost]
    public ActionResult Edit(Flavour flavour)
    {
      _db.Entry(flavour).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisFlavour = _db.Flavours.FirstOrDefault(flavour => flavour.FlavourId == id);
      return View(thisFlavour);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisFlavour = _db.Flavours.FirstOrDefault(flavour => flavour.FlavourId == id);
      _db.Treats.Remove(thisFlavour);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

      public ActionResult AddTreat(int id)
    {
      var thisFlavour = _db.Treats.FirstOrDefault(flavour => flavour.FlavourId == id);
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatsId", "Name");
      return View(thisFlavour);
    }

    [HttpPost]
    public ActionResult AddTreat(Flavour flavour, int TreatsId)
    {
      if(TreatsId != 0)
      {
         _db.FlavourTreat.Add(new FlavourTreat() { TreatsId = TreatsId, FlavourId = flavourId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

     [HttpPost]
    public ActionResult DeleteFlavour(int joinId)
    {
      var joinEntry = _db.FlavourTreat.FirstOrDefault(entry => entry.FlavourTreatId == joinId);
      _db.FlavourTreat.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}