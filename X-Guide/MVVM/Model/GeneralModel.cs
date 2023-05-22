using System.IO;
using System.Net;

using System.Xml.Serialization;

namespace X_Guide.MVVM.Model
{
    /*
     * 1.Parameterless constructor
     * 2.Constructor less scanner
     */

    public class GeneralModel
    {
        public string Filepath { get; set; } = "";
        public string Terminator { get; set; } = "";

        private string _ip;

        public string Ip
        {
            get { return _ip; }
            set => _ip = IPAddress.TryParse(value, out _) ? value : "127.0.0.1";
        }

        public int Port { get; set; } = 8080;
        public bool Debug { get; set; } = false;
        public int VisionSoftware { get; set; } = 1;

        public GeneralModel()
        {
        }

        public static GeneralModel ReadFromXML(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    return (GeneralModel)new XmlSerializer(typeof(GeneralModel)).Deserialize(reader);
                }
            }
            catch
            {
                //Implement message
                return new GeneralModel();
            }
        }
    }
}