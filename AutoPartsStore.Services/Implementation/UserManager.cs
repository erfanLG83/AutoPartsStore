using AutoPartsStore.Domain.Auth;
using AutoPartsStore.Services.Contract;
using AutoPartsStore.Services.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutoPartsStore.Services.Implementation
{
    public class AppUserManager : UserManager<AppUser>, IAppUserManager
    {
        private readonly AppIdentityErrorDescriber _error;
        private readonly ILookupNormalizer _keyNoramalizer;
        private readonly ILogger<AppUserManager> _logger;
        private readonly IOptions<IdentityOptions> _options;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IEnumerable<IPasswordValidator<AppUser>> _passwordValidators;
        private readonly IServiceProvider _service;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IEnumerable<IUserValidator<AppUser>> _userValidators;
        public AppUserManager(
            IUserStore<AppUser> userStore,
            IServiceProvider service,
            IEnumerable<IUserValidator<AppUser>> userValidators,
            IEnumerable<IPasswordValidator<AppUser>> passwordValidators,
            ILogger<AppUserManager> logger,
            AppIdentityErrorDescriber error,
            ILookupNormalizer keyNoramalizer,
            IPasswordHasher<AppUser> passwordHasher,
            IOptions<IdentityOptions> options

            )


            : base(userStore, options, passwordHasher, userValidators, passwordValidators, keyNoramalizer, error, service, logger)
        {
            _error = error;
            _keyNoramalizer = keyNoramalizer;
            _logger = logger;
            _options = options;
            _passwordHasher = passwordHasher;
            _service = service;
            _passwordValidators = passwordValidators;
            _userStore = userStore;
            _userValidators = userValidators;
        }


        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await Users.ToListAsync();
        }
        public async Task<AppUser> FindUserByNameAsync(string userName)
        {
            return await Users.FirstAsync(n => n.NormalizedUserName == userName.ToUpper());
        }
        public async Task<string> GetFullNameAsync(ClaimsPrincipal claims)
        {
            AppUser user = await GetUserAsync(claims);
            return user.UserName;
        }
        public async Task<AppUser> GetUserAsync(ClaimsPrincipal claims)
        {
            return await Users.FirstAsync(n => n.Id == claims.FindFirstValue(ClaimTypes.Name));
        }

        public string NormalizeKey(string key)
        {
            return key.ToUpper();
        }
    }
}
