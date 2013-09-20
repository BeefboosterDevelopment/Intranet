<%@ WebHandler Language="C#" Class="Beefbooster.IntranetSite.Handler" %>

using System.Web;
using System.IO;
using Beefbooster.BusinessLogic;


namespace Beefbooster.IntranetSite
{
        //class ViewDocumentEvent : WebSystemEvent
        //{
    //        public ViewDocumentEvent(CarbonDocument document, HttpRequest request) 
    //            : base(LoggingConstants.ViewDocumentItemId,  request)
    //        {
    //            _params.Add("DocName", document.Name);
    //            _params.Add("DocId", document.DocumentId.ToString());
    //            _params.Add("DocVersion", document.VersionNumber.ToString());
    //       }
        //}

    public class Handler : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            if (context != null)
            {
                string fileToSend = context.Request.Params["FileToSend"] as string;
            }
        }   


        public void SendOneFile(HttpContext context, string zipFileName)
        {
            //FileInfo fi = new FileInfo(zipFileName);
            byte[] bytes = File.ReadAllBytes(zipFileName);   

            // send the file
            ClientFileStreamer s = new ClientFileStreamer(context);
            s.SendFileToClient(bytes, zipFileName);
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}