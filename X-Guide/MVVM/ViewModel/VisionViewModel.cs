using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace X_Guide.MVVM.ViewModel
{
    public class VisionViewModel : ViewModelBase, ICloneable
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;


        public string Ip { get; set; } = string.Empty;

        public int Port { get; set; }

        public string Terminator { get; set; } = string.Empty;

        public string Filepath { get; set; } = string.Empty;

        public int Software { get; set; }

        public object Clone()
        {
            return new VisionViewModel
            {
                Id = Id,
                Name = Name,
                Port = Port,
                Ip = Ip,
                Terminator = Terminator,
                Filepath = Filepath,
                Software = Software
            };

        }






    }
}
