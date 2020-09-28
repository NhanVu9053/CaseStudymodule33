using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TiemSach.Models.EF;
using TiemSach.Models.UserModel;
using TiemSach.ViewModel.RoleViewModel;

namespace TiemSach.Controllers
{
    public class RolesManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDbContext context;

        public RolesManagerController(RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            this.roleManager = roleManager;
            this.context = context;
        }
        public IActionResult Index()
        {
            var roles = roleManager.Roles;
            var model = new List<Role>();
            model = roles.Select(r => new Role
            {
                RoleId = r.Id,
                Name = r.Name
            }).ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await roleManager.CreateAsync(new IdentityRole
                {
                    Name = model.Name
                });
                if (result.Succeeded)
                    return RedirectToAction("Index", "RolesManager");
                foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var model = new EditRoleViewModel
                {
                    RoleId = role.Id,
                    Name = role.Name
                };
                ViewBag.UsersCount = (from u in context.Users
                                      join r in context.UserRoles on u.Id equals r.UserId
                                      where r.RoleId == id
                                      select u).ToList().Count;
                return View(model);
            }

            return View("~/Views/Error/PageNotFound.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(model.RoleId);
                if (role != null)
                {
                    role.Name = model.Name;
                    var result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                        return RedirectToAction("Index", "RolesManager");
                    foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }
    }
}
