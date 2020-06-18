using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Core.Models;
using Recipes.Data.Models;

namespace Recipes.API.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private readonly RecipesContext _context;

        public ItemsController(RecipesContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<List<Item>> GetItems() => await _context.Items.AsNoTracking().ToListAsync();

        [HttpGet("{id}")]
        public async Task<RecipesForItem> GetRecipesForItem(int id) => new RecipesForItem(
            await _context.Recipes
                .Where(r => r.Result.Id == id)
                .AsNoTracking()
                .ToListAsync(),
            await _context.PatternKeys
                .Where(pk => pk.Item.Id == id)
                .Include(pk => pk.Recipe)
                .AsNoTracking()
                .Select(pk => pk.Recipe)
                .ToListAsync());
    }
}