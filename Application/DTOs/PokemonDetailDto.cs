using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PokemonDetailDto
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public string Abilities { get; set; }
        public string Type { get; set; }
    }
}
