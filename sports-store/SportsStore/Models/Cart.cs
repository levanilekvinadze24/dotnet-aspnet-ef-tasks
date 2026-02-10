namespace SportsStore.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = [];

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine? line = this.Lines.FirstOrDefault(p => p.Product.ProductId == product.ProductId);
            if (line == null)
            {
                this.Lines.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product)
        {
            _ = this.Lines.RemoveAll(l => l.Product.ProductId == product.ProductId);
        }

        public virtual void Clear()
        {
            this.Lines.Clear();
        }

        public decimal ComputeTotalValue() => this.Lines.Sum(x => x.Product.Price * x.Quantity);
    }

    public class CartLine
    {
        public int CartLineId { get; set; }

        public Product Product { get; set; } = new Product();

        public int Quantity { get; set; }
    }
}
