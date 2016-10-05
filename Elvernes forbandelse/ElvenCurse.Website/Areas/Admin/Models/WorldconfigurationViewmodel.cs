using System.Collections.Generic;
using ElvenCurse.Core.Model;

namespace ElvenCurse.Website.Areas.Admin.Models
{
    public class WorldconfigurationViewmodel
    {
        public IEnumerable<Worldsection> Worldsections { get; set; }
        public IEnumerable<Terrainfile> Terrains { get; set; }
    }
}