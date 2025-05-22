using System;
using project.Models;
using project.Interfaces;
namespace project.Repositories{ 
class Repository : IRepository
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
}