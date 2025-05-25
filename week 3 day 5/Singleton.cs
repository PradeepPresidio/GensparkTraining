using System;
public sealed class Singleton
{
    private Singleton() { }
    private static Singleton instance;
    public string Message {  get; set; }
    public static Singleton Instance
    {
        get {
            if(instance == null)
                instance = new Singleton();
            return instance;
        }
    }
}
public class Program()
{
    public static void Main(string[] args)
    {
        Singleton obj = Singleton.Instance;
        obj.Message = "Hi";
        //Console.WriteLine(obj.Message);
        Singleton another = Singleton.Instance;
        another.Message = "hello";
        Console.WriteLine(another.Message+obj.Message);
    }
}