using AutoPartsStore.Domain.Auth;
using AutoPartsStore.Infrastructure.Admin.UsersManager;
using AutoPartsStore.Persistence;
using AutoPartsStore.Services.Contract;
using AutoPartsStore.Services.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPartsStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAppUserManager _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IFileWorker _fileWorker;
        public UsersManagerController(IAppUserManager userManager, RoleManager<AppRole> roleManager, IFileWorker fileWorker, ApplicationDbContext context)
        {
            _fileWorker = fileWorker;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        private async Task SetRoles()
        {
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
        }
        public async Task<IActionResult> Index(int index = 1, int row = 5)
        {
            int count = 0;
            var allUsers = await _userManager.GetUsersAsync();
            allUsers = allUsers.Where(n=>!n.IsDeleted);
            IEnumerable<AppUser> users = Pagination.GetData<AppUser>(allUsers, ref count, row, index);
            int pageCount = count % row == 0 ? count / row : (count / row) + 1;
            ViewBag.Pagination = new Pagination(Url, pageCount, index, row, action: "Index", controller: "UsersManager");
            int rowCounter = row * (index - 1);
            List<UserViewModel> model = new List<UserViewModel>();
            foreach (var item in users)
            {
                model.Add(new UserViewModel(item, await _userManager.GetRolesAsync(item), ++rowCounter));
            }
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            await SetRoles();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDTO userCreate)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    Address = userCreate.Address,
                    Email = userCreate.Email,
                    PostalCode = userCreate.PostalCode,
                    UserName = userCreate.Username,
                    FirstName = userCreate.FirstName,
                    PhoneNumber = userCreate.PhoneNumber,
                    EmailConfirmed = true,
                    LastName = userCreate.LastName,
                };
                if (userCreate.ImageFile != null)
                    user.ImageName = await _fileWorker.AddFileToPathAsync(userCreate.ImageFile, "img");
                var result = await _userManager.CreateAsync(user, userCreate.Password);
                if (result.Succeeded)
                {
                    string[] roles = userCreate.UserRoles.Split(',');
                    if (roles.Any())
                        await _userManager.AddToRolesAsync(user, roles);
                    return LocalRedirect("/admin/usersmanager");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            await SetRoles();
            return View(userCreate);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            await SetRoles();
            return View(new UserEditDto(
                user: user,
                roles: await _userManager.GetRolesAsync(user)
                ));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditDto userEdit)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userEdit.Id);
                if (user == null)
                    return NotFound();
                #region Update User Properties
                user.LastName = userEdit.LastName;
                user.UserName = userEdit.Username;
                user.LockoutEnabled = userEdit.IsLockout;
                user.PhoneNumber = userEdit.PhoneNumber;
                user.EmailConfirmed = userEdit.EmailConfirmed;
                user.FirstName = userEdit.FirstName;
                user.PostalCode = userEdit.PostalCode;
                user.Address = userEdit.Address;
                if (userEdit.ImageFile != null)
                {
                    if (user.ImageName != null)
                        _fileWorker.RemoveFileInPath("/img/" + user.ImageName);
                    user.ImageName = await _fileWorker.AddFileToPathAsync(userEdit.ImageFile, "img");
                }
                #endregion
                #region Upadte User Roles
                var roles = userEdit.UserRoles.Split(',');
                var recentRoles = await _userManager.GetRolesAsync(user);
                var deletedRoles = recentRoles.Except(roles).ToArray();
                var addedRoles = roles.Except(recentRoles).ToArray();
                if (deletedRoles.Length != 0)
                {
                    await _userManager.RemoveFromRolesAsync(user, deletedRoles);
                }
                if (addedRoles.Length != 0)
                {
                    await _userManager.AddToRolesAsync(user, addedRoles);
                }
                #endregion
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return LocalRedirect("/admin/usersmanager");
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            await SetRoles();
            return View(userEdit);
        }
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var model = new UserDetailsViewModel(user, await _userManager.GetRolesAsync(user));
            model.TotalBuys = _context.Orders.Where(n => n.UserId == user.Id).Sum(n => n.TotalPrice).ToString();
            return View(model);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return Json(new { Success = false });
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);
            return Json(new { Success = true });
        }
    }
}