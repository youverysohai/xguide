using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Guide.Service.Communication
{
    public class JogCommand
    {
        //JOG,TOOL,{jogDistance},0,0,0,0,0,0,0\r\n
        public string Mode { get; set; } = "TOOL";
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double RX { get; set; }
        public double RY { get; set; }
        public double RZ { get; set; }
        public double Speed { get; set; }
        public double Acceleration { get; set; }

        public JogCommand()
        {
            
        }

        public JogCommand SetX(double x)
        {
            X = x;
            return this;
        }

        public JogCommand SetY(double y)
        {
            Y = y;
            return this;
        }
        public JogCommand SetZ(double z)
        {
            Z = z;
            return this;
        }

        public JogCommand SetRZ(double rz)
        {
            RZ = rz;
            return this;
        }

        public JogCommand SetSpeed(double speed)
        {
            Speed = speed;
            return this;
        }

        public JogCommand SetAcceleration(double acceleration)
        {
            Acceleration = acceleration;
            return this;
        }

        public override string ToString()
        {
            return $"JOG,{Mode},{X},{Y},{Z},{RZ},{RX},{RY},{Speed},{Acceleration}";
        }

    }
}
