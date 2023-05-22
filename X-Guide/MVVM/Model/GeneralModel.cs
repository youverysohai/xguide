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
        private int _visionSoftware;

        public int VisionSoftware
        {
            get
            {
                if (_visionSoftware == 0) VisionSoftware = 1;
                return _visionSoftware;
            }
            set { _visionSoftware = value; }
        }

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

        public void WriteToXML(string filePath)
        {
            CheckDirectory(filePath);

            var writer = new XmlSerializer(typeof(GeneralModel));

            using (TextWriter file = new StreamWriter(filePath))
            {
                writer.Serialize(file, this);
            }
        }

        private void CheckDirectory(string filePath)
        {
            string filepath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(filepath)) { Directory.CreateDirectory(filePath); }
        }
    }
}