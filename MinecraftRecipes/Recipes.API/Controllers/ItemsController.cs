using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<Item>> GetItems() => await _context.Items.ToListAsync();

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetRecipesForItem(int id) => Ok(new
        {
            CreatedFrom = await _context.Recipes
                .Where(r => r.Result.Id == id)
                .ToListAsync(),
            IsPartOf = await _context.PatternKeys
                .Where(pk => pk.Item.Id == id)
                .Include(pk => pk.Recipe)
                .Select(pk => pk.Recipe)
                .ToListAsync()
        });
    }
}