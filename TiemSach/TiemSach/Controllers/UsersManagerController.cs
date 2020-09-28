using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TiemSach.Models.UserModel;
using TiemSach.ViewModel.UserViewModel;

namespace TiemSach.Controllers
{
    public class UsersManagerController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersManagerController(UserManager<User> userManager, IWebHostEnvironment webHostEnvironment,
            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var users = userManager.Users;
            if (users != null && users.Any())
            {
                var model = new List<UserViewModel>();
                model = users.Select(u => new UserViewModel
                {
                    UserId = u.Id,
                    Address = u.Address,
                    Email = u.Email,
                    Gender = u.Gender,
                    Name = u.Name,
                    ProfilePicture = u.ProfilePicture
                }).ToList();
                foreach (var user in model) user.RoleName = GetRolesName(user.UserId);
                return View(model);
            }

            return View();
        }
        public string GetRolesName(string userId)
        {
            var user = Task.Run(async () => await userManager.FindByIdAsync(userId)).Result;
            var roles = Task.Run(async () => await userManager.GetRolesAsync(user)).Result;
            return roles != null ? string.Join(", ", roles) : string.Empty;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = roleManager.Roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Address = model.Address,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    IsDeleted = false
                };
                if (model.ImageFile != null)
                {
                    var uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images\\users");
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.ImageFile.FileName)}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using var fs = new FileStream(filePath, FileMode.Create);
                    model.ImageFile.CopyTo(fs);
                    user.ProfilePicture = fileName;
                }

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.RoleId))
                    {
                        var role = await roleManager.FindByIdAsync(model.RoleId);
                        var addRoleResult = await userManager.AddToRoleAsync(user, role.Name);
                        if (addRoleResult.Succeeded) return RedirectToAction("Index", "UsersManager");
                        foreach (var error in addRoleResult.Errors) ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var model = new EditUserViewModel
                {
                    Address = user.Address,
                    UserId = user.Id,
                    Email = user.Email,
                    Gender = user.Gender,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    ProfilePicture = user.ProfilePicture
                };
                var rolesName = await userManager.GetRolesAsync(user);
                if (rolesName != null && rolesName.Any())
                {
                    var role = await roleManager.FindByNameAsync(rolesName.FirstOrDefault());
                    model.RoleId = role.Id;
                }

                ViewBag.Roles = roleManager.Roles;
                return View(model);
            }

            return View("~/Views/Error/PageNotFound.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.Address = model.Address;
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.Gender = model.Gender;
                    user.Name = model.Name;
                    user.PhoneNumber = model.PhoneNumber;
                    user.ProfilePicture = model.ProfilePicture;
                    user.Id = model.UserId;

                    if (model.ImageFile != null)
                    {
                        var uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images\\users");
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.ImageFile.FileName)}";
                        var filePath = Path.Combine(uploadFolder, fileName);
                        using var fs = new FileStream(filePath, FileMode.Create);
                        model.ImageFile.CopyTo(fs);

                        user.ProfilePicture = fileName;
                        if (!string.IsNullOrEmpty(model.ProfilePicture))
                        {
                            var delFile = Path.Combine(webHostEnvironment.WebRootPath, "images\\users",
                                model.ProfilePicture);
                            System.IO.File.Delete(delFile);
                        }
                    }

                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var rolesName = await userManager.GetRolesAsync(user);
                        var delRoles = await userManager.RemoveFromRolesAsync(user, rolesName);

                        if (!string.IsNullOrEmpty(model.RoleId))
                        {
                            var role = await roleManager.FindByIdAsync(model.RoleId);
                            var addRoleResult = await userManager.AddToRoleAsync(user, role.Name);
                            if (addRoleResult.Succeeded) return RedirectToAction("Index", "UsersManager");
                            foreach (var error in addRoleResult.Errors) ModelState.AddModelError("", error.Description);
                        }

                        return RedirectToAction("Index", "UsersManager");
                    }

                    foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        public async Task<IActionResult> Remove(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded) return RedirectToAction("Index", "UsersManager");
                foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            }

            return View();
        }
    }
}
