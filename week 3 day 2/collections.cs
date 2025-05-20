using System;
using System.Collections.Generic;
using System.Linq;
public class Employee

{
    int id, age;
    string name;
    double salary;
    public Employee()
    {

    }
    public Employee(int id, int age, string name, double salary)
    {
        this.id = id;
        this.age = age;
        this.name = name;
        this.salary = salary;
    }
    public void TakeEmployeeDetailsFromUser()
    {
        Console.WriteLine("Please enter the employee ID");
        id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Please enter the employee name");
        name = Console.ReadLine();
        Console.WriteLine("Please enter the employee age");
        age = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Please enter the employee salary");
        salary = Convert.ToDouble(Console.ReadLine());
    }
    public override string ToString()
    {
        return "Employee ID : " + id + "\nName : " + name + "\nAge : " + age + "\nSalary : " + salary;
    }
    public int Id { get => id; set => id = value; }
    public int Age { get => age; set => age = value; }
    public string Name { get => name; set => name = value; }
    public double Salary { get => salary; set => salary = value; }
}

class EmployeePromotion
{
    public List<String> promotionQueue = new List<String>();
    public EmployeePromotion()
    {
        Console.WriteLine("Enter Employee names, Blank to stop");
        while (true)
        {

            string EmployeeName = Console.ReadLine();
            if (EmployeeName == "")
            {
                break;
            }
            promotionQueue.Add(EmployeeName);
        }
    }
    public int getPositionByName()
    {
        Console.WriteLine("Enter name of employee to find their position");
        string name = Console.ReadLine();
        return promotionQueue.IndexOf(name);
    }
    public void optimizeSpace()
    {
        Console.WriteLine("Size before Optimization " + promotionQueue.Capacity);
        promotionQueue.TrimExcess();
        Console.WriteLine("Size after Optimization " + promotionQueue.Capacity);
    }
    public void printInAsc()
    {
        Console.WriteLine("Employee names in Ascending Order");
        promotionQueue.Sort();
        foreach (var item in promotionQueue)
        {
            Console.WriteLine(item);
        }
    }
    public void insertIntoEmployeeDict(Dictionary<int,Employee>EmployeeDict)
    {
        int id, age;
        double salary;
        string name;
        Console.WriteLine("Enter employee ID");
        id = Convert.ToInt32(Console.ReadLine());
        if(EmployeeDict.ContainsKey(id))
        {
            Console.WriteLine("Employee with the ID is already present");
            return;
        }
        Console.WriteLine("Enter employee name");
        name = Console.ReadLine();
        Console.WriteLine("Enter age");
        age = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter salary");
        salary = double.Parse(Console.ReadLine());
        EmployeeDict.Add(id, new Employee(id, age, name, salary));
        Console.WriteLine("Employee Added to Dictionary");
    }
    public void getEmployeeByID(Dictionary<int,Employee>EmployeeDict,int id)
    {
        if(!EmployeeDict.ContainsKey(id))
        {
            Console.WriteLine("Employee with given ID not found");
            return;
        }
        Console.Write($"Employee ID : {EmployeeDict[id].Id} , Employee Name  : {EmployeeDict[id].Name} , Employee Age : {EmployeeDict[id].Age}, Employee Salary : {EmployeeDict[id].Salary}\n");
    }
    public List<String> PromotionQueue { get => promotionQueue; set => promotionQueue = value; }
    
    public List<Employee> EmployeeDictToList(Dictionary<int,Employee>EmployeeDict)
    {
        List < Employee > EmployeeList = new List<Employee>();
        foreach (Employee emp in EmployeeDict.Values) {
        EmployeeList.Add(emp);
        }
        return EmployeeList;
    }
    public void sortEmployeesBySalary(List<Employee> EmployeeList)
    {
        EmployeeList.Sort(new SalaryComparer());
    }
    public void displayEmployees(List<Employee> EmployeeList)
    {
        foreach (Employee emp in EmployeeList) { 
        Console.WriteLine(emp.ToString());
        }
    }
    static void Main()
    { 
        EmployeePromotion p = new EmployeePromotion();
         //EASY
        //foreach (string empName in p.PromotionQueue)
        //{
        //    Console.WriteLine(empName);
        //}
        //Console.WriteLine(p.getPositionByName()+1);
        //p.optimizeSpace();
        //p.printInAsc();
        //EASY

        Dictionary<int, Employee> EmployeeDict= new Dictionary<int, Employee>();
     p.insertIntoEmployeeDict(EmployeeDict);
        p.insertIntoEmployeeDict(EmployeeDict);
        p.insertIntoEmployeeDict(EmployeeDict);
        p.getEmployeeByID(EmployeeDict,1);
        List<Employee> EmployeeList = p.EmployeeDictToList(EmployeeDict);
     p.sortEmployeesBySalary(EmployeeList);  
        p.displayEmployees(EmployeeList);
        Employee emp = EmployeeList.First(emp=>emp.Id == 1);
        if (emp != null) { 
        Console.WriteLine(emp.ToString());
        }
        string name;
        Console.WriteLine("Enter name to filter ");
        name = Console.ReadLine();
        List<Employee> filteredEmployees = EmployeeList.Where(emp => emp.Name == name).ToList();
        Console.WriteLine("List of Employees with given name ");
        foreach (var employee in filteredEmployees) {
            Console.WriteLine(employee.ToString());
        }
        int employeeId;
        Console.WriteLine("Enter employee ID to find older employees");
        employeeId = int.Parse(Console.ReadLine());
        Employee reqEmp = EmployeeList.Find(emp => emp.Id == employeeId);
        if (reqEmp != null) { 
        List<Employee> olderEmployees = EmployeeList.Where(emp=>emp.Age > reqEmp.Age).ToList();
            Console.WriteLine("List of Employees older than given Employee");
        p.displayEmployees(olderEmployees);
        }
    }
}
public class SalaryComparer : IComparer<Employee>
{
    public int Compare(Employee X, Employee Y)
    {
        if(X == null || Y == null) return 0;
        return X.Salary.CompareTo(Y.Salary);
    }
}