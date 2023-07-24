using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XGuideSQLiteDB.Models
{
    public class Calibration
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public Nullable<int> ManipulatorId { get; set; }
        public Nullable<int> Orientation { get; set; }
        public Nullable<double> Speed { get; set; }
        public Nullable<double> Acceleration { get; set; }
        public Nullable<int> MotionDelay { get; set; }
        public Nullable<int> XOffset { get; set; }
        public Nullable<int> YOffset { get; set; }
        public int? XMove { get; set; }
        public int? YMove { get; set; }
        public double MMPerPixel { get; set; }
        public double CRZOffset { get; set; }
        public double CYOffset { get; set; }
        public double CXOffset { get; set; }
        public string Procedure { get; set; }
        public bool Mode { get; set; }
        public double? JointRotationAngle { get; set; }

        [ForeignKey(nameof(ManipulatorId))]
        public virtual Manipulator Manipulator { get; set; }

        public override string ToString()
        {
            return $"Calibration [Id: {Id}, Name: {Name}, ManipulatorId: {ManipulatorId}, Orientation: {Orientation}, Speed: {Speed}, " +
                   $"Acceleration: {Acceleration}, MotionDelay: {MotionDelay}, XOffset: {XOffset}, YOffset: {YOffset}, " +
                   $"MMPerPixel: {MMPerPixel}, CRZOffset: {CRZOffset}, " +
                   $"CYOffset: {CYOffset}, CXOffset: {CXOffset}, Procedure: {Procedure}, " +
                   $"Mode: {Mode}, JointRotationAngle: {JointRotationAngle}]";
        }
    }
}