using ApplyInk.Common.Enum;
using ApplyInk.Common;
using ApplyInk.Web.Data;
using ApplyInk.Web.Data.Entities;
using ApplyInk.Web.Helpers;
using ApplyInk.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ApplyInk.Common.Responses;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace Iglesia.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        //private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;


        public AccountController(
            DataContext context,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IMailHelper mailHelper
            /*IBlobHelper blobHelper*/)

        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
           // _blobHelper = blobHelper;
            _mailHelper = mailHelper;

        }


       

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email or password incorrect.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Countries = _combosHelper.GetComboCountries(),
                Categories = _combosHelper.GetComboCategories(),
                Departments = _combosHelper.GetComboDepartments(0),
                Cities = _combosHelper.GetComboCities(0),
                Shops = _combosHelper.GetComboShops(0)

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                /* if (model.ImageFile != null)
                 {
                     imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                 }*/

                var usertype = (model.userType.Equals("tattoer")) ? UserType.Tattooer : UserType.User;

                User user = await _userHelper.AddUserAsync(model, imageId, usertype);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    model.Countries = _combosHelper.GetComboCountries();
                    model.Categories = _combosHelper.GetComboCategories();
                    model.Departments = _combosHelper.GetComboDepartments(model.CountryId);
                    model.Cities = _combosHelper.GetComboCities(model.DepartmentId);
                    model.Shops = _combosHelper.GetComboShops(model.CityId);
                    return View(model);
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
                if (response.IsSuccess)
                {
                    ViewBag.Message = "The instructions to allow your user has been sent to email.";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }


            model.Countries = _combosHelper.GetComboCountries();
            model.Categories = _combosHelper.GetComboCategories();
            model.Departments = _combosHelper.GetComboDepartments(model.CountryId);
            model.Cities = _combosHelper.GetComboCities(model.DepartmentId);
            model.Shops = _combosHelper.GetComboShops(model.CityId);
            return View(model);
        }


        public JsonResult GetDepartments(int countryId)
        {
            Country country = _context.Countries
                .Include(c => c.Departments)
                .FirstOrDefault(c => c.Id == countryId);
            if (country == null)
            {
                return null;
            }

            return Json(country.Departments.OrderBy(d => d.Name));
        }

        public JsonResult GetCities(int departmentId)
        {
            Department department = _context.Departments
                .Include(d => d.Cities)
                .FirstOrDefault(d => d.Id == departmentId);
            if (department == null)
            {
                return null;
            }

            return Json(department.Cities.OrderBy(c => c.Name));
        }


        public JsonResult GetShops(int cityId)
        {
            City city = _context.Cities
                .Include(d => d.Shops)
                .FirstOrDefault(d => d.Id == cityId);
            if (city == null)
            {
                return null;
            }

            return Json(city.Shops.OrderBy(c => c.Name));
        }


        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

       [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                string link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                _mailHelper.SendMail(model.Email, "Password Reset", $"<h1>Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");
                ViewBag.Message = "The instructions to recover your password has been sent to email.";
                return View();

            }

            return View(model);
        }
        public IActionResult NotAuthorized()
        {

            return View();
        }



        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                IdentityResult result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Password reset successful.";
                    return View();
                }

                ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            ViewBag.Message = "User not found.";
            return View(model);
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexAdmin()
        {
            return View(await _context.Users
                  .Where(u => u.UserType == UserType.Admin)
                  .ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexTattooer()
        {
            return View(await _context.Users
                  .Where(u => u.UserType == UserType.Tattooer)
                  .Include(c => c.Categories)
                  .Include(s => s.Shop)
                  .ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexUser()
        {
            return View(await _context.Users
                  .Where(u => u.UserType == UserType.User)
                  .ToListAsync());
        }

        /* public async Task<IActionResult> ChangeUser()
         {
             User user = await _userHelper.GetUserAsync(User.Identity.Name);
             District district = new District();
             Region region = new Region();

             if (user == null)
             {
                 return NotFound();
             }

             if (!User.IsInRole(UserType.Admin.ToString()))
             {
                 district = await _context.Districts.FirstOrDefaultAsync(d => d.Churches.FirstOrDefault(c => c.Id == user.Church.Id) != null);
                 if (district == null)
                 {
                     district = await _context.Districts.FirstOrDefaultAsync();
                 }

                 region = await _context.Regions.FirstOrDefaultAsync(c => c.Districts.FirstOrDefault(d => d.Id == district.Id) != null);
                 if (region == null)
                 {
                     region = await _context.Regions.FirstOrDefaultAsync();
                 }
             }

             EditUserViewModel model = new EditUserViewModel
             {
                 Address = user.Address,
                 FirstName = user.FirstName,
                 LastName = user.LastName,
                 PhoneNumber = user.PhoneNumber,
                 ImageId = user.ImageId,
                 Regions = _combosHelper.GetComboRegions(),
                 Professions = _combosHelper.GetComboProfessions(),
                 Id = user.Id,
                 Document = user.Document
             };

             if (User.IsInRole(UserType.Admin.ToString()))
             {
                 model.Districts = _combosHelper.GetComboDistricts(0);
                 model.Churches = _combosHelper.GetComboChurches(0);
             }
             else
             {
                 model.Churches = _combosHelper.GetComboChurches(district.Id);
                 model.ChurchId = user.Church.Id;
                 model.RegionId = region.Id;
                 model.DistrictId = district.Id;
                 model.Districts = _combosHelper.GetComboDistricts(region.Id);
                 model.ProfessionID = user.Profession.Id;
             }

             return View(model);
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> ChangeUser(EditUserViewModel model)
         {
             if (ModelState.IsValid)
             {
                 Guid imageId = model.ImageId;

                 if (model.ImageFile != null)
                 {
                     imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                 }

                 User user = await _userHelper.GetUserAsync(User.Identity.Name);

                 user.FirstName = model.FirstName;
                 user.LastName = model.LastName;
                 user.Address = model.Address;
                 user.PhoneNumber = model.PhoneNumber;
                 user.ImageId = imageId;
                 user.Church = await _context.Churches.FindAsync(model.ChurchId);
                 user.Profession = await _context.Professions.FindAsync(model.ProfessionID);
                 user.Document = model.Document;

                 await _userHelper.UpdateUserAsync(user);
                 return RedirectToAction("Index", "Home");
             }

             model.Professions = _combosHelper.GetComboProfessions();
             model.Regions = _combosHelper.GetComboRegions();
             model.Districts = _combosHelper.GetComboDistricts(model.RegionId);
             model.Churches = _combosHelper.GetComboChurches(model.DistrictId);
             return View(model);
         }
        */
         public IActionResult ChangePassword()
         {
             return View();
         }

         [HttpPost]
         public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
         {
             if (ModelState.IsValid)
             {
                 var user = await _userHelper.GetUserAsync(User.Identity.Name);
                 if (user != null)
                 {
                     var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                     if (result.Succeeded)
                     {
                         return RedirectToAction("ChangeUser");
                     }
                     else
                     {
                         ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                     }
                 }
                 else
                 {
                     ModelState.AddModelError(string.Empty, "User no found.");
                 }
             }

             return View(model);
         }


    }

}
