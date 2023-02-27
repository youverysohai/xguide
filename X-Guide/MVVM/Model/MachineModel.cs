using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.MVVM.Model
{
    internal class MachineModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string ManipulatorIP { get; set; }
        public string ManipulatorPort { get; set; }
        public string VisionIP { get; set; }

        public string VisionPort { get; set; }


        public MachineModel()
        {
            Name = "Default";
            Type = 1;
            Description = "Default";
            ManipulatorIP = "127.0.0.1";
            ManipulatorPort = "8000";
            VisionIP = "127.0.0.1";
            VisionPort = "8000";
        }

        public MachineModel(int id, string name, int type, string description, string manipulatorIP, string manipulatorPort, string visionIP, string visionPort)
        {
            Id = id;
            Name = name;
            Type = type;
            Description = description;
            ManipulatorIP = manipulatorIP;
            ManipulatorPort = manipulatorPort;
            VisionIP = visionIP;
            VisionPort = visionPort;
        }
    }
}
