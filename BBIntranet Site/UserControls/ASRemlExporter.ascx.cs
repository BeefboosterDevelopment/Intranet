using System;
using System.Configuration;
using System.IO;
using System.Web;
using Beefbooster.ASREML;


public partial class ASRemlGeneratorControl : System.Web.UI.UserControl
{
    private const string DEFAULT_LAST_RESORT_FOLDER = "C:\\TEMP\\ASReml\\Export\\";


    protected void Page_Load(object sender, EventArgs e)
    {
        btnDownload.Enabled = (lblZipFileName.Text.Length > 0);
    }




    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        lblZipFileName.Text = string.Empty;
        hdnZipFilePath.Value = string.Empty;

        var rootFolder = ConfigurationManager.AppSettings["ASRemlDataFilesFolder"];
        if (string.IsNullOrEmpty(rootFolder)) rootFolder = DEFAULT_LAST_RESORT_FOLDER;

        var bullConnectionString = ConfigurationManager.ConnectionStrings["BullConnectionString"].ConnectionString;
        var cowCalfConnectionString = ConfigurationManager.ConnectionStrings["CowCalfConnectionString"].ConnectionString;


        var exporter = new ASREMLExport(rootFolder,cowCalfConnectionString,bullConnectionString);       

        var zipFileName = exporter.CreateZip();

        if (zipFileName != null)
        {
            var fi = new FileInfo(zipFileName);
            lblZipFileName.Text = fi.Name;
            hdnZipFilePath.Value = fi.FullName;
        }

        btnDownload.Enabled = (lblZipFileName.Text.Length > 0);
    }



    protected void btnDownload_Click(object sender, EventArgs e)
    {
        string zipFileName = hdnZipFilePath.Value;
        FileInfo fi = new FileInfo(zipFileName);
        if (fi.Exists)
        {
            // convert it to a byte array
            byte[] bytes = File.ReadAllBytes(zipFileName);

            // send the file
            ClientFileStreamer streamer = new ClientFileStreamer(HttpContext.Current);
            streamer.SendFileToClient(bytes, zipFileName);
        }
        else 
            throw new ApplicationException(string.Format("{0} not found.", zipFileName));

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        hdnZipFilePath.Value = null;
        lblZipFileName.Text = string.Empty;
        btnDownload.Enabled = false;
    }
}
