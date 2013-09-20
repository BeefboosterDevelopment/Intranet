namespace Beefbooster.Web
{
    /// <summary>
    /// Summary description for CacheStaticValues
    /// </summary>
    public class CacheStaticValues
    {

        #region Enumerations

        /// <summary>
        /// Match policy group names defined in webconfig
        /// </summary>
        public enum CacheProfileName
        {
            Menus,
            Groups,
            Accounts,
            LookUps
        }
        public enum CacheDuration
        {
            Low = 5,
            Medimum = 15,
            High = 30,
            UltraHigh = 60
        }

        #endregion

        public static string BreederList = "BBBreederss";
        public static string HerdList = "BBHerds";
        public static string YearList = "BBYearLetters";
        public static string LoggedOnBreederHerd = "BBLoggedOnBreederHerd";
        public static string LoggedOnBreederStrain= "BBLoggedOnBreederStrain";
        public static string EBVControlNumber = "EBVControlNumber";

        #region Cache Policies

        public static string CachePolicy = "CachePolicy";

        #endregion

    }
}