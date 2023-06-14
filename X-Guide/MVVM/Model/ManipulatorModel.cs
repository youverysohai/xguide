using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using VM.Core;
using X_Guide.Enums;

namespace X_Guide.MVVM.Model
{
    public class ManipulatorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ManipulatorType Type { get; set; }
        public string Description { get; set; }
        
        public ManipulatorModel()
        {
          
            Name = "Default";
            Type = default;
            Description = "Default";
        }

        public ManipulatorModel(int id, string name, ManipulatorType type, string description)
        {
            Id = id;
            Name = name;
            Type = type;
            Description = description;
        }
    }
}
