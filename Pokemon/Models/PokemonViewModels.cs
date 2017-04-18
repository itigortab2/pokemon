using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Pokemon.Models
{
    public class PokemonViewModels
    {
        public int PokemonId { get; set; }
        public string title { get; set; }

        public virtual ICollection<PType> PType { get; set; }

        public PokemonViewModels() {
            this.PType = new HashSet<PType>();
        }

    }
}