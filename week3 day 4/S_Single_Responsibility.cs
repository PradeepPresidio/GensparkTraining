using System;
using System.Collections.Generic;

class InventoryObj
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public string Location { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Description: {Description}, Price: {Price}, Quantity: {Quantity}, Location: {Location}";
    }
}

// Only handles data storage (CRUD)
class Repository: IRepository
{
    private Dictionary<int, InventoryObj> stockItems = new();

    public void Add(InventoryObj item)
    {
        stockItems[item.Id] = item;
    }

    public InventoryObj Get(int id)
    {
        return stockItems.ContainsKey(id) ? stockItems[id] : null;
    }

    public void Update(int id, InventoryObj newItem)
    {
        if (stockItems.ContainsKey(id))
        {
            stockItems[id] = newItem;
        }
    }

    public List<InventoryObj> GetAll()
    {
        return new List<InventoryObj>(stockItems.Values);
    }
}

// S - Single Responsibility Principle: Each class has one responsibility.
// Only generates reports
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

// O -Extend with new classes, no need to modify Original discount with if/else chain
interface IDiscount
{
    double ApplyDiscount(InventoryObj product);
}
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

// L - Liskov Substitution Principle: Subtypes can be substituted for base type.
void PrintDiscountedPrice(IDiscount discount, InventoryObj item)
{
    double price = discount.ApplyDiscount(item);
    Console.WriteLine($"Discounted Price: {price}");
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
class Program
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
