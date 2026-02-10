using Microsoft.AspNetCore.Mvc;
using SportsStore.Models; // Needed for Cart

namespace SportsStore.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart cart;

        public CartSummaryViewComponent(Cart cart)
        {
            this.cart = cart;
        }

        public IViewComponentResult Invoke() => this.View(this.cart);
    }
}
