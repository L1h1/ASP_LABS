using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_LABS.Domain.Entities
{
    public class Genre
    {
        public Genre() { }
        public Genre(string name)
        {
            Name = name;
            NormalizedName = name.ToLower().Replace(' ', '-');
		}
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; protected set; /*required for EF*/  }

        public List<Book>? Books { get; set; }
    }
}
