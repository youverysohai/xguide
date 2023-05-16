using System;

namespace X_Guide.MVVM.ViewModel
{
    public class HikVisionViewModel : ViewModelBase, ICloneable
    {
        public string Ip { get; set; } = string.Empty;

        public int Port { get; set; } = 0;

        public string Terminator { get; set; } = string.Empty;

        public string Filepath { get; set; } = string.Empty;

        public object Clone()
        {
            return new HikVisionViewModel
            {
                Port = Port,
                Ip = Ip,
                Terminator = Terminator,
                Filepath = Filepath,
            };
        }
    }
}