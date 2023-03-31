//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace X_Guide
{
    using System;
    using System.Collections.Generic;
    
    public partial class Calibration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ManipulatorId { get; set; }
        public Nullable<int> MountingOrientation { get; set; }
        public string VisionFilePath { get; set; }
        public Nullable<double> RobotSpeed { get; set; }
        public Nullable<double> RobotAccel { get; set; }
        public Nullable<double> MotionDelay { get; set; }
        public Nullable<double> XOffset { get; set; }
        public Nullable<double> YOffset { get; set; }
        public Nullable<double> CameraXScaling { get; set; }
        public Nullable<double> CameraYScaling { get; set; }
        public Nullable<double> RZOffset { get; set; }
        public string VisionFlow { get; set; }
    
        public virtual Manipulator Manipulator { get; set; }
    }
}
