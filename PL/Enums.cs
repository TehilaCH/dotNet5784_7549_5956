using System;
using System.Collections;
using System.Collections.Generic;

namespace PL;

internal class EngineersCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerLevel> s_enums =
        (Enum.GetValues(typeof(BO.EngineerLevel)) as IEnumerable<BO.EngineerLevel>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();

    //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}