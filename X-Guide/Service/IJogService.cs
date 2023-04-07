using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.Service.Communication;

namespace X_Guide.Service
{
    public interface IJogService
    {
        void Enqueue(JogCommand jogCommand);
        void Start();
        void Stop();
    }
}
