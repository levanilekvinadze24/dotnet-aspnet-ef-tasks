namespace StoreDAL.Data.InitDataFactory;

using System;
using StoreDAL.Entities;

public class TestDataFactory : AbstractDataFactory
{
    // Pre-hashed with PasswordHasher (salt = "store-salt")
    // admin -> A8ABA2C93534484522FFA9D5B841A43738D8DCF75D57807BA7B49E1F633550B8
    // user  -> 04F6C56BA9AB2EE9523C3F5963C470664B333B248DAA16B5736C35D5B18659B6
    public override Category[] GetCategoryData()
    {
        return new[]
        {
            new Category(1, "fruits"),
            new Category(2, "water"),
            new Category(3, "vegetables"),
            new Category(4, "seafood"),
            new Category(5, "meat"),
            new Category(6, "grocery"),
            new Category(7, "milk food"),
            new Category(8, "smartphones"),
            new Category(9, "laptop"),
            new Category(10, "photocameras"),
            new Category(11, "kitchen accessories"),
            new Category(12, "spices"),
            new Category(13, "juice"),
            new Category(14, "alcohol drinks"),
        };
    }

    public override UserRole[] GetUserRoleData()
    {
        return new[]
        {
            new UserRole(1, "Admin"),
            new UserRole(2, "Registered"),
            new UserRole(3, "Guest"),
        };
    }

    public override User[] GetUserData()
    {
        return new[]
        {
            new User(
                1,
                "System",
                "Admin",
                "admin",
                "A8ABA2C93534484522FFA9D5B841A43738D8DCF75D57807BA7B49E1F633550B8",
                1),
            new User(
                2,
                "John",
                "User",
                "user",
                "04F6C56BA9AB2EE9523C3F5963C470664B333B248DAA16B5736C35D5B18659B6",
                2),
        };
    }

    public override OrderState[] GetOrderStateData()
    {
        return new[]
        {
            new OrderState(1, "New Order"),
            new OrderState(2, "Canceled by user"),
            new OrderState(3, "Canceled by administrator"),
            new OrderState(4, "Confirmed"),
            new OrderState(5, "Moved to delivery company"),
            new OrderState(6, "In delivery"),
            new OrderState(7, "Delivered to client"),
            new OrderState(8, "Delivery confirmed by client"),
        };
    }

    public override Manufacturer[] GetManufacturerData()
    {
        return new[]
        {
            new Manufacturer(1, "Apple"),
            new Manufacturer(2, "Samsung"),
            new Manufacturer(3, "Lenovo"),
            new Manufacturer(4, "Canon"),
            new Manufacturer(5, "KitchenPro"),
        };
    }

    public override ProductTitle[] GetProductTitleData()
    {
        return new[]
        {
            new ProductTitle(1, "iPhone 15",          8),  // smartphones
            new ProductTitle(2, "Galaxy S24",         8),  // smartphones
            new ProductTitle(3, "ThinkPad X1 Carbon", 9),  // laptop
            new ProductTitle(4, "MacBook Air 13",     9),  // laptop
            new ProductTitle(5, "Canon EOS R10",     10),  // photocameras
            new ProductTitle(6, "Blender Pro 900",   11),  // kitchen accessories
        };
    }

    public override Product[] GetProductData()
    {
        return new[]
        {
            new Product(1, 1, 1, "128 GB, Midnight",           999.00m), // iPhone 15 / Apple
            new Product(2, 2, 2, "256 GB, Graphite",           949.00m), // Galaxy S24 / Samsung
            new Product(3, 3, 3, "Gen 12, 16GB/512GB",        1999.00m), // ThinkPad X1 / Lenovo
            new Product(4, 4, 1, "M2, 8GB/256GB",             1099.00m), // MacBook Air / Apple
            new Product(5, 5, 4, "Mirrorless APS-C body",      899.00m), // Canon EOS R10 / Canon
            new Product(6, 6, 5, "High-power kitchen blender",  89.99m), // Blender / KitchenPro
        };
    }

    public override CustomerOrder[] GetCustomerOrderData()
    {
        return new[]
        {
            new CustomerOrder(1, DateTime.UtcNow.AddDays(-1).ToString("O"), 2, 1),  // user placed, New Order
            new CustomerOrder(2, DateTime.UtcNow.AddDays(-10).ToString("O"), 2, 7), // Delivered to client
        };
    }

    public override OrderDetail[] GetOrderDetailData()
    {
        return new[]
        {
            new OrderDetail(1, 1, 6,   89.99m, 1), // New order: Blender x1
            new OrderDetail(2, 2, 1,  999.00m, 1), // Delivered order: iPhone 15 x1
            new OrderDetail(3, 2, 4, 1099.00m, 1), // Delivered order: MacBook Air x1
        };
    }
}
