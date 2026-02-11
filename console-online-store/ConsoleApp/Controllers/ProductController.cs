using System;
using StoreBLL.Models;
using StoreBLL.Services;
using StoreDAL.Data;

namespace ConsoleApp.Controllers
{
    public static class ProductController
    {
        private static StoreDbContext context = UserMenuController.Context;

        // ---------- Products (list/show only for Step 4) ----------
        public static void ShowAllProducts()
        {
            var svc = new ProductService(context);
            foreach (var m in svc.GetAll())
            {
                Console.WriteLine(m);
            }
        }

        public static void ShowProduct()
        {
            var svc = new ProductService(context);
            Console.Write("Product Id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                return;
            }

            var p = (ProductModel)svc.GetById(id);
            Console.WriteLine(p);
        }

        // Leave creation/update/delete for products to a later step if desired
        public static void AddProduct() => throw new NotImplementedException();

        public static void UpdateProduct() => throw new NotImplementedException();

        public static void DeleteProduct() => throw new NotImplementedException();

        // ---------- Categories ----------
        public static void AddCategory()
        {
            var svc = new CategoryService(context);
            Console.Write("Category name: ");
            var name = Console.ReadLine() ?? string.Empty;
            var m = new CategoryModel(0, name);
            svc.Add(m);
            Console.WriteLine($"Category created Id={m.Id}");
        }

        public static void UpdateCategory()
        {
            var svc = new CategoryService(context);
            Console.Write("Category Id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                return;
            }

            Console.Write("New name: ");
            var name = Console.ReadLine() ?? string.Empty;
            svc.Update(new CategoryModel(id, name));
            Console.WriteLine("Category updated.");
        }

        public static void DeleteCategory()
        {
            var svc = new CategoryService(context);
            Console.Write("Category Id to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                return;
            }

            svc.Delete(id);
            Console.WriteLine("Category deleted.");
        }

        public static void ShowAllCategories()
        {
            var svc = new CategoryService(context);
            foreach (var m in svc.GetAll())
            {
                Console.WriteLine(m);
            }
        }

        // ---------- Product Titles ----------
        public static void AddProductTitle()
        {
            var svc = new ProductTitleService(context);
            Console.Write("Title: ");
            var title = Console.ReadLine() ?? string.Empty;
            Console.Write("CategoryId: ");
            if (!int.TryParse(Console.ReadLine(), out var catId))
            {
                return;
            }

            var m = new ProductTitleModel(0, title, catId, string.Empty);
            svc.Add(m);
            Console.WriteLine($"Product title created Id={m.Id}");
        }

        public static void UpdateProductTitle()
        {
            var svc = new ProductTitleService(context);
            Console.Write("ProductTitle Id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                return;
            }

            var current = (ProductTitleModel)svc.GetById(id);
            Console.WriteLine($"Current: {current}");

            Console.Write("New title (blank to keep): ");
            var s = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(s))
            {
                current.Title = s;
            }

            Console.Write("New CategoryId (blank to keep): ");
            s = Console.ReadLine();
            if (int.TryParse(s, out var newCatId))
            {
                current.CategoryId = newCatId;
            }

            svc.Update(current);
            Console.WriteLine("Product title updated.");
        }

        public static void DeleteProductTitle()
        {
            var svc = new ProductTitleService(context);
            Console.Write("ProductTitle Id to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                return;
            }

            svc.Delete(id);
            Console.WriteLine("Product title deleted.");
        }

        public static void ShowAllProductTitles()
        {
            var svc = new ProductTitleService(context);
            foreach (var m in svc.GetAll())
            {
                Console.WriteLine(m);
            }
        }

        // ---------- Manufacturers ----------
        public static void AddManufacturer()
        {
            var svc = new ManufacturerService(context);
            Console.Write("Manufacturer name: ");
            var name = Console.ReadLine() ?? string.Empty;
            var m = new ManufacturerModel(0, name);
            svc.Add(m);
            Console.WriteLine($"Manufacturer created Id={m.Id}");
        }

        public static void UpdateManufacturer()
        {
            var svc = new ManufacturerService(context);
            Console.Write("Manufacturer Id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                return;
            }

            Console.Write("New name: ");
            var name = Console.ReadLine() ?? string.Empty;
            svc.Update(new ManufacturerModel(id, name));
            Console.WriteLine("Manufacturer updated.");
        }

        public static void DeleteManufacturer()
        {
            var svc = new ManufacturerService(context);
            Console.Write("Manufacturer Id to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                return;
            }

            svc.Delete(id);
            Console.WriteLine("Manufacturer deleted.");
        }

        public static void ShowAllManufacturers()
        {
            var svc = new ManufacturerService(context);
            foreach (var m in svc.GetAll())
            {
                Console.WriteLine(m);
            }
        }
    }
}
