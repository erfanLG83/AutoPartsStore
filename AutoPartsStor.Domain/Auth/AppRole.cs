using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Domain.Auth
{
    public class AppRole:IdentityRole
    {
        public string Description { get; set; }
    }
}
