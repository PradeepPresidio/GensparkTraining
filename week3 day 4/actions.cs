using WholeApplication.Interfaces;
using WholeApplication.Models;
using WholeApplication.Repositories;
using WholeApplication.Services;
using WholeApplication.Interfaces;
using WholeApplication.Models;
using WholeApplication.Repositories;
using WholeApplication.Services;

namespace WholeApplication
{
    internal class Program
    {
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
        static void Main(string[] args)
        {
            //IRepositor<int, Employee> employeeRepository = new EmployeeRepository();
            //IEmployeeService employeeService = new EmployeeService(employeeRepository);
            //ManageEmployee manageEmployee = new ManageEmployee(employeeService);
            //manageEmployee.Start();
            //new Program();
            Program program = new();
        }
    }
}


namespace WholeApplication
{
    internal class Program
    {
        public delegate void MyDelegate(int num1, int num2);

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
            MyDelegate del = new MyDelegate(Add);
            del += Product;
            del(10, 20);
        }
        static void Main(string[] args)
        {
            //IRepositor<int, Employee> employeeRepository = new EmployeeRepository();
            //IEmployeeService employeeService = new EmployeeService(employeeRepository);
            //ManageEmployee manageEmployee = new ManageEmployee(employeeService);
            //manageEmployee.Start();
            new Program();
        }
    }
}
