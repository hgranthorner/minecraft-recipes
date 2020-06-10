using Recipes.API.Controllers;
using Xunit;

namespace Recipes.Tests.Units
{
    public class RecipesControllerUnits : IClassFixture<RecipesSeedDataFixture>
    {
        private readonly RecipesController _controller;
        private readonly RecipesSeedDataFixture _fixture;

        public RecipesControllerUnits(RecipesSeedDataFixture fixture)
        {
            _fixture = fixture;
            _controller = new RecipesController(_fixture.Context);
        }
    }
}