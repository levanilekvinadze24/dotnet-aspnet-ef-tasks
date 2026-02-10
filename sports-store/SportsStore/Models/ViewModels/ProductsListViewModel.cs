using System.Collections.Generic;

namespace SportsStore.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        public PagingInfo PagingInfo { get; set; } = new PagingInfo();

        public string? CurrentCategory { get; set; }
    }
}
