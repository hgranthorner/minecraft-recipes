using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Data.Models;

namespace Recipes.API.Controllers
{
    [Route("api/[controller]")]
    public class RecipesController : Controller
    {
        private readonly RecipesContext _context;

        public RecipesController(RecipesContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<OkObjectResult> GetRecipes() => Ok(await _context.Recipes.Select(r => new
        {
            r.Id,
            r.Name
        }).ToListAsync());

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetRecipeInfo(int id)
        {
            var recipe = await _context.Recipes
                .Where(r => r.Id == id)
                .Include(r => r.Result)
                .Include(r => r.PatternKeys)
                .ThenInclude(pk => pk.Item)
                .Select(x => new
                {
                    RecipeName = x.Name,
                    RecipeId = x.Id,
                    x.Pattern,
                    x.Result,
                    PatternInformation = x.PatternKeys.Select(pk => new
                    {
                        pk.Character,
                        ItemName = pk.Item.Name,
                        ItemId = pk.Item.Id
                    })
                })
                .FirstOrDefaultAsync();
            return Ok(recipe);
        }
    }
}