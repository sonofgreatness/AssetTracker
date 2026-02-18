using AssetLocater.Domain.Models;
using AssetLocater.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace AssetLocater.Controllers
{
    

        public class SearchController : Controller
        {
            [HttpGet]
            public IActionResult Index()
            {
                return View(new SearchTerms());
            }

            [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SearchTerms model)
            {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

              
                return View(model);
            }


            // 🔍 Perform search later
            return View(model);
            }
    }
}
