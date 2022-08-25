using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.UseAuthorization;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Bakery.Controllers
{
  public class TreatsController : Controller
  {
    private readonly BakeryContext _db;
    private readonly UserIdTracker<UserId> _userIdTracker;

    public TreatsController(UserIdTracker<UserId> UserIdTracker, BakeryContext db)
    {
      _userIdTracker = userIdTracker;
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      return View(_db.Treats.ToList());
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Treat treat, int FlavourId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      treat.User = currentUser;
      _db.Treats.Add(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      var thisTreat = _db.Treats
      .Include(treat => treat.JoinEntities)
      .ThenInclude(join => join.Flavour)
      .FirstOrDefault(treat => treat.TreatsId == id);
      return View(thisTreat);
    }
    public ActionResult Edit(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatsId == id);
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Entry(treat).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatsId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatsId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

      public ActionResult AddFlavour(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatsId == id);
      ViewBag.FlavourId = new SelectList(_db.Flavours, "FlavourId", "Name");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddFlavour(Treat treat, int FlavourId)
    {
      if(FlavourId != 0)
      {
         _db.FlavourTreat.Add(new FlavourTreat() { FlavourId = FlavourId, TreatsId = treat.TreatsId});
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