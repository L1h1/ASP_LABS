using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_LABS.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get
            {
                if (Genres == null) return "...";
                var result = "";
                foreach(var genre in Genres)
                {   
                    result += $" {genre.Name}";
                }
                return result;
            }
        }
        public List<Genre>? Genres { get; set; }

        public double Price { get; set; }

        public string? ImagePath { get; set; }


    }
}
