using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using AplicatieWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net.Mime;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AplicatieWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AplicatieUtilizator> _signInManager;
        private readonly UserManager<AplicatieUtilizator> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<AplicatieUtilizator> userManager,
            SignInManager<AplicatieUtilizator> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Nume")]
            public string Nume { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Prenume")]
            public string Prenume { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} trebuie sa contina intre {2} si {1} caractere.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Parola")]
            public string Parola { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmare parola")]
            [Compare("Parola", ErrorMessage = "Parolele nu coincid.")]
            public string ConfirmareParola{ get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Cod administrator (optional)")]
            public string CodAdmin { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new AplicatieUtilizator { UserName = Input.Email, Email = Input.Email, Nume=Input.Nume, Prenume=Input.Prenume };
                var result = await _userManager.CreateAsync(user, Input.Parola);
                if(Input.CodAdmin=="9999")
                    await _userManager.AddToRoleAsync(user, "Administrator");
                else
                    await _userManager.AddToRoleAsync(user, "Student");
                if (result.Succeeded)
                {
                    _logger.LogInformation("A fost creat un nou cont");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    var client = new SendGridClient("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");
                    var msg = new SendGridMessage();

                    msg.SetFrom(new EmailAddress("testeGrila258@gmail.com", ""));

                    var recipient =
                        new EmailAddress(Input.Email);
                    msg.AddTo(recipient);

                    msg.SetSubject("Confirmare adresa de e-mail");

                    msg.AddContent(MimeType.Text, "Click pentru confirmare adresa de e-mail: ");
                    msg.AddContent(MimeType.Html, "<a href=" + callbackUrl + ">Click pentru confirmare e-mail</a>");
                    var response = await client.SendEmailAsync(msg);


                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
