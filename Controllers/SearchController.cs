using AssetLocater.Domain.Models;
using AssetLocater.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;



namespace AssetLocater.Controllers
{
    

        public class SearchController : Controller
        {


        private readonly IVehicleSearchEngine _searchEngine;
        private readonly SearchResultStore _resultStore;


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
            return View("Processor", model);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Processor(SearchTerms model)
        {
            var result = _searchEngine.Execute(model);
            var key = _resultStore.Store(result);

            return RedirectToAction("Preview", new { key });
        }

        [HttpGet]
        public IActionResult Preview(string key)
        {
            var result = _resultStore.Get(key);
            if (result == null) return NotFound();

            return View(result);
        }



    }

}
