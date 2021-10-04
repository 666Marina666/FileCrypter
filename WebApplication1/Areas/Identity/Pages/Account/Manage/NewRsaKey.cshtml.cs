using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApplication1.Controllers;
using WebApplication1.Models;

namespace WebApplication1.Areas.Identity.Pages.Account.Manage
{
    public class NewRsaKey : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly RsaParametersController _rsaParametersManager;
        

        public NewRsaKey(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger, RsaParametersController rsaParametersManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _rsaParametersManager = rsaParametersManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Public key")]
            public string PublicKey { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Private key")]
            public string PrivateKey { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            user.RsaParameters.Add(new RsaParameters()
            {
                PrivateKey = Input.PrivateKey,
                PublicKey = Input.PublicKey
            });

            _logger.LogInformation("User changed added rsa key pair to collection.");
            StatusMessage = "New key pair has added.";

            return RedirectToPage();
        }
    }
}
