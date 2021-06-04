using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AplicatieWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AplicatieWeb.Areas.Identity.Pages.Account.Manage
{
    public class ResetAuthenticatorModel : PageModel
    {
        UserManager<AplicatieUtilizator> _userManager;
        private readonly SignInManager<AplicatieUtilizator> _signInManager;
        ILogger<ResetAuthenticatorModel> _logger;

        public ResetAuthenticatorModel(
            UserManager<AplicatieUtilizator> userManager,
            SignInManager<AplicatieUtilizator> signInManager,
            ILogger<ResetAuthenticatorModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nu a fost gasit utilizatorul cu ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nu a fost gasit utilizatorul cu ID '{_userManager.GetUserId(User)}'.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            _logger.LogInformation("Utilizatorul cu ID '{UserId}' si-a resetat cheia pentru aplicatia de autentificare.", user.Id);
            
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Cheia pentru aplicatia de autentificare a fost resetata, va trebui sa configurati aplicatia de autentificare folosind noua cheie.";

            return RedirectToPage("./EnableAuthenticator");
        }
    }
}