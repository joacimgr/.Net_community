using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DistroLabCommunity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace DistroLabCommunity.Controllers {
    public class HomeController : Controller {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserHandler _userHandler;
        private readonly IMessageHandler _messageHandler;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserHandler userHandler, IMessageHandler messageHandler) {
            _userManager = userManager;
            _signInManager = signInManager;
            _userHandler = userHandler;
            _messageHandler = messageHandler;
        }

        public IActionResult Index() {
            if (_signInManager.IsSignedIn(User)) {
                string id = _userManager.GetUserId(HttpContext.User);
                if (_userHandler.UserIDExists(id)) {
                    //Possibly store userID in session
                    if (_userHandler.AddUserLogin(_userManager.GetUserId(HttpContext.User))) {
                        return RedirectToAction("Welcome", "Community");
                    }
                    else {
                        TempData["userCreationInfo"] = "Unable to add timestamp";
                    }
                }
                return RedirectToAction("SetUserName");
            }
            else {
                return View();
            }
        }

        [Authorize]
        public IActionResult SetUserName() {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetUserName(string username) {
            if(Regex.IsMatch(username, @"^[a-zA-Z0-9]+$")){
                if (!_userHandler.UsernameExists(username)) {
                    if (_userHandler.AddUser(_userManager.GetUserId(HttpContext.User), username)) {
                        return RedirectToAction("Index");
                    }
                    else {
                        TempData["userCreationInfo"] = "Unable to create user";
                    }
                }
                else {
                    TempData["userCreationInfo"] = "Username not available";
                }
            }
            else {
                TempData["userCreationInfo"] = "Invalid characters";
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
