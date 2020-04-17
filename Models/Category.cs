using Recipie.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<Recipe> Recipes { get; set; }
        public Category() { }
        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
