using ClassLibrary.Models;
using Newtonsoft.Json;

namespace DataMigration
{

    public static class DemoDataRepository
    {
        public static List<Category> Categories() { return JsonConvert.DeserializeObject<List<Category>>(File.ReadAllText("DemoData/categories.json"))!; }
        public static List<Subcategory> Subcategories() { return JsonConvert.DeserializeObject<List<Subcategory>>(File.ReadAllText("DemoData/subcategories.json"))!; }
        public static List<Product> Products() { return JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText("DemoData/products.json"))!; }
        public static List<ProductItem> ProductItems() { return JsonConvert.DeserializeObject<List<ProductItem>>(File.ReadAllText("DemoData/productitems.json"))!; }
        public static List<Image> Images() { return JsonConvert.DeserializeObject<List<Image>>(File.ReadAllText("DemoData/images.json"))!; }
        public static List<Role> Roles() { return JsonConvert.DeserializeObject<List<Role>>(File.ReadAllText("DemoData/roles.json"))!; }
        public static List<Order> Orders() { return JsonConvert.DeserializeObject<List<Order>>(File.ReadAllText("DemoData/orders.json"))!; }
        public static List<User> Users() { return JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("DemoData/users.json"))!; }
        public static List<Payment> Payments() { return JsonConvert.DeserializeObject<List<Payment>>(File.ReadAllText("DemoData/payments.json"))!; }
        public static List<Customer> Customers() { return JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText("DemoData/customers.json"))!; }
        public static List<DiscountCode> DiscountCodes() { return JsonConvert.DeserializeObject<List<DiscountCode>>(File.ReadAllText("DemoData/discountcodes.json"))!; }
        public static List<Category> SøborgCategories() { return JsonConvert.DeserializeObject<List<Category>>(File.ReadAllText("DemoData/SøborgJSON/s_categories.json"))!; }
        public static List<Category> SøborgSubcategories() { return JsonConvert.DeserializeObject<List<Category>>(File.ReadAllText("DemoData/SøborgJSON/s_subcategories.json"))!; }
    }
}
