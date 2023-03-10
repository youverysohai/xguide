using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.Communication.Service
{
    public interface IClientService
    {
        Task ConnectServer();
    }
}
