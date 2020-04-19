﻿using System;
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
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
    }
}
