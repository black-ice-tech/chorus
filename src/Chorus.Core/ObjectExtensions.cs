﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Chorus.Core
{
    public static class ObjectExtensions
    {
        public static void ThrowIfNull(this object arg, string argName)
        {
           _ = arg ?? throw new ArgumentNullException(argName);
        }
    }
}
