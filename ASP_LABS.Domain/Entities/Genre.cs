using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_LABS.Domain.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get { return Name.ToLower().Replace(' ', '-'); } }

        public List<Book> Books { get; set; }
    }
}
