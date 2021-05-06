using AutoPartsStore.Domain.Auth;
using AutoPartsStore.Infrastructure.Admin.UsersManager;
using AutoPartsStore.Services.Contract;
using AutoPartsStore.Services.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoPartsStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersManagerController : Controller
    {
        private readonly IAppUserManager _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IFileWorker _fileWorker;
        public UsersManagerController(IAppUserManager userManager, RoleManager<AppRole> roleManager, IFileWorker fileWorker)
        {
            _fileWorker = fileWorker;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        private async Task SetRoles()
        {
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
        }
        public async Task<IActionResult> Index(int index = 1, int row = 5)
        {
            int count = 0;
            IEnumerable<AppUser> users = Pagination.GetData<AppUser>(await _userManager.GetUsersAsync(), ref count, row, index);
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
                };
                var result = await _userManager.CreateAsync(user,userCreate.Password);
            }
            return View(userCreate);
        }
    }
}