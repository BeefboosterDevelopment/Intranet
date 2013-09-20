namespace Beefbooster.Web
{
    /// <summary>
    /// WebConfigSettings assists in instantiating WebConfigLoader class.
    /// WebConfigSettings instantiated only once.
    /// </summary>

    public class WebConfigSettings
    {
        public static WebConfigLoader Configurations;
        public WebConfigSettings()
        {
            Configurations = new WebConfigLoader();
        }
    }
}
