using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MonProjetAirplane.Controllers
{
    [Authorize] // Restreint l'accès uniquement aux utilisateurs connectés
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LogoutUser()
        {
            // Si vous utilisez Identity :
            // await _signInManager.SignOutAsync();
            
            // Redirection directe vers la page de connexion de l'Area Identity
            return Redirect("/Identity/Account/Login");
        }
    }

 
}