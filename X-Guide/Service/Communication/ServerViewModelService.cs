using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.Service.Communation;

namespace X_Guide.Service.Communication
{
    public class ServerViewModelService
    {
        ServerCommand _command;

        public ServerViewModelService(ServerCommand command)
        {
            _command = command;
        }


    }
}
