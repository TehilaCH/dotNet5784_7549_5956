internal partial class Program
{
    private static void Main(string[] args)
    {
        Welcome5956();
        Welcome7549();
        Console.ReadKey();
    }

    static partial void Welcome7549();
    private static void Welcome5956()
    {
        Console.WriteLine("Hello, World!");
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();
        Console.WriteLine("{0}, welcome to my first console application", name);
    }
}