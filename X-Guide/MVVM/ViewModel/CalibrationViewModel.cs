using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.ViewModel;

namespace X_Guide.MVVM.ViewModel.CalibrationWizardSteps
{
    public class CalibrationViewModel : ViewModelBase
    {



        public int Id { get; set; }
        public string Name { get; set; }
        public ManipulatorViewModel Manipulator { get; set; }
        public VisionViewModel Vision { get; set; }
        public bool Mode { get; set; }
        public int Orientation { get; set; }
        public double XOffset { get; set; }
        public double YOffset { get; set; }
        public double CXOffSet { get; set; }
        public double CYOffset { get; set; }
        public double CRZOffset { get; set; }
        public double Speed { get; set; }
        public double Acceleration { get; set; }
        public double MotionDelay { get; set; }
        public double Mm_per_pixel { get; set; }
        public string Procedure { get; set; }

    }
}
