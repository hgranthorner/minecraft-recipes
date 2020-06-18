using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Recipes.API.Controllers;
using Xunit;

namespace Recipes.Tests.Units
{
    public class RecipesControllerUnits : IClassFixture<RecipesSeedDataFixture>
    {
        public RecipesControllerUnits(RecipesSeedDataFixture fixture)
        {
            fixture.SeedDatabase(nameof(RecipesControllerUnits));
            _fixture = fixture;
            _controller = new RecipesController(_fixture.Context);
        }

        private readonly RecipesController _controller;
        private readonly RecipesSeedDataFixture _fixture;

        [Fact]
        public async Task Gets_All_Recipes()
        {
            var recipes = await _fixture.Context.Recipes
                .AsNoTracking()
                .Select(r =>
                    new
                    {
                        r.Id, r.Name
                    })
                .ToListAsync();
            var actuals = (await _controller.GetRecipes())
                .ToList();
            actuals.Should().BeEquivalentTo(recipes);
        }
    }
}