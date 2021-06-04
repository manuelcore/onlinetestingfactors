using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using AplicatieWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace AplicatieWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<AplicatieUtilizator> _userManager;

        public RegisterConfirmationModel(UserManager<AplicatieUtilizator> userManager)
        {
            _userManager = userManager;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Nu s-a putut gasi utilizatorul cu adresa de email '{email}'.");
            }

            Email = email;
            //Once you add a real email sender, you should remove this code that lets you confirm the account
           //DisplayConfirmAccountLink = true;
           // if (DisplayConfirmAccountLink)
           // {
                //var userId = await _userManager.GetUserIdAsync(user);
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //EmailConfirmationUrl = Url.Page(
                //    "/Account/ConfirmEmail",
                //    pageHandler: null,
                //    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                //    protocol: Request.Scheme);

            //return RedirectToPage("/ConfirmEmail");
            //}

            return Page();
        }
    }
}
