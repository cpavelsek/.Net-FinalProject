using Final2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Final2
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {

            logger.Info("Program started");

            try
            {
                string choice;
                do
                {
                    Console.WriteLine("Enter your selection:");
                    Console.WriteLine("1) Add a Product");
                    Console.WriteLine("2) Edit a Product");
                    Console.WriteLine("3) Show Products");
                    Console.WriteLine("4) Add a Category");
                    Console.WriteLine("5) Edit a Category");
                    Console.WriteLine("6) Show all Categories");
                    Console.WriteLine("7) Show all Categories and details");
                    Console.WriteLine("8) Show speicific Category and details");
                    Console.WriteLine("Enter q to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info("Option {choice} selected", choice);

                    if (choice == "1")
                    {
                        Console.Write("Enter a name for a new Product: ");
                        var product = new Products { ProductName = Console.ReadLine() };

                        ValidationContext context = new ValidationContext(product, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(product, context, results, true);
                        if (isValid)
                        {
                            var db = new Northwind_05_CEPContext();
                            // check for unique name
                            if (db.Products.Any(b => b.ProductName == product.ProductName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Product name exists", new string[] { "Name" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                // save Product to database
                                db.AddProduct(product);
                                logger.Info("Blog added - {name}", product.ProductName);
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }
                    else if (choice == "2")
                    {    // edit Product
                        Console.WriteLine("Choose the product to edit:");

                        var test = new Northwind_05_CEPContext();
                        var productsQuery = test.Products.OrderBy(b => b.ProductName);

                        foreach (var product1 in productsQuery)
                        {
                            Console.WriteLine(product1.ProductId + ": " + product1.ProductName);
                        }

                     //   var product = GetProduct(test);
                        var product = Console.ReadLine();
                        if (product != null)
                        {
                            // UpdateProduct
                            Products UpdateProduct = InputProducts(test);
                            if (UpdateProduct != null)
                            {
                                UpdateProduct.ProductName = UpdateProduct.ProductName;
                                test.EditProduct(UpdateProduct);
                                logger.Info("Product (Name: {productName}) updated", UpdateProduct.ProductName);
                            }
                        }
                    }
                    else if (choice == "3")
                    {

                        var test = new Northwind_05_CEPContext();

                        var productsQuery = test.Products.OrderBy(b => b.ProductName);
                        var productsActive = test.Products.OrderBy(b => b.Discontinued);
                        //bool productsDiscountinued = test.Products.Contains(b => b.where)

                        Console.WriteLine("If you would like to see all products, enter 1.");
                        Console.WriteLine("If you would like to see all active products, enter 2.");
                        Console.WriteLine("If you would like to see all discontinued products, enter 3.");
                        Console.WriteLine("If you would like to see specific products, enter 4.");
                        var userChoice = Console.ReadLine();

                        if (userChoice == "1")
                        {
                            Console.WriteLine($"{productsQuery.Count()} Products returned");
                            foreach (var product in productsQuery)
                            {
                                Console.WriteLine(product.ProductId + ": " + product.ProductName);
                            }
                        }
                        else if (userChoice == "2")
                        {
                            foreach (var product in productsActive)
                            {
                                if (product.Discontinued == false)
                                    Console.WriteLine(product.ProductId + ": " + product.ProductName);
                            }

                        }
                        else if (userChoice == "3")
                        {
                            foreach (var product in productsActive)
                            {
                                if (product.Discontinued == true)
                                    Console.WriteLine(product.ProductId + ": " + product.ProductName);
                            }
                        }
                        else if (userChoice == "4")
                        {
                            Console.WriteLine("Which Product Would you like to See?");
                            foreach (var product in productsQuery)
                            {
                                Console.WriteLine(product.ProductId + ": " + product.ProductName);
                            }
                            Console.WriteLine("Enter the Product ID of the product you would like to see.");
                            int.TryParse(Console.ReadLine(), out int productChoice);


                            IEnumerable<Products> products;

                            products = test.Products.Where(p => p.ProductId == productChoice);
                            foreach (var item in products)
                            {
                                Console.WriteLine("Product ID: " + item.ProductId + "\nProduct Name: " + item.ProductName + "\nQuantity Per Unit: " + item.QuantityPerUnit +
                                                  "\nProduct Supplier: " + item.Supplier + "\nProduct Price: " + item.UnitPrice + "\nProducts In Stock: " + item.UnitsInStock + "\nProducts on Order: " + item.UnitsOnOrder);
                            }
                        }
                        else { Console.WriteLine("Please review the previous choices again."); }

                        Console.WriteLine(" ");
                    }
                    else if (choice == "4")
                    {
                        Console.Write("Enter a name for a new Category: ");
                        var category = new Categories { CategoryName = Console.ReadLine() };

                        ValidationContext context = new ValidationContext(category, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(category, context, results, true);
                        if (isValid)
                        {
                            var db = new Northwind_05_CEPContext();
                            // check for unique name
                            if (db.Categories.Any(b => b.CategoryName == category.CategoryName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Category name exists", new string[] { "Name" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                // save Category to database
                                db.AddCategory(category);
                                logger.Info("Category added - {name}", category.CategoryName);
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }

                    }
                    //edit category
                    else if (choice == "5")
                    {
                        Console.WriteLine("Choose the Category to edit:");

                        var test = new Northwind_05_CEPContext();
                        var categoryQuery = test.Categories.OrderBy(b => b.CategoryName);

                        foreach (var category in categoryQuery)
                        {
                            Console.WriteLine(category.CategoryId + ": " + category.CategoryName);
                        }
                        var UpdateProduct = GetProduct(test);
                        var product = Console.ReadLine();

                        if (UpdateProduct != null)
                        {
                            UpdateProduct.ProductName = UpdateProduct.ProductName;
                            test.EditProduct(UpdateProduct);
                            logger.Info("Category (Name: {productName}) updated", UpdateProduct.ProductName);
                        }

                    }
                    //Show all categories
                    else if (choice == "6")
                    {
                        var test = new Northwind_05_CEPContext();

                        var categoryQuery = test.Categories.OrderBy(b => b.CategoryName);

                        Console.WriteLine($"{categoryQuery.Count()} Categories returned");
                        foreach (var category in categoryQuery)
                        {
                            Console.WriteLine("Name: " + category.CategoryName + "\nDescription: " + category.Description);
                        }
                        Console.WriteLine();
                    }
                    //show all categories and details
                    else if (choice == "7")
                    {
                        var test = new Northwind_05_CEPContext();
                        var test1 = new Northwind_05_CEPContext();

                        var categoryQuery = test.Categories.OrderBy(b => b.CategoryName);

                        IEnumerable<Products> products;

                        products = test1.Products.OrderBy(p => p.Category);
                      //  var category1Products = test1.Products.Include(); 

                        Console.WriteLine($"{categoryQuery.Count()} Categories returned");
                        foreach (var category in categoryQuery)
                        {
                            Console.WriteLine("Category Name: " + category.CategoryName + "\nDescription: " + category.Description);

                            var specificCategory = from prod in products
                                                   group prod by prod.CategoryId;

                            //foreach (var productGroup in specificCategory)
                            //{
                            //    Console.WriteLine(productGroup.Key);
                            //    foreach(Products product in productGroup)
                            //    {
                            //        Console.Write(" {1}", product.ProductName);
                            //    }
                            //}
                        }
                        Console.WriteLine();

                    }
                    //show specific category and details 
                    else if (choice == "8")
                    {
                        var test = new Northwind_05_CEPContext();

                        var categoryQuery = test.Categories.OrderBy(b => b.CategoryName);

                        Console.WriteLine("Which category would you like to see in detail?  Enter the Category ID.");
                        //show all categories
                        foreach (var category in categoryQuery)
                        {
                            Console.WriteLine("Category ID: " + category.CategoryId + "\nName: " + category.CategoryName);
                        }


                        if (int.TryParse(Console.ReadLine(), out int CategoryID))
                        {
                            IEnumerable<Categories> categories;
                            if (CategoryID != 0 && test.Categories.Count(b => b.CategoryId == CategoryID) == 0)
                            {
                                logger.Error("There are no Categories saved with that Id");
                            }
                            else
                            {
                                // display posts from all blogs
                                categories = test.Categories.OrderBy(p => p.CategoryId);
                                if (CategoryID == 0)
                                {
                                    // display all posts from all blogs
                                    categories = test.Categories.OrderBy(p => p.CategoryName);
                                }
                                else
                                {
                                    // display category information
                                    categories = test.Categories.Where(p => p.CategoryId == p.CategoryId).OrderBy(p => p.CategoryName);
                                }
                                Console.WriteLine();
                                foreach (var item in categories)
                                {
                                    //Console.WriteLine($"Category ID: {item.CategoryId}\nTitle: {item.CategoryName}\nDescription: {item.Description}\n");
                                    if (CategoryID == item.CategoryId)
                                    {
                                        Console.WriteLine($"Category ID: {item.CategoryId}\nTitle: {item.CategoryName}\nDescription: {item.Description}\n");
                                    }
                                }
                            }
                        }

                    }

                }
                while (choice != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }



        public static Products GetProduct(Northwind_05_CEPContext test)
        {
            var categories = test.Categories.OrderBy(b => b.CategoryName);

            var products = test.Products.OrderBy(b => b.ProductId);
            foreach (Products p in products)
            {
                Console.WriteLine(p.ProductName);
                if (p.ProductName.Count() == 0)
                {
                    Console.WriteLine($"  <no products>");
                }
                else
                {
                    foreach (Products p1 in p.Category.Products)
                    {
                        Console.WriteLine($"  {p.ProductId}) {p.ProductName}");
                    }
                }
            }
            if (int.TryParse(Console.ReadLine(), out int ProductID))
            {
                Products post = test.Products.FirstOrDefault(p => p.ProductId == ProductID);
                if (post != null)
                {
                    return post;
                }
            }
            logger.Error("Invalid Post Id");
            return null;
        }



        public static Products InputProducts(Northwind_05_CEPContext test)
        {

            Products post = new Products();
            Console.WriteLine("Enter the Product title");
            post.ProductName = Console.ReadLine();


            ValidationContext context = new ValidationContext(post, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(post, context, results, true);
            if (isValid)
            {
                return post;
            }
            else
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
            return null;
        }
    }
}
 


