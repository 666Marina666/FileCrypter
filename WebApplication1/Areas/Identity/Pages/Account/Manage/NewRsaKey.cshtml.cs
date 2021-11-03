using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Implemetations;

namespace WebApplication1.Areas.Identity.Pages.Account.Manage
{
    public class NewRsaKey : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly IKeyServices _keyServices;
        

        public NewRsaKey(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger, 
            IKeyServices keyServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _keyServices = keyServices;
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
            public string KeyPairName { get; set; }
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

            var userName = HttpContext.User.Identity.Name;
            var newKey = _keyServices.Create(userName, Input.KeyPairName);

            _logger.LogInformation("User add rsa key pair to collection.");
            StatusMessage = $"New key {newKey.PairName} pair has added.";

            return RedirectToPage();
        }
    }
}
