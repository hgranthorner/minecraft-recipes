using System.Collections.Generic;

namespace Recipes.Core.Models
{
    public class RecipesForItem
    {
        public RecipesForItem(List<Recipe> createdFrom, List<Recipe> isPartOf)
        {
            CreatedFrom = createdFrom;
            IsPartOf = isPartOf;
        }
        
        public List<Recipe> CreatedFrom { get; }
        public List<Recipe> IsPartOf { get; }

        public override string ToString()
        {
            return $"{{ CreatedFrom = {CreatedFrom}, IsPartOf = {IsPartOf} }}";
        }

        public override bool Equals(object value)
        {
            return (value is RecipesForItem type) &&
                   EqualityComparer<List<Recipe>>.Default.Equals(type.CreatedFrom, CreatedFrom) &&
                   EqualityComparer<List<Recipe>>.Default.Equals(type.IsPartOf, IsPartOf);
        }

        public override int GetHashCode()
        {
            int num = 0x7a2f0b42;
            num = (-1521134295 * num) + EqualityComparer<List<Recipe>>.Default.GetHashCode(CreatedFrom);
            return (-1521134295 * num) + EqualityComparer<List<Recipe>>.Default.GetHashCode(IsPartOf);
        }
    }
}