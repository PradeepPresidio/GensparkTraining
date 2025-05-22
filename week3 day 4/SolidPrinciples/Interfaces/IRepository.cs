using System;
using project.Models;
namespace project.Interfaces
{
	interface IDiscount
	{
		double ApplyDiscount(InventoryObj product);
	}
	// I - Interface Segregation Principle
	interface IReadableRepo
	{
		InventoryObj Get(int id);
		List<InventoryObj> GetAll();
	}

	interface IWritableRepo
	{
		void Add(InventoryObj item);
		void Update(int id, InventoryObj newItem);
	}

	interface IRepository : IReadableRepo, IWritableRepo { }
}