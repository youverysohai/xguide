using System.Configuration;

namespace X_Guide
{
    public class HikVisionConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("HikIp")]
        public string Ip
        {
            get { return (string)this["HikIp"]; }
            set { this["HikIp"] = value; }
        }

        [ConfigurationProperty("HikPort")]
        public int Port
        {
            get
            {
                return int.TryParse(this["HikPort"].ToString(), out int result) ? result : default;
            }
            set { this["HikPort"] = value; }
        }

        [ConfigurationProperty("HikFilepath")]
        public string Filepath
        {
            get { return (string)this["HikFilepath"]; }
            set { this["HikFilepath"] = value; }
        }

        [ConfigurationProperty("HikTerminator")]
        public string Terminator
        {
            get { return (string)this["HikTerminator"]; }
            set { this["HikTerminator"] = value; }
        }
    }
}