using System.Net;

namespace X_Guide.MVVM.Model
{
    public class HikVisionModel
    {
        private string _ip;

        public string Ip
        {
            get { return _ip; }
            set
            {
                _ip = IPAddress.TryParse(value, out _) ? value : "127.0.0.1";
            }
        }

        public int Port { get; set; }
        public string Terminator { get; set; }
        public string Filepath { get; set; }
    }
}