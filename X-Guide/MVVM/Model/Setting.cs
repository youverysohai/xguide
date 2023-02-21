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
    public class Setting
    {
        public string MachineID { get; set; }


        public string MachineDescription { get; set; }


        public string SoftwareRevision { get; set; }

        public string RobotIP { get; set; }


        public string RobotPort { get; set; }


        public string ShiftStartTime { get; set; }


        public string VisionIP { get; set; }


        public string VisionPort { get; set; }


        public string MaxScannerCapTime { get; set; }


        public string LogFilePath { get; set; }


        /*  public string Error
          {
              get
              {
                  return null;
              }
          }

          public string this[string columnName]
          {
              get
              {
                  string error = string.Empty;
                  if (columnName == "MachineID")
                      if (string.IsNullOrEmpty(MachineID)) error = "Machine ID is required.";

                  return error;
              }

          }*/

        public Setting()
        {
            MachineID = "M01";
            MachineDescription = "Machine Description";
            SoftwareRevision = "v1.0";
            RobotIP = "127.0.0.1";
            RobotPort = "23";
            ShiftStartTime = "00:00:00";
            VisionIP = "127.0.0.1";
            VisionPort = "23";
            MaxScannerCapTime = "100";
            LogFilePath = "";
        }

        public Setting(string machineID, string machineDescription, string softwareRevision, string robotIP, string robotPort, string startTime, string visionIP, string visionPort, string maxScannerCapTime, string logFilePath)
        {
            MachineID = machineID;
            MachineDescription = machineDescription;
            SoftwareRevision = softwareRevision;
            RobotIP = robotIP;
            RobotPort = robotPort;
            ShiftStartTime = startTime;
            VisionIP = visionIP;
            VisionPort = visionPort;
            MaxScannerCapTime = maxScannerCapTime;
            LogFilePath = logFilePath;
        }
        public static Setting ReadFromXML(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    return (Setting)new XmlSerializer(typeof(Setting)).Deserialize(reader);



                }

            }
            catch
            {

                return new Setting();
            }
        }
        public void WriteToXML(string filePath)
        {
            CheckDirectory(filePath);
 

            var writer = new XmlSerializer(typeof(Setting));

            using (TextWriter file = new StreamWriter(filePath))
            {
                writer.Serialize(file, this);
            }
        }

        private void CheckDirectory(string filePath)
        { filePath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(filePath)) { Directory.CreateDirectory(filePath); }

        }
    }
}
