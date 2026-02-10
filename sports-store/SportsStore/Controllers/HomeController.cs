using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.Repository;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        // Public members come first
        public int PageSize { get; private set; } = 4;

        // Private readonly fields
        private readonly IStoreRepository repository;

        // Constructor
        public HomeController(IStoreRepository repo)
        {
            this.repository = repo;
        }

        // Index action
        public ViewResult Index(string? category, int productPage = 1)
        {
            var products = this.repository.Products
                                         .Where(p => category == null || p.Category == category)
                                         .OrderBy(p => p.ProductId)
                                         .Skip((productPage - 1) * this.PageSize)
                                         .Take(this.PageSize);

            var pagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = this.PageSize,
                TotalItems = category == null
                             ? this.repository.Products.Count()
                             : this.repository.Products.Count(p => p.Category == category),
            };

            return this.View(new ProductsListViewModel
            {
                Products = products,
                PagingInfo = pagingInfo,
                CurrentCategory = category,
            });
        }

        // Error action
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View();
        }
    }
}
