using AutoPartsStore.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPartsStore.Infrastructure.Admin.UsersManager
{
    public class UserDetailsViewModel
    {
        public UserDetailsViewModel(AppUser user,IEnumerable<string> roles)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Image = user.ImageName;
            Roles = roles;
            LockoutEnabled = user.LockoutEnabled;
            EmailConfirmed = user.EmailConfirmed;
            AccessFailedCount = user.AccessFailedCount;
            LockoutEnd = user.LockoutEnd;
            Address = user.Address;
            FullName = user.FirstName + " " + user.LastName;
            PostalCode = user.PostalCode;
        }
        public string TotalBuys { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string PostalCode { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool EmailConfirmed { get; set; }

        public int AccessFailedCount { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
