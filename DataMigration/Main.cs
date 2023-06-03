
using DataMigration;

DemoDataGenerator dataGenerator = new();
dataGenerator.PopulateDatabase(msSQL: true);


//DataMigrater dataMigrater = new();
//var (Product, ProductItems, Images) = dataMigrater.ExtractProducts();
//int counter = 0;
//Console.WriteLine(Product.Count + "products - " + ProductItems.Count + "productItems");
////foreach (var productItem in ProductItems)
////{
////    if (string.IsNullOrEmpty(productItem.Product.Design))
////    {
////        counter++;
////        //Console.WriteLine(productItem.ToString());
////        //Console.WriteLine(productItem.Product.ToString());
////    }
////    else
////    {
////        Console.WriteLine(productItem.Product.Design);
////    }
////}
////Console.WriteLine(counter + "productItems has empty design");
