using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.Model
{
    public class VisionModel
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public string Terminator { get; set; }
        public string Name { get; set; }
        public string Filepath { get; set; }

        public int Software { get; set; }
    }
}
