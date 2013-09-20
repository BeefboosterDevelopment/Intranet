using System;


using Beefbooster.BusinessLogic;

public partial class TestStationReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PretestReportXML xmlGenerator = new PretestReportXML("PretestReport.xml");
        if (!xmlGenerator.CheckIntegrity())
        {
            xmlGenerator.Refresh();
        }
    }

    protected void RegenerateData(object sender, EventArgs e)
    {
        PretestReportXML objXML = new PretestReportXML("PretestReport.xml");
        objXML.Refresh();
    }
}
