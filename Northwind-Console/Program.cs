using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using NLog;
using NorthwindConsole.Models;

namespace NorthwindConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                string choice;
                do
                {
                    Console.WriteLine("1) Display Categories");
                    Console.WriteLine("2) Add Category");
                    Console.WriteLine("3) Display Category and related products");
                    Console.WriteLine("4) Display all Categories and their related products");
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info($"Option {choice} selected");
                    if (choice == "1")
                    {
                        var db = new NorthwindContext();
                        var query = db.Categories.OrderBy(p => p.CategoryName);

                        Console.WriteLine($"{query.Count()} records returned");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName} - {item.Description}");
                        }
                    }
                    else if (choice == "2")
                    {
                            Category category = new Category();
                            Console.WriteLine("Enter Category Name:");
                            category.CategoryName = Console.ReadLine();
                            Console.WriteLine("Enter the Category Description:");
                            category.Description = Console.ReadLine();

                        ValidationContext context = new ValidationContext(category, null, null); // what do I want to validate? = category put category in our context
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(category, context, results, true); // validate category and return it to results = bool
                        if (isValid)
                        {
                            var db = new NorthwindContext();
                            db.Categories.Add(category); //added in class
                            var erro = db.GetValidationErrors(); //added in class
                            if (erro.Any()) //added in class
                            {
                                Console.WriteLine(erro);
                            }

                           // db.SaveChanges(); // added in class
                            // check for unique name
                            if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                db.SaveChanges(); // added in class
                                // TODO: save category to db
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
       //                         logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                                logger.Error($"{result.ErrorMessage}");
                                Console.WriteLine($"ERROR: {result.ErrorMessage}");
                            }
                        }

                    } else if (choice == "3")
                        {
                            var db = new NorthwindContext();
                            var query = db.Categories.OrderBy(p => p.CategoryId);

                            Console.WriteLine("Select the category whose products you want to display:");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                            }
                            int id = int.Parse(Console.ReadLine());
                            Console.Clear();
                            logger.Info($"CategoryId {id} selected");
                            Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
                            Console.WriteLine($"{category.CategoryName} - {category.Description}");
                            foreach (Product p in category.Products) // products enumerated in the category bcause of the list
                            {
                                Console.WriteLine(p.ProductName);
                            }
                        }
                        else if (choice == "4")
                        {
                            var db = new NorthwindContext();
                            var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId); // dont do .ToList() after because we can add to it with where etc.
 // doesn't work                        query.Where(c => c.Description.Contains("xxxx")); // added in class
                        foreach (var item in query)
                            {
                                Console.WriteLine($"{item.CategoryName}");
                                foreach (Product p in item.Products)
                                {
                                    Console.WriteLine($"\t{p.ProductName}");
                                }
                            }
                        }
                        Console.WriteLine();

                    } while (choice.ToLower() != "q") ;
                }
                catch (DbEntityValidationException) // code added in class
            {
                // in github
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
