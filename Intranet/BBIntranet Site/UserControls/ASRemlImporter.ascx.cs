using System;
using System.Configuration;
using System.IO;
using Beefbooster.ASREML;
using Beefbooster.BusinessLogic;
using Beefbooster.Web;


public partial class ASRemlProcessorControl : System.Web.UI.UserControl
{
    //  By default the maximum size of the uploaded file is 4 MB. If you try to upload a bigger file, you’ll get a
    //  runtime error. To change this restriction, modify the maxRequestLength attribute of the <httpRuntime> setting in
    //  the application’s web.config file. The size is specified in kilobytes, so <httpRuntime maxRequestLength="8192"/>
    //  sets the maximum file size to 8 MB.

    protected void Page_Load(object sender, EventArgs e)
    {
        txtResult.Text = string.Empty;
        txtStatusMessage.Text = string.Empty;
        btnGetStatus.Enabled = false;
    }


    protected void btnValidate_Click(object sender, EventArgs e)
    {
        if (txtUploadedFile.Text.Length != 0)
        {
            // Extract the filename part from the full path of the original file.
            var ebvDataFileName = txtUploadedFile.Text;

            var sqlServerBulkCopyFolder = ConfigurationManager.AppSettings["ASRemlBulkInsertFolder"];
            var localBulkCopyFolder = ConfigurationManager.AppSettings["ASRemlDataFilesFolder"];
            var ebvConnectionString = ConfigurationManager.ConnectionStrings["EBVConnectionString"].ConnectionString;
            var controlNumber =  getControlNumber();
           
            var importer = new ASREMLImport(ebvDataFileName, sqlServerBulkCopyFolder, localBulkCopyFolder, ebvConnectionString, controlNumber);

            txtStrainCode.Text = importer.Strain;
            btnProcessFile.Enabled = importer.Validate();
        }
    }

    private int getControlNumber()
    {
        if (BBWebUtility.UserCache[CacheStaticValues.EBVControlNumber] == null)
            return Constants.InitializeInt;

        var strControlNumber = BBWebUtility.UserCache[CacheStaticValues.EBVControlNumber].ToString();

        if (string.IsNullOrEmpty(strControlNumber))
            throw new ApplicationException("No EBV control number found.");

        return (Convert.ToInt32(strControlNumber));
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (uploader.PostedFile.ContentLength == 0)
        {
            // no data
        }
        else
        {
            string destDir = Server.MapPath("./Uploads");

            // Extract the filename part from the full path of the original file.
            string fileName = Path.GetFileName(uploader.PostedFile.FileName);

            // Combine the destination directory with the filename.
            string destPath = Path.Combine(destDir, fileName);

            // Save the file on the server.
            uploader.PostedFile.SaveAs(destPath);

            txtUploadedFile.Text = destPath;
        }
    }

    protected void btnProcessFile_Click(object sender, EventArgs e)
    {
        txtResult.Text = string.Empty;
        txtStatusMessage.Text = string.Empty;
        btnGetStatus.Enabled = false;

        if (txtUploadedFile.Text.Length != 0)
        {
            // Extract the filename part from the full path of the original file.
            string ebvDataFileName = uploader.PostedFile.FileName;

            var sqlServerBulkCopyFolder = ConfigurationManager.AppSettings["ASRemlBulkInsertFolder"];
            var localBulkCopyFolder = ConfigurationManager.AppSettings["ASRemlDataFilesFolder"];
            var ebvConnectionString = ConfigurationManager.ConnectionStrings["EBVConnectionString"].ConnectionString;
            var controlNumber = getControlNumber();

            
            // Do the work
            var importer = new ASREMLImport(txtUploadedFile.Text, sqlServerBulkCopyFolder, localBulkCopyFolder, ebvConnectionString, controlNumber);

            txtStrainCode.Text = importer.Strain;

            if (importer.Validate())
            {
                int status = importer.ImportData();
                if (status == 1)
                {
                    txtResult.Text = "Success";
                    txtResult.ForeColor = System.Drawing.Color.Green;
                    btnGetStatus.Enabled = false;
                }
                else
                {
                    txtResult.Text = "Fail";
                    txtResult.ForeColor = System.Drawing.Color.Red;
                    btnGetStatus.Enabled = true;
                }
            }
            else
            {
                throw new ApplicationException(string.Format("{0} is invalid.", ebvDataFileName));
            }
        }
    }

    protected void btnGetStatus_Click(object sender, EventArgs e)
    {
        var sqlServerBulkCopyFolder = ConfigurationManager.AppSettings["ASRemlBulkInsertFolder"];
        var localBulkCopyFolder = ConfigurationManager.AppSettings["ASRemlDataFilesFolder"];
        var ebvConnectionString = ConfigurationManager.ConnectionStrings["EBVConnectionString"].ConnectionString;
        var controlNumber = getControlNumber();

        var importer = new ASREMLImport(txtUploadedFile.Text, sqlServerBulkCopyFolder, localBulkCopyFolder, ebvConnectionString, controlNumber);

        txtStatusMessage.Text = importer.GetStatusMessage();
    }


}
