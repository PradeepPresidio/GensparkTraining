using System;
using project.Models;
using project.Interfaces;
using project.Repositories;
namespace project.Services
{
    class InventoryReport
    {
        private readonly IRepository repository;

        // D - Dependency Inversion Principle
        // InventoryReport depend on abstraction (IRepository), not concrete (Repository)
        public InventoryReport(IRepository repo)
        {
            repository = repo;
        }

        public void GenerateInventoryReport()
        {
            var items = repository.GetAll();
            items.ForEach(item => Console.WriteLine(item.ToString()));
        }
    }
}