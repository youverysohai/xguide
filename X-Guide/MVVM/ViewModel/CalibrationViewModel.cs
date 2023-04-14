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
        private ManipulatorViewModel Manipulator { get; set; }
        private VisionViewModel Vision { get; set; }
        public int Mode { get; set; }
        public int Orientation { get; set; }
        private double CXOffSet { get; set; }
        private double CYOffset { get; set; }
        private double CRZOffset { get; set; }
        private double Speed { get; set; }
        private double Acceleration { get; set; }
        private double MotionDelay { get; set; }
        private double Mm_per_pixel { get; set; }
        private string Procedure { get; set; }

    }
}
