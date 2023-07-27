using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace XGuideSQLiteDB.Models
{
    public enum ManipulatorType
    {
        [Description("Gantry With Rotary Axis")]
        GantrySystemR = 1,

        [Description("Gantry Without Rotary Axis")]
        GantrySystemWR = 2,

        [Description("SCARA")]
        SCARA = 3,

        [Description("6-Axis")]
        SixAxis = 4,
    }

    [Serializable]
    public class Manipulator : IEntity
    {
        [Key]
        public int Id { get; set; }

        public ManipulatorType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Manipulator Id: {Id}, Type: {Type}, Name: {Name}, Description: {Description}";
        }
    }
}