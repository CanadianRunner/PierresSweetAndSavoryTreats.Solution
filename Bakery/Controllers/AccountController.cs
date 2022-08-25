using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Bakery.Models;
using System.Threading.Tasks;
using Bakery.ViewModels;

namespace Bakery.Controllers
{
  public class AccountController : Controller
  {
    private readonly BakeryContext _db;
    private readonly UserIdTracker<UserId> _userIdTracker;
    private readonly UserSignIn<UserId> _userSignIn;

    public AccountController(UserIdTracker<UserId> UserIdTracker, UserSignIn<UserId> userSignIn, BakeryContext db)
    {
      _userIdTracker = userIdTracker;
      _userSignIn = userSignIn;
      _db = db;
    }

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
      var appUser = new UserId { UserName = model.Email}
      IdentityResult result = await _userIdTracker.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        {
          return View();
        }
      }
    }

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
      if(result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        return View();
      }
    }

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }
  }
}
