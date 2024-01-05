
using System.Reflection.Emit;

namespace DO;
/// <summary>
/// Engineer Entity represents an engineer with all his details 
/// </summary>
/// <param name="IdNum"></param>Engineer ID number
/// <param name="Name"></param>Name of engineer
/// <param name="Email"></param> Engineer's email address
/// <param name="CostPerHour"></param>
/// <param name="EngineerLevel"></param>The engineer's experience

public record Engineer
(
    int IdNum,   //key
    string? Name= null,
    string? Email = null,
    EngineerLevel? EngineerLevel = null,
    double? CostPerHour =null
    
)

{
    public Engineer() : this(0) { } // empty ctor
}





