using System.Web;
using Microsoft.Reporting.WebForms;

namespace Beefbooster.Web
{

    /// <summary>
    /// Summary description for Util
    /// </summary>
    public static class BBWebUtility
    {
        //public BBWebUtility()
        //{
        //}
        
        private static SessionCache _userCache;
        public static SessionCache UserCache
        {
            get { return _userCache ?? (_userCache = new SessionCache()); }
        }

        public static void OutputToExcel(LocalReport localReport, string fileName, HttpResponse Response)
        {
            const string reportType = "Excel";
            string mimeType;
            string encoding;
            string fileNameExtension;


            // The DeviceInfo settings should be changed based on the reportType
            //      http://msdn2.microsoft.com/en-us/library/ms155397.aspx

            const string deviceInfo = "<DeviceInfo>" +
                                      "  <OutputFormat>Excel</OutputFormat>" +
                                      "  <OmitDocumentMap>false</OmitDocumentMap>" +
                                      "  <OmitFormulas>false</OmitFormulas>" +
                                      "  <RemoveSpace>0.125in</RemoveSpace>" +
                                      "  <SimplePageHeaders>false</SimplePageHeaders>" +
                                      "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            //Render the report
            byte[] renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            // Clear the response stream and write the bytes to the outputstream
            // Set content-disposition to "attachment" so that user is prompted to take an action
            // on the file (open or save)
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + fileNameExtension);
            Response.BinaryWrite(renderedBytes);
            Response.End();
        }

        public static void OutputToPDF(LocalReport localReport, string fileName, bool landscape, bool legal, HttpResponse Response)
        {
            const string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;


            // The DeviceInfo settings should be changed based on the reportType
            //      http://msdn2.microsoft.com/en-us/library/ms155397.aspx

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>" + (landscape ? (legal ? "14" : "11") : "8.5") + "in</PageWidth>" +
            "  <PageHeight>" + (landscape ? "8.5" : (legal ? "14" : "11")) + "in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.25in</MarginLeft>" +
            "  <MarginRight>0.25in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

            /*  other attributes for the DeviceInfo are
                StartPage - The first page of the report to render. A value of 0 indicates that all pages are rendered. The default value is 1.
                Columns - The number of columns to set for the report. This value overrides the report's original settings.
                ColumnSpacing - The column spacing to set for the report. This value overrides the report's original settings.
                EndPage - The last page of the report to render. The default value is the value for StartPage.
            */

            Warning[] warnings;
            string[] streams;

            //Render the report
            byte[] renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            // Clear the response stream and write the bytes to the outputstream
            // Set content-disposition to "attachment" so that user is prompted to take an action
            // on the file (open or save)
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + fileNameExtension);
            Response.BinaryWrite(renderedBytes);
            Response.End();
        }
    }
}