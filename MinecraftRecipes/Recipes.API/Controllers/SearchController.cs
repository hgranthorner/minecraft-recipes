using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Recipes.Data.Models;

namespace Recipes.API.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private readonly RecipesContext _context;

        public SearchController(RecipesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IQueryable GetRecipes(string searchQuery)
        {
            var normalizedQuery = searchQuery.ToLower().Replace(" ", "");
            var recipes = _context.Recipes
                .Select(r => new
                {
                    Recipe = r,
                    Name = r.Name.ToLower().Replace(" ", "")
                })
                .Where(x =>
                    x.Name.Contains(normalizedQuery) ||
                    normalizedQuery.Contains(x.Name));
            return recipes.Select(r => new
            {
                r.Recipe.Id,
                r.Recipe.Name
            });
        }
    }
}