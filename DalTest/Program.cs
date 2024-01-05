using Dal;
using DalApi;
using DO;

namespace DalTest
{
    internal class Program
    {
        private static IEngineer? s_dalIEngineer = new EngineerImplementation(); //stage 1
        private static IDependence? s_dalIDependence = new DependenceImplementation(); //stage 1
        private static ITask? s_dalITask = new TaskImplementation(); //stage 1

       
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
