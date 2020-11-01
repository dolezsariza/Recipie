using Recipie.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipie.Domain.Models
{
    public class Ingredient
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Energy { get; set; }
        public int Fat { get; set; }
        public int Carbohydrate { get; set; }
        public int Sugar { get; set; }
        public int Protein { get; set; }
        public int Salt { get; set; }
        public List<RecipeIngredient> RecipeIngredients { get; set; }

        public Ingredient() { }
        public Ingredient(string name, string description)
        {
            Name = name;
            Description = description;
        }

    }
}