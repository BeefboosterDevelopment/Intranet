using System;
using System.Web;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Beefbooster.Web
{
    /// <summary>
    /// Contains the implementation for the site settings.
    /// </summary>
    public class SiteSettings
    {
        private const string XmlConfigFile = "~/App_Data/site-config.xml";

        private string _reportDataFolder;
        private string _siteName;
        private string _siteEmail;

        public string SiteName
        {
            get
            {
                return _siteName;
            }
            set
            {
                lock (this)
                {
                    _siteName = value;
                }
            }
        }

        public string SiteEmailAddress
        {
            get
            {
                return _siteEmail;
            }
            set
            {
                lock (this)
                {
                    _siteEmail = value;
                }
            }
        }

        public string SiteEmailFromField
        {
            get
            {
                return String.Format("{0} <{1}>", _siteName, _siteEmail);
            }
        }

        public string ReportDataFolder
        {
            get
            {
                return _reportDataFolder;
            }
            set
            {
                lock (this)
                {
                    _reportDataFolder = value;
                }
            }
        }

        public static SiteSettings LoadFromConfiguration()
        {
            SiteSettings s = LoadFromXml();

            if (s == null)
            {
                s = new SiteSettings();
                s.SiteName = "Beefbooster.com";
                s.SiteEmailAddress = "info@beefbooster.com";
                s.ReportDataFolder = "~/App_Data/XML/";

                SaveToXml(s);
            }
            return s;
        }
        public static SiteSettings GetSharedSettings()
        {
            return BeefboosterHttpApplication.BeefboosterApplicationSettings;
        }
        public static bool UpdateSettings(SiteSettings newSettings)
        {
            // write settings to code or db

            // update Application-wide settings, only over-writing settings that users should edit
            lock (BeefboosterHttpApplication.BeefboosterApplicationSettings)
            {
                // XML Report Data
                BeefboosterHttpApplication.BeefboosterApplicationSettings.ReportDataFolder = newSettings.ReportDataFolder;

                // Site Name
                BeefboosterHttpApplication.BeefboosterApplicationSettings.SiteName = newSettings.SiteName;

                // Contact Email Address for Site
                BeefboosterHttpApplication.BeefboosterApplicationSettings.SiteEmailAddress = newSettings.SiteEmailAddress;

                // Serialize to Xml Config File
                return SaveToXml(BeefboosterHttpApplication.BeefboosterApplicationSettings);
            }
        }
        private static SiteSettings LoadFromXml()
        {
            SiteSettings settings = null;

            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                string configPath = context.Server.MapPath(XmlConfigFile);

                XmlSerializer xml;
                FileStream fs = null;

                bool success = false;
                int numAttempts = 0;

                while (!success && numAttempts < 2)
                {
                    try
                    {
                        numAttempts++;
                        xml = new XmlSerializer(typeof(SiteSettings));
                        fs = new FileStream(configPath, FileMode.Open, FileAccess.Read);
                        settings = xml.Deserialize(fs) as SiteSettings;
                        success = true;
                    }
                    catch (Exception x)
                    {
                        // if an exception is thrown, there might have been a sharing violation;
                        // we wait and try again (max: two attempts)
                        success = false;
                        System.Threading.Thread.Sleep(1000);
                        if (numAttempts == 2)
                            throw new Exception("The site configuration could not be loaded.", x);
                    }
                }

                if (fs != null)
                    fs.Close();
            }

            return settings;

        }
        public string GetXml()
        {
            StringBuilder result = new StringBuilder();
            StringWriter s = new StringWriter(result);
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(SiteSettings));
                xml.Serialize(s, this);
            }
            finally
            {
                s.Close();
            }

            return result.ToString();

        }
        private static bool SaveToXml(SiteSettings settings)
        {
            if (settings == null)
                return false;

            HttpContext context = HttpContext.Current;
            if (context == null)
                return false;

            string configPath = context.Server.MapPath(XmlConfigFile);

            XmlSerializer xml;
            FileStream fs = null;

            bool success = false;
            int numAttempts = 0;

            while (!success && numAttempts < 2)
            {
                try
                {
                    numAttempts++;
                    xml = new XmlSerializer(typeof(SiteSettings));
                    fs = new FileStream(configPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                    xml.Serialize(fs, settings);
                    success = true;
                }
                catch
                {
                    // if an exception is thrown, there might have been a sharing violation;
                    // we wait and try again (max: two attempts)
                    success = false;
                    System.Threading.Thread.Sleep(1000);
                }
            }

            if (fs != null)
                fs.Close();

            return success;

        }
    }
}

