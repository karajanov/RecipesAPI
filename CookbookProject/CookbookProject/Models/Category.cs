﻿using System.Collections.Generic;

namespace CookbookProject.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual IEnumerable<Recipe> Recipes { get; set; }
    }
}
