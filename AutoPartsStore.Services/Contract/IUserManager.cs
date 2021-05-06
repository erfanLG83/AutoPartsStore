using AutoPartsStore.Domain.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutoPartsStore.Services.Contract
{
    public interface IAppUserManager
    {
        #region BaseClass
        IPasswordHasher<AppUser> PasswordHasher { get; set; }
        IList<IUserValidator<AppUser>> UserValidators { get; }
        IList<IPasswordValidator<AppUser>> PasswordValidators { get; }
        ILookupNormalizer KeyNormalizer { get; set; }
        IdentityErrorDescriber ErrorDescriber { get; set; }
        IdentityOptions Options { get; set; }
        bool SupportsUserAuthenticationTokens { get; }
        bool SupportsUserAuthenticatorKey { get; }
        bool SupportsUserTwoFactorRecoveryCodes { get; }
        bool SupportsUserTwoFactor { get; }
        bool SupportsUserPassword { get; }
        bool SupportsUserSecurityStamp { get; }
        bool SupportsUserRole { get; }
        bool SupportsUserLogin { get; }
        bool SupportsUserEmail { get; }
        bool SupportsUserPhoneNumber { get; }
        bool SupportsUserClaim { get; }
        bool SupportsUserLockout { get; }
        bool SupportsQueryableUsers { get; }
        IQueryable<AppUser> Users { get; }
        Task<string> GenerateConcurrencyStampAsync(AppUser user);
        Task<IdentityResult> CreateAsync(AppUser user);
        Task<IdentityResult> UpdateAsync(AppUser user);
        Task<IdentityResult> DeleteAsync(AppUser user);
        Task<AppUser> FindByIdAsync(string userId);
        Task<AppUser> FindByNameAsync(string userName);
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        string NormalizeKey(string key);
        Task UpdateNormalizedUserNameAsync(AppUser user);
        Task<string> GetUserNameAsync(AppUser user);
        Task<IdentityResult> SetUserNameAsync(AppUser user, string userName);
        Task<string> GetUserIdAsync(AppUser user);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<bool> HasPasswordAsync(AppUser user);
        Task<IdentityResult> AddPasswordAsync(AppUser user, string password);
        Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);
        Task<IdentityResult> RemovePasswordAsync(AppUser user);
        Task<string> GetSecurityStampAsync(AppUser user);
        Task<IdentityResult> UpdateSecurityStampAsync(AppUser user);
        Task<string> GeneratePasswordResetTokenAsync(AppUser user);
        Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string newPassword);
        Task<AppUser> FindByLoginAsync(string loginProvider, string providerKey);
        Task<IdentityResult> RemoveLoginAsync(AppUser user, string loginProvider, string providerKey);
        Task<IdentityResult> AddLoginAsync(AppUser user, UserLoginInfo login);
        Task<IList<UserLoginInfo>> GetLoginsAsync(AppUser user);
        Task<IdentityResult> AddClaimAsync(AppUser user, Claim claim);
        Task<IdentityResult> AddClaimsAsync(AppUser user, IEnumerable<Claim> claims);
        Task<IdentityResult> ReplaceClaimAsync(AppUser user, Claim claim, Claim newClaim);
        Task<IdentityResult> RemoveClaimAsync(AppUser user, Claim claim);
        Task<IdentityResult> RemoveClaimsAsync(AppUser user, IEnumerable<Claim> claims);
        Task<IList<Claim>> GetClaimsAsync(AppUser user);
        Task<IdentityResult> AddToRoleAsync(AppUser user, string role);
        Task<IdentityResult> AddToRolesAsync(AppUser user, IEnumerable<string> roles);
        Task<IdentityResult> RemoveFromRoleAsync(AppUser user, string role);
        Task<IdentityResult> RemoveFromRolesAsync(AppUser user, IEnumerable<string> roles);
        Task<IList<string>> GetRolesAsync(AppUser user);
        Task<bool> IsInRoleAsync(AppUser user, string role);
        Task<string> GetEmailAsync(AppUser user);
        Task<IdentityResult> SetEmailAsync(AppUser user, string email);
        Task<AppUser> FindByEmailAsync(string email);
        Task UpdateNormalizedEmailAsync(AppUser user);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
        Task<IdentityResult> ConfirmEmailAsync(AppUser user, string token);
        Task<bool> IsEmailConfirmedAsync(AppUser user);
        Task<string> GenerateChangeEmailTokenAsync(AppUser user, string newEmail);
        Task<IdentityResult> ChangeEmailAsync(AppUser user, string newEmail, string token);
        Task<string> GetPhoneNumberAsync(AppUser user);
        Task<IdentityResult> SetPhoneNumberAsync(AppUser user, string phoneNumber);
        Task<IdentityResult> ChangePhoneNumberAsync(AppUser user, string phoneNumber, string token);
        Task<bool> IsPhoneNumberConfirmedAsync(AppUser user);
        Task<string> GenerateChangePhoneNumberTokenAsync(AppUser user, string phoneNumber);
        Task<bool> VerifyChangePhoneNumberTokenAsync(AppUser user, string token, string phoneNumber);
        Task<bool> VerifyUserTokenAsync(AppUser user, string tokenProvider, string purpose, string token);
        Task<string> GenerateUserTokenAsync(AppUser user, string tokenProvider, string purpose);
        void RegisterTokenProvider(string providerName, IUserTwoFactorTokenProvider<AppUser> provider);
        Task<IList<string>> GetValidTwoFactorProvidersAsync(AppUser user);
        Task<bool> VerifyTwoFactorTokenAsync(AppUser user, string tokenProvider, string token);
        Task<string> GenerateTwoFactorTokenAsync(AppUser user, string tokenProvider);
        Task<bool> GetTwoFactorEnabledAsync(AppUser user);
        Task<IdentityResult> SetTwoFactorEnabledAsync(AppUser user, bool enabled);
        Task<bool> IsLockedOutAsync(AppUser user);
        Task<IdentityResult> SetLockoutEnabledAsync(AppUser user, bool enabled);
        Task<bool> GetLockoutEnabledAsync(AppUser user);
        Task<DateTimeOffset?> GetLockoutEndDateAsync(AppUser user);
        Task<IdentityResult> SetLockoutEndDateAsync(AppUser user, DateTimeOffset? lockoutEnd);
        Task<IdentityResult> AccessFailedAsync(AppUser user);
        Task<IdentityResult> ResetAccessFailedCountAsync(AppUser user);
        Task<int> GetAccessFailedCountAsync(AppUser user);
        Task<IList<AppUser>> GetUsersForClaimAsync(Claim claim);
        Task<IList<AppUser>> GetUsersInRoleAsync(string roleName);
        Task<string> GetAuthenticationTokenAsync(AppUser user, string loginProvider, string tokenName);
        Task<IdentityResult> SetAuthenticationTokenAsync(AppUser user, string loginProvider, string tokenName, string tokenValue);
        Task<IdentityResult> RemoveAuthenticationTokenAsync(AppUser user, string loginProvider, string tokenName);
        Task<string> GetAuthenticatorKeyAsync(AppUser user);
        Task<IdentityResult> ResetAuthenticatorKeyAsync(AppUser user);
        string GenerateNewAuthenticatorKey();
        Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(AppUser user, int number);
        Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(AppUser user, string code);
        Task<int> CountRecoveryCodesAsync(AppUser user);
        Task<byte[]> CreateSecurityTokenAsync(AppUser user);

        #endregion

        #region Custome Methods
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> FindUserByNameAsync(string userName);
        Task<string> GetFullNameAsync(ClaimsPrincipal claims);
        Task<AppUser> GetUserAsync(ClaimsPrincipal claims);
        #endregion
    }
}
