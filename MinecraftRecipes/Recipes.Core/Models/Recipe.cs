using System.Collections.Generic;

namespace Recipes.Core.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
        public string Pattern { get; set; }
        public List<PatternKey> PatternKeys { get; set; } = new List<PatternKey>();
        public Item Result { get; set; }
        public int ResultId { get; set; }
        public int ResultCount { get; set; }
    }
}