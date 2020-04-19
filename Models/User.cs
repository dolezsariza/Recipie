using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipie.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipie.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Introduction { get; set; }
        public string ProfilePicture { get; set; }
        [NotMappedAttribute]
        public List<Recipe> Recipes { get; set; }
        public string RoleName { get; set; }
    }
}
