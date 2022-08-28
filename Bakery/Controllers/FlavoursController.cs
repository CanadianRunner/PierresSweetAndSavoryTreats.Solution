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
    private readonly UserManager<ApplicationUser> _userManager;

    public FlavoursController(UserManager<ApplicationUser> userManager, BakeryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      return View(_db.Flavours.ToList());
    }

    [Authorize]
    public ActionResult Create()
    {
      return View();
    }

    [Authorize]
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

    [Authorize]
    public ActionResult Edit(int id)
    {
      var thisFlavour = _db.Flavours.FirstOrDefault(flavour => flavour.FlavourId == id);
      return View(thisFlavour);
    }
    [Authorize]
    [HttpPost]
    public ActionResult Edit(Flavour flavour)
    {
      _db.Entry(flavour).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
    public ActionResult Delete(int id)
    {
      var thisFlavour = _db.Flavours.FirstOrDefault(flavour => flavour.FlavourId == id);
      return View(thisFlavour);
    }
    
    [Authorize]
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisFlavour = _db.Flavours.FirstOrDefault(flavour => flavour.FlavourId == id);
      _db.Flavours.Remove(thisFlavour);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
    public ActionResult AddTreat(int id)
    {
      var thisFlavour = _db.Flavours.FirstOrDefault(flavour => flavour.FlavourId == id);
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      return View(thisFlavour);
    }

    [Authorize]
    [HttpPost]
    public ActionResult AddTreat(Flavour flavour, int TreatId)
    {
      if(TreatId != 0)
      {
         _db.FlavourTreat.Add(new FlavourTreat() { TreatId = TreatId, FlavourId = flavour.FlavourId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
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