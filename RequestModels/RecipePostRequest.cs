using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.RequestModels
{
    public class RecipePostRequest
    {
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public string Carbohydrate { get; set; }
        public string Energy { get; set; }
        public string Fat { get; set; }
        public string Protein { get; set; }
        public string Salt { get; set; }
        public string Sugar { get; set; }
        public DateTime Date { get; set; }
        public RecipePostRequest()
        {
            Date = DateTime.Now;
        }
    }
}
