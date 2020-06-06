using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Education.Areas.Identity.Models;
using Education.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Education.ApiControllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;


        public UsersController(SignInManager<AppUser> signInManager,
          UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<string> Post(UserCreate model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Email
                };
                IdentityResult result
                    = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //return JsonConvert.SerializeObject("Register successfully");

                    UserData userData = new UserData();
                    userData.Name = user.Name;
                    userData.Email = user.Email;
                    userData.Image = Education.Helper.Helper.GetGravatarUrl(user.Email);
                    return JsonConvert.SerializeObject(userData);
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
            }

            return JsonConvert.SerializeObject("Register failed");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<string> Post2(UserLogin model)
        {
           
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    //return JsonConvert.SerializeObject("Login successfully");

                    var user = await _userManager.FindByEmailAsync(model.Email);
                    UserData userData = new UserData();
                    userData.Name = user.Name;
                    userData.Email = user.Email;
                    userData.Image = Education.Helper.Helper.GetGravatarUrl(user.Email);
                    return JsonConvert.SerializeObject(userData);

                }
            }

            return JsonConvert.SerializeObject("Login failed");

        }

        [HttpPost]
        [Route("Logout")]
        public async Task<string> Post3()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return JsonConvert.SerializeObject("Logout successfully");

            }
            catch (Exception ex)
            {

            }

            return JsonConvert.SerializeObject("Logout failed");

        }

    }
}
