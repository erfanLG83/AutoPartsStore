using AutoPartsStore.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPartsStore.Infrastructure.Admin.UsersManager
{
    public class UserViewModel
    {
        public UserViewModel()
        {

        }
        public UserViewModel(AppUser user , IEnumerable<string> roles , int row)
        {
            Id = user.Id;
            Row = row;
            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Roles = "";
            foreach (var item in roles)
            {
                Roles +=  item + ",";
            }
            if (Roles.Length > 0)
            {
                Roles = Roles.Remove(Roles.Length - 1, 1);
            }
            Image = user.ImageName;
        }
        public int Row { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public string Image { get; set; }
    }
}
