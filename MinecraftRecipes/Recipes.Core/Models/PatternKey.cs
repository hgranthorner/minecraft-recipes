namespace Recipes.Core.Models
{
    public class PatternKey
    {
        public int Id { get; set; }
        public string Character { get; set; }

        public Item Item { get; set; }
        public Recipe Recipe { get; set; }
    }
}