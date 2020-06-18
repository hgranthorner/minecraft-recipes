using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Recipes.API.Controllers;
using Xunit;

namespace Recipes.Tests.Units
{
    public class ItemControllerUnits : IClassFixture<RecipesSeedDataFixture>
    {
        private readonly ItemsController _controller;
        private readonly RecipesSeedDataFixture _fixture;

        public ItemControllerUnits(RecipesSeedDataFixture fixture)
        {
            fixture.SeedDatabase(nameof(ItemControllerUnits));
            _fixture = fixture;
            _controller = new ItemsController(_fixture.Context);
        }

        [Fact]
        public async Task Can_Get_Items()
        {
            var items = await _controller.GetItems();
            var fixtureItems = await _fixture.Context.Items.ToListAsync();

            items.Should().BeEquivalentTo(fixtureItems);
        }

        [Fact]
        public async Task Can_Get_Recipe_Info_For_Item_Id()
        {
            foreach (var id in _fixture.Context.Items.Select(i => i.Id))
            {
                var data = await _controller.GetRecipesForItem(id);
                var fixtureData = await _fixture.Context.Recipes.Where(r => r.Result.Id == id).ToListAsync();
                data.CreatedFrom.Should().BeEquivalentTo(fixtureData);
                Assert.Equal(
                    _fixture.Context.PatternKeys.Count(pk => pk.Item.Id == id),
                    data.IsPartOf.Count);
            }
        }
    }
}