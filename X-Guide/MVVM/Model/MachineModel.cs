using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using VM.Core;

namespace X_Guide.MVVM.Model
{
    public class MachineModel : ModelBase
    {

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value;
                OnPropertyChanged();
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value;
                OnPropertyChanged();
            }
        }
        private int _type;

        public int Type
        {
            get { return _type; }
            set { _type = value;
                OnPropertyChanged();
            }
        }
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        private string _manipulatorIP;

        public string ManipulatorIP
        {
            get { return _manipulatorIP; }
            set { _manipulatorIP = value; OnPropertyChanged(); }
        }

        private string _manipulatorPort;

        public string ManipulatorPort
        {
            get { return _manipulatorPort; }
            set { _manipulatorPort = value; OnPropertyChanged(); }
        }
        private string _visionIP;

        public string VisionIP
        {
            get { return _visionIP; }
            set { _visionIP = value; OnPropertyChanged(); }
        }
        private string _visionPort;

        public string VisionPort
        {
            get { return _visionPort; }
            set { _visionPort = value; OnPropertyChanged(); }
        }

        private string _manipulatorTerminator;

        public string ManipulatorTerminator
        {
            get { return _manipulatorTerminator; }
            set { _manipulatorTerminator = value; OnPropertyChanged(); }
        }
        private string _visionTerminator;

        public string VisionTerminator
        {
            get { return _visionTerminator; }
            set { _visionTerminator = value; OnPropertyChanged(); }
        }






        public MachineModel()
        {
            Name = "Default";
            Type = 1;
            Description = "Default";
            ManipulatorIP = "127.0.0.1";
            ManipulatorPort = "8000";
            VisionIP = "127.0.0.1";
            VisionPort = "8000";
            ManipulatorTerminator = "NA";
            VisionTerminator = "NA";
        }

        public MachineModel(int id, string name, int type, string description, string manipulatorIP, string manipulatorPort, string visionIP, string visionPort, string manipulatorTerminator, string visionTerminator)
        {
            Id = id;
            Name = name;
            Type = type;
            Description = description;
            ManipulatorIP = manipulatorIP;
            ManipulatorPort = manipulatorPort;
            VisionIP = visionIP;
            VisionPort = visionPort;
            ManipulatorTerminator = manipulatorTerminator;
            VisionTerminator = visionTerminator;
        }
    }
}
