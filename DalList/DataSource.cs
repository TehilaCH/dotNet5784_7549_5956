
namespace Dal;
static internal class DataSource
{
    /// <summary>
    /// internal class that creates automatic running numbers for the fields defined as "running ID number"
    /// </summary>
    internal static class Config  
    {

        internal const int startTaskId = 1000;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal const int startDependenceId = 1000;
        private static int nextDependenceId = startDependenceId;
        internal static int NextDependenceId { get => nextDependenceId++; }


    }

    /// <summary>
    /// Lists containing the entities
    /// </summary>

    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Dependence> Dependences { get; } = new();
    
}
