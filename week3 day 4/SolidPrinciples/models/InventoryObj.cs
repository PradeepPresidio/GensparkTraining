using System;
using System.Collections.Generic;

namespace project.Models
{
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
}