using System;
using System.Collections.Generic;
namespace WholeApplication
{
    
        public class Employee
        {
            public int Id { get; set; }
            public int Age { get; set; }
            public string Name { get; set; }
            public double Salary { get; set; }

            public Employee(int id, int age, string name, double salary)
            {
                Id = id;
                Age = age;
                Name = name;
                Salary = salary;
            }

            public override string ToString()
            {
                return $"Id: {Id}, Name: {Name}, Age: {Age}, Salary: {Salary}";
            }
        }

    internal class Program
    {
        List<Employee> employees = new List<Employee>()
        {
            new Employee(101,30, "John Doe",  50000),
            new Employee(102, 25,"Jane Smith",  60000),
            new Employee(103,35, "Sam Brown",  70000)
        };
        //public delegate void MyDelegate<T>(T num1, T num2);
        //public delegate void MyFDelegate(float num1, float num2);
        public void Add(int n1, int n2)
        {
            int sum = n1 + n2;
            Console.WriteLine($"The sum of {n1} and {n2} is {sum}");
        }
        public void Product(int n1, int n2)
        {
            int prod = n1 * n2;
            Console.WriteLine($"The sum of {n1} and {n2} is {prod}");
        }
        Program()
        {
            //MyDelegate<int> del = new MyDelegate<int>(Add);
            Action<int, int> del = Add;
            del += Product;
            //del += delegate (int n1, int n2)
            //{
            //    Console.WriteLine($"The division result of {n1} and {n2} is {n1 / n2}");
            //};
            del += (int n1, int n2) => Console.WriteLine($"The division result of {n1} and {n2} is {n1 / n2}");
            del(100, 20);
        }
        public void func(Employee emp) { Console.WriteLine(emp.ToString()); }
        void FindEmployee()
        {
            int empId = 102;
            Predicate<Employee> predicate = e => e.Id == empId;
            Employee? emp = employees.Find(predicate);
            Console.WriteLine(emp.ToString() ?? "No such employee");
            Action<Employee> act = func;
            employees.ForEach(act);
        }

        static void Main(string[] args)
        {
            //IRepositor<int, Employee> employeeRepository = new EmployeeRepository();
            //IEmployeeService employeeService = new EmployeeService(employeeRepository);
            //ManageEmployee manageEmployee = new ManageEmployee(employeeService);
            //manageEmployee.Start();
            //new Program();
            Program program = new();
            program.FindEmployee();
        }
    }
}
