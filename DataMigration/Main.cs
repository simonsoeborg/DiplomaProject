using ClassLibrary.Models;

GroenlundDbContext context = new();
DataMigrater dataMigrater = new(context);
//DemoDataGenerator demoDataGenerator = new(context);

dataMigrater.CreateDataInDatabase();
//dataMigrater.PrintProducts();
