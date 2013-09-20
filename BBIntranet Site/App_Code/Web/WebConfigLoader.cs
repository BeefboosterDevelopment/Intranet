using System;
using System.Configuration;

namespace Beefbooster.Web
{
    /// <summary>
    /// WebConfigLoader assists in having strongly typed values from web.config (namely from the appSettings and connectionStrings sections) to avoid typos.
    /// It also acts as a central repository of all session variables used in the  application. 
    /// The values from these sections of the web.config must be loaded through the WebConfigLoader. Whenever the new value is added to the web.config file appSettings and connectionStrings sections, developers should add them as the private members of the WebConfigAppSettings class and to add the read access modifier. 
    /// This class also assists in performance boost as it helps to avoid casting the object that ends up loaded in the .NET Framework ConfigurationManager to the appropriate type. 
    /// Use read-only access modifier as this class is not allowing to write to the web.config (and this is something that should never be done for security purposes).
    /// To add a value stored in a web.config, follow this pattern:
    /// <example>
    /// <code>
    /// //value for the LoginMaxAllowed
    /// private int loginMaxAllowed;
    /// public int LoginMaxAllowed
    /// {
    ///		get
    ///		{
    ///			return loginMaxAllowed;
    ///		}
    /// }
    /// </example>
    /// </code> 
    /// </summary>
    public class WebConfigLoader
    {
        #region Connection Strings
        private string _cowCalf_ConnectionString;
        private string _bull_ConnectionString;
        private string _eBV_ConnectionString;
        #endregion

        #region Application Settings
        private string _aSRemlBulkInsertFolder;
        private string _aSRemlDataFilesFolder;
        #endregion

        #region Properties
        public string CowCalf_ConnectionString
        {
            get
            {
                return _cowCalf_ConnectionString;
            }
        }
        public string Bull_ConnectionString
        {
            get
            {
                return _bull_ConnectionString;
            }
        }
        public string EBV_ConnectionString
        {
            get
            {
                return _eBV_ConnectionString;
            }
        }
        public string ASRemlBulkInsertFolder
        {
            get { return _aSRemlBulkInsertFolder; }
        }
        public string ASRemlDataFilesFolder
        {
            get { return _aSRemlDataFilesFolder; }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public WebConfigLoader()
        {
            LoadConfigSettings();
        }

        private static void AlertSiteShutDown(string errorMessage)
        {
            throw new ApplicationException(errorMessage);
            //  send error that site is down
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadConfigSettings()
        {
            if (ConfigurationManager.AppSettings["ASRemlBulkInsertFolder"] != null)
            {
                _aSRemlBulkInsertFolder = Convert.ToString(ConfigurationManager.AppSettings["ASRemlBulkInsertFolder"]);
            }
            else
            {
                AlertSiteShutDown("CRITICAL: ASRemlBulkInsertFolder value is not set in the AppSettings section of web.config");
            }

            if (ConfigurationManager.AppSettings["ASRemlDataFilesFolder"] != null)
            {
                _aSRemlDataFilesFolder = Convert.ToString(ConfigurationManager.AppSettings["ASRemlDataFilesFolder"]);
            }
            else
            {
                AlertSiteShutDown("CRITICAL: ASRemlDataFilesFolder value is not set in the AppSettings section of web.config");
            }

            #region Connection String Retrieval

            if (ConfigurationManager.ConnectionStrings["CowCalfConnectionString"] != null)
            {
                _cowCalf_ConnectionString = ConfigurationManager.ConnectionStrings["CowCalfConnectionString"].ToString();
            }
            else
            {
                AlertSiteShutDown("CRITICAL: CowCalfConnectionString is not set in the ConnectionStrings settings!");
            }

            if (ConfigurationManager.ConnectionStrings["BullConnectionString"] != null)
            {
                _bull_ConnectionString = ConfigurationManager.ConnectionStrings["BullConnectionString"].ToString();
            }
            else
            {
                AlertSiteShutDown("CRITICAL: BullConnectionString is not set in the ConnectionStrings settings!");
            }

            if (ConfigurationManager.ConnectionStrings["EBVConnectionString"] != null)
            {
                _eBV_ConnectionString = ConfigurationManager.ConnectionStrings["EBVConnectionString"].ToString();
            }
            else
            {
                AlertSiteShutDown("CRITICAL: EBVConnectionString is not set in the ConnectionStrings settings!");
            }
            #endregion

        }
    }
}
