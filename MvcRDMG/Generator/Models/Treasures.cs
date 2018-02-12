using System.Collections.Generic;

namespace MvcRDMG.Generator.Models
{
    public class Treasures
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Rarity { get; set; }
        public bool Magical { get; set; }
        public List<string> Types { get; set; }
    }
}