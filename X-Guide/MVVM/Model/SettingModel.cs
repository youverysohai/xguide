using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Xml.Serialization;

namespace X_Guide.MVVM.Model
{
    /*
     * 1.Parameterless constructor 
     * 2.Constructor less scanner
     */
    public class SettingModel
    {


        public string LogFilePath { get; set; }


        public SettingModel()
        {
            LogFilePath = "";
        }

        public SettingModel(string logFilePath)
        {

            LogFilePath = logFilePath;
        }

        public static SettingModel ReadFromXML(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    return (SettingModel)new XmlSerializer(typeof(SettingModel)).Deserialize(reader);



                }

            }
            catch
            {
                //Implement message
                return new SettingModel();
            }
        }
        public void WriteToXML(string filePath)
        {
            CheckDirectory(filePath);

            var writer = new XmlSerializer(typeof(SettingModel));

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
