

using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DO;
/// <summary>
/// Indicates the dependency between tasks, i.e. which task can only be executed after the completion 
/// of a previous task.
/// </summary>
/// <param name="IDNumber"></param>
/// <param name="IdPendingTask"></param>ID number of pending task
/// <param name="IdPreviousTask"></param>Previous assignment ID number
public record Dependence
( int IDNumber, //key (run)
  int? IdPendingTask =null,
  int? IdPreviousTask= null


)
{  
    public Dependence() : this(0) { }

}
