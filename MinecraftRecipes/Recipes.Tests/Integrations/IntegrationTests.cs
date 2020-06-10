using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Core.Models;
using Recipes.Data.Models;
using Xunit;

namespace Recipes.Tests.Integrations
{
    public class IntegrationTests : IClassFixture<RecipesWebApplicationFactory>
    {
        private readonly RecipesWebApplicationFactory _factory;
        private readonly RecipesContext _context;

        public IntegrationTests(RecipesWebApplicationFactory factory)
        {
            _factory = factory;
            _context = RecipesWebApplicationFactory.CreateDatabase();
        }

        [Theory]
        [InlineData("/api/items")]
        public async Task Get_ItemsController(string url)
        {
            var client = _factory.CreateClient();
            var data = await Helpers.NetworkRequestAsync<IEnumerable<Item>>(client.GetAsync(url));
            Assert.Equal(_context.Items.Count(), data.Count());
        }

        [Fact]
        public async Task GetById_ItemsController()
        {
            var client = _factory.CreateClient();
            const string urlBase = "/api/items/";
            foreach (var id in _context.Items.Select(i => i.Id))
            {
                var url = urlBase + id;
                var data = await Helpers.NetworkRequestAsync<RecipesForItem>(client.GetAsync(url));
                Assert.Equal(
                    _context.Recipes
                        .Where(r => r.Result.Id == id)
                        .Select(x => x.Id),
                    data.CreatedFrom.Select(r => r.Id));
                Assert.Equal(
                    _context.PatternKeys
                        .Where(pk => pk.Item.Id == id)
                        .Select(pk => pk.Recipe.Id),
                    data.IsPartOf.Select(r => r.Id));
            }
        }
    }
}