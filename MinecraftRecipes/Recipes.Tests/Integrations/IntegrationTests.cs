using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Recipes.Core.Models;
using Recipes.Data.Models;
using Xunit;

namespace Recipes.Tests.Integrations
{
    public class IntegrationTests : IClassFixture<RecipesWebApplicationFactory>
    {
        public IntegrationTests(RecipesWebApplicationFactory factory)
        {
            _factory = factory;
            _context = RecipesWebApplicationFactory.Context;
        }

        private readonly RecipesWebApplicationFactory _factory;
        private readonly RecipesContext _context;

        [Theory]
        [InlineData("/api/items")]
        public async Task Get_ItemsController(string url)
        {
            var client = _factory.CreateClient();
            var data = await Helpers.NetworkRequestAsync<IEnumerable<Item>>(client.GetAsync(url));
            data.Should().BeEquivalentTo(_context.Items);
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
                data.CreatedFrom.Should().BeEquivalentTo(_context.Recipes
                    .Where(r => r.Result.Id == id));
                data.IsPartOf.Should().BeEquivalentTo(_context.PatternKeys
                    .Where(pk => pk.Item.Id == id)
                    .Include(pk => pk.Recipe)
                    .Select(pk => pk.Recipe));
            }
        }
    }
}