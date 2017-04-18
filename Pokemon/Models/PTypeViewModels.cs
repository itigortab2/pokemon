using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokemon.Models
{
    public class PTypeViewModels
    {

        public int idType { get; set; }
        public string type { get; set; }
        public PTypeViewModels()
        {
            this.Pokemon = new HashSet<Pokemon>();
        }


        public virtual ICollection<Pokemon> Pokemon { get; set; }

    }
}