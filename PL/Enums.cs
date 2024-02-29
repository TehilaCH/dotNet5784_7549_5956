using System;
using System.Collections;
using System.Collections.Generic;

namespace PL;

internal class EngineersCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerLevel> s_enums =
        (Enum.GetValues(typeof(BO.EngineerLevel)) as IEnumerable<BO.EngineerLevel>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();

}


internal class StatusCollection : IEnumerable
{ 
    static readonly IEnumerable<BO.Status> Status_enums =
       (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

    public IEnumerator GetEnumerator() => Status_enums.GetEnumerator();

}