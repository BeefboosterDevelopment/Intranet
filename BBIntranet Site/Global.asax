<%@ Import namespace="Beefbooster.Web"%>
<%@ Application Language="C#" %>

<script runat="server">
    public static WebConfigSettings objWebConfigSettings; 
    void Application_Start(Object sender, EventArgs e)
    {
        try
        {
            objWebConfigSettings = new WebConfigSettings();
            ////Check if the debugging is enabled
            //if (HttpContext.Current.IsDebuggingEnabled)
            //{
            //    //Send email  
            //}    
        }
        catch //(Exception ex)
        {
            //HttpContext objContext = HttpContext.Current;
            //objContext.Response.Write(ex.InnerException.ToString());
            //TODO: Log Errors
            Application_EndRequest(sender, e);
        }

    }
    void Application_EndRequest(Object sender, EventArgs e)
    {

    }
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
