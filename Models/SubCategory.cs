using Recipie.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipie.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //[ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Recipe> Recipes { get; set; }

        public SubCategory() { }
        public SubCategory(string name, string description, int categoryId)
        {
            Name = name;
            Description = description;
            CategoryId = categoryId;
        }
    }
}