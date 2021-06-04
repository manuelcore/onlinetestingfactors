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
    public class Disable2faModel : PageModel
    {
        private readonly UserManager<AplicatieUtilizator> _userManager;
        private readonly ILogger<Disable2faModel> _logger;

        public Disable2faModel(
            UserManager<AplicatieUtilizator> userManager,
            ILogger<Disable2faModel> logger)
        {
            _userManager = userManager;
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

            if (!await _userManager.GetTwoFactorEnabledAsync(user))
            {
                throw new InvalidOperationException($"Nu se poate dezactiva 2FA pentru utilizatorul cu ID '{_userManager.GetUserId(User)}' deoarece nu este activata");
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

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new InvalidOperationException($"Eroare neasteptata in timpul incercarii de dezactivare 2FA pentru utilizatorul cu ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("Utilizatorul cu ID '{UserId}' si-a dezactivat 2FA", _userManager.GetUserId(User));
            StatusMessage = "2FA a fost dezactivata. O puteti reactiva la setarea unei aplicatii de autentificare.";
            return RedirectToPage("./TwoFactorAuthentication");
        }
    }
}