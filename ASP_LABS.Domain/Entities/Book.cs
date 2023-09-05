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
        public string? Description { get;set; }
       
        public Genre? Genre { get; set; }
        public int GenreId { get; set; }

        public double Price { get; set; }

        public string? ImagePath { get; set; }


    }
}
