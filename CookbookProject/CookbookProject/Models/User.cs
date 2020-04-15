using System.Collections.Generic;

namespace CookbookProject.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public byte[] Password { get; set; }

        public virtual IEnumerable<Recipe> Recipes { get; set; }
    }
}
