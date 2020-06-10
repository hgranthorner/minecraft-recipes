using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Recipes.API.Controllers;
using Xunit;

namespace Recipes.Tests.Units
{
    public class ItemControllerUnits : IClassFixture<RecipesSeedDataFixture>
    {
        public ItemControllerUnits(RecipesSeedDataFixture fixture)
        {
            _fixture = fixture;
            _controller = new ItemsController(_fixture.Context);
        }

        private readonly ItemsController _controller;
        private readonly RecipesSeedDataFixture _fixture;

        [Fact]
        public async Task Can_Get_Items()
        {
            var items = await _controller.GetItems();
            var contextItems = await _fixture.Context.Items.ToListAsync();
            Assert.Equal(contextItems, items);
        }

        [Fact]
        public async Task Can_Get_Recipe_Info_For_Item_Id()
        {
            foreach (var id in _fixture.Context.Items.Select(i => i.Id))
            {
                var data = await _controller.GetRecipesForItem(id);
                Assert.Equal(
                    _fixture.Context.Recipes.Where(r => r.Result.Id == id),
                    data.CreatedFrom);
                Assert.Equal(
                    _fixture.Context.PatternKeys.Count(pk => pk.Item.Id == id),
                    data.IsPartOf.Count);
            }
        }
    }
}