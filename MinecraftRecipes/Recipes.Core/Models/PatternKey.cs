namespace Recipes.Core.Models
{
    public class PatternKey
    {
        public int Id { get; set; }
        public string Character { get; set; }

        public Item Item { get; set; }
        public int ItemId { get; set; }
        public Recipe Recipe { get; set; }
        public int RecipeId { get; set; }
    }
}