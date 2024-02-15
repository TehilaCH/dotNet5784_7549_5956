﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace PL;

internal class SemestersCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerLevel> s_enums =
        (Enum.GetValues(typeof(BO.EngineerLevel)) as IEnumerable<BO.EngineerLevel>)!;

    public IEnumerator<BO.EngineerLevel> GetEnumerator() => s_enums.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}