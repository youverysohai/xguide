﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.Logging
{
    public interface ILoggable
    {
        void Log(string message);
    }
}
