using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.Repository;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IStoreRepository repository;

        public Cart Cart { get; set; }

        public CartController(IStoreRepository repository, Cart cart)
        {
            this.repository = repository;
            this.Cart = cart;
        }

        // GET: /Cart
        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            return this.View(new CartViewModel
            {
                Cart = this.Cart,
                ReturnUrl = returnUrl ?? "/",
            });
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public IActionResult AddToCart(long productId, string returnUrl)
        {
            var product = this.repository.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                this.Cart.AddItem(product, 1);
            }

            return this.RedirectToAction("Index", new { returnUrl });
        }

        // POST: /Cart/Remove
        [HttpPost]
        public IActionResult Remove(long productId, string returnUrl)
        {
            var line = this.Cart.Lines.FirstOrDefault(cl => cl.Product.ProductId == productId);
            if (line != null)
            {
                this.Cart.RemoveLine(line.Product);
            }

            return this.RedirectToAction("Index", new { returnUrl });
        }

        public IActionResult Index(Uri returnUrl)
        {
            throw new NotImplementedException();
        }
    }
}
