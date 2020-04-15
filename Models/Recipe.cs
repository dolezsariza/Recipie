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
        [NotMappedAttribute]
        public List<Ingredient> Ingredients { get; set; }
        public int Energy { get; set; }
        public int Fat { get; set; }
        public int Carbohydrate { get; set; }
        public int Sugar { get; set; }
        public int Protein { get; set; }
        public int Salt { get; set; }
        public List<RecipeIngredient> RecipeIngredients { get; set; }
        public DateTime Date { get; set; }

        public Recipe(string name, string description, string ownerId)
        {
            Name = name;
            Description = description;
            OwnerId = ownerId;
        }
    }
}
