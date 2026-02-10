using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.Repository;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IStoreRepository repository;

        public NavigationMenuViewComponent(IStoreRepository repo)
        {
            this.repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            this.ViewBag.SelectedCategory = this.RouteData?.Values["category"];

            var categories = this.repository.Products
                                       .Select(x => x.Category)
                                       .Distinct()
                                       .OrderBy(x => x);

            return this.View(categories);
        }
    }
}
