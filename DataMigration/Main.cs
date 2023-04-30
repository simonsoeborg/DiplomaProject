
using DataMigration;

DemoDataGenerator dataGenerator = new();
dataGenerator.PopulateDatabase();
//DataMigrater dataMigrater = new();
//var (Product, ProductItems, Images) = dataMigrater.ExtractProducts();

//foreach (var productItem in ProductItems)
//{
//    Console.WriteLine(productItem.ToString());
//    Console.WriteLine(productItem.Product.ToString());
//}
