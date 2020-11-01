using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RecipeTag> RecipeTags { get; set; }
        public Tag(string name)
        {
            Name = name;
        }
        public Tag() { }
    }
}
