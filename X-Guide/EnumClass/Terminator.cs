using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide
{
    public enum Terminator
    {
        [Description("")]
        NA,
        [Description("\r")]
        CR,
        [Description("\n")]
        LF,
        [Description("\r\n")]
        CRLF,
   
    }
}
