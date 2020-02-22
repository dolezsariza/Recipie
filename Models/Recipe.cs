using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public List<Ingredient> Ingredients { get; set; }
        public int Energy { get; set; }
        public int Fat { get; set; }
        public int Carbohydrate { get; set; }
        public int Sugar { get; set; }
        public int Protein { get; set; }
        public int Salt { get; set; }
    }
}
