using System;
using project.Models;
using project.Interfaces;
using project.Repositories;
using project.Services;

class Shop
{
    static void Main(string[] args)
    {
        //Liskov
        IRepository repo = new Repository();
        IDiscount empDiscount = new EmployeeDiscount();
        IDiscount custDiscount = new CustomerDiscount();

        var item1 = new InventoryObj { Id = 1, Name = "Keyboard", Description = "Mechanical", Price = 100, Quantity = 10, Location = "A1" };
        var item2 = new InventoryObj { Id = 2, Name = "Mouse", Description = "Wireless", Price = 50, Quantity = 20, Location = "A2" };

        repo.Add(item1);
        repo.Add(item2);

        Console.WriteLine("Employee Discount Price: " + empDiscount.ApplyDiscount(item1));
        Console.WriteLine("Customer Discount Price: " + custDiscount.ApplyDiscount(item1));

        InventoryReport report = new(repo);
        Console.WriteLine("\nInventory Report:");
        report.GenerateInventoryReport();
    }
}