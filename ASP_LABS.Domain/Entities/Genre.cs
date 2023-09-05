using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ASP_LABS.Domain.Entities
{
    public class Genre
    {


        private string _name;
        
        public int Id { get; set; }
        public string? Name { get 
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string? NormalizedName { get
            {
                return _name==null?"": _name.ToLower().Replace(' ', '-'); 
            }

            protected set { } /*required for EF*/  }

    }
}
