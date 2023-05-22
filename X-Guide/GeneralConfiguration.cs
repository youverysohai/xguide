using System.Configuration;

namespace X_Guide
{
    public class GeneralConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("Filepath")]
        public string Filepath
        {
            get { return (string)this["Filepath"]; }
            set { this["Filepath"] = value; }
        }

        [ConfigurationProperty("Debug")]
        public bool Debug
        {
            get { return bool.Parse(this["Debug"].ToString()); }
            set { this["Debug"] = value; }
        }

        [ConfigurationProperty("Ip")]
        public string Ip
        {
            get { return (string)this["Ip"]; }
            set { this["Ip"] = value; }
        }

        [ConfigurationProperty("Port")]
        public int Port
        {
            get { return int.TryParse(this["Port"].ToString(), out int result) ? result : default; }
            set { this["Port"] = value; }
        }

        [ConfigurationProperty("Terminator")]
        public string Terminator
        {
            get { return (string)this["Terminator"]; }
            set { this["Terminator"] = value; }
        }

        [ConfigurationProperty("VisionSoftware")]
        public int VisionSoftware
        {
            get
            {
                return int.TryParse(this["VisionSoftware"].ToString(), out int result) ? result : 0;
            }
            set { this["VisionSoftware"] = value.ToString(); }
        }
    }
}