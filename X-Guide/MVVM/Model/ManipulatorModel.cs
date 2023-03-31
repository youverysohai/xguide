using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using VM.Core;

namespace X_Guide.MVVM.Model
{
    public class ManipulatorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Terminator { get; set; }

        
        public ManipulatorModel()
        {
          
            Name = "Default";
            Type = 1;
            Description = "Default";
            Ip = "127.0.0.1";
            Port = "8000";
            Terminator = string.Empty;
        }

        public ManipulatorModel(int id, string name, int type, string description, string manipulatorIP, string manipulatorPort, string manipulatorTerminator)
        {
            Id = id;
            Name = name;
            Type = type;
            Description = description;
            Ip = manipulatorIP;
            Port = manipulatorPort;
            Terminator = manipulatorTerminator;
        }
    }
}
