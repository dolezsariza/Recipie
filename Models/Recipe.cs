using Recipie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Domain.Models
{
    public class Recipe
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey(nameof(User))]
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        [NotMappedAttribute]
        public List<Ingredient> Ingredients { get; set; }
        public int Energy { get; set; }
        public float Fat { get; set; }
        public float Carbohydrate { get; set; }
        public float Sugar { get; set; }
        public float Protein { get; set; }
        public float Salt { get; set; }
        public List<RecipeIngredient> RecipeIngredients { get; set; }

        //[ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        //[ForeignKey(nameof(SubCategory))]
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public List<RecipeTag> RecipeTags { get; set; }
        [NotMapped]
        public List<Tag> Tags { get; set; }
        
        public DateTime Date { get; set; }

        public Recipe(string name, string description, string ownerId)
        {
            Name = name;
            Description = description;
            OwnerId = ownerId;
        }
    }
}
