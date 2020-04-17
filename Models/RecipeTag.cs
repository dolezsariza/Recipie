using Recipie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Models
{
    public class RecipeTag
    {
        public Recipe Recipe { get; set; }
        public int RecipeId { get; set; }
        public Tag Tag { get; set; }
        public int TagId { get; set; }
    }
}
