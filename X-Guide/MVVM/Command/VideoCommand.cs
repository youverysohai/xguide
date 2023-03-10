using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.Service;

namespace X_Guide.MVVM.Command
{
    internal class VideoCommand : CommandBase
    {
        private ImageService _imageService;

        public VideoCommand(ImageService imageService)
        {
            _imageService = imageService;
        }
        public override void Execute(object parameter)
        {
            string command = parameter.ToString();
            if(command == "Start")
            {
                _imageService.StartCamera();
            }
            else if(command == "Stop")
            {
                _imageService.StopCamera();
            }
        }
    }
}
