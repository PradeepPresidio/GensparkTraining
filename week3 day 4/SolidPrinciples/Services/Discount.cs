using System;
using project.Models;
using project.Interfaces;
using project.Repositories;
namespace project.Services
{
    class EmployeeDiscount : IDiscount
    {
        public double ApplyDiscount(InventoryObj product)
        {
            return product.Price * 0.85; // 15% off for employees
        }
    }

    class CustomerDiscount : IDiscount
    {
        public double ApplyDiscount(InventoryObj product)
        {
            return product.Price * 0.95; // 5% off for customers
        }
    }
    
}