namespace X_Guide.Service.Communication
{
    public class JogCommand
    {
        //JOG,TOOL,{jogDistance},0,0,0,0,0,0,0\r\n
        public string Mode { get; set; } = "TOOL";

        public string ManipulatorName { get; set; }
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

        public JogCommand SetManipulatorName(string manipulatorName)
        {
            ManipulatorName = manipulatorName;
            return this;
        }

        public JogCommand SetMode(int mode)
        {
            Mode = mode == 0 ? "TOOL" : "GLOBAL";
            return this;
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

        public void Reset()
        {
            ResetCoordinate();
            ManipulatorName = Mode = "";
            Speed = Acceleration = 0;
        }

        public void ResetCoordinate()
        {
            X = Y = Z = RX = RY = RZ = 0;
        }

        public override string ToString()
        {
            string command = $"JOG,{ManipulatorName},{Mode},{X},{Y},{Z},{RZ},{RX},{RY},{Speed},{Acceleration}";
            ResetCoordinate();
            return command;
        }
    }
}