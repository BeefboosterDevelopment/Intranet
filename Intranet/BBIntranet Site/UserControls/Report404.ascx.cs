using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class Report404 : UserControl
{
    private Rpt404_DataObject _classDataObject;

    #region Properties

    public string StrainCode { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
    }


    protected void GenerateReport(object sender, CommandEventArgs e)
    {
        int yearBorn = DateTime.Now.Year;
        if (Int32.Parse(ddlYearBorn.SelectedValue) == -1)
        {
            if (DateTime.Now.Month <= 6)
                yearBorn--;
        }
        else
        {
            yearBorn = Int32.Parse(ddlYearBorn.SelectedValue);
        }

        decimal medRel = decimal.Parse(tbMedReliability.Text);
        decimal highRel = decimal.Parse(tbHighReliability.Text);

        string strain = ddlStrain.SelectedValue;
        string reportStyle = string.Empty;

        _classDataObject = new Rpt404_DataObject(yearBorn, strain, reportStyle, medRel, highRel);


        rv.LocalReport.ReportPath = "ReportDefinitions/404_SireSelectionSheets.rdlc";

        rv.LocalReport.DataSources.Clear();

        // retrieve data
        IList<Rpt404_DataItem> detailItems = _classDataObject.GetData();

        LocalReport lrpt = rv.LocalReport;

        // set this report's data source(s):

        // details
        var rpt404DataSource = new ReportDataSource("Rpt404_DataItem", detailItems);
        lrpt.DataSources.Add(rpt404DataSource);

        IList<ReportParameter> rpt404ParameterList = new List<ReportParameter>();
        rpt404ParameterList.Add(new ReportParameter("rpYearBorn",
                                                    _classDataObject.YearBorn.ToString(CultureInfo.InvariantCulture)));
        rpt404ParameterList.Add(new ReportParameter("rpStrainCode", _classDataObject.StrainCode));
        rpt404ParameterList.Add(new ReportParameter("rpReportStyle", _classDataObject.ReportStyle));
        rpt404ParameterList.Add(new ReportParameter("rpReportYear", _classDataObject.ReportYear));
        rpt404ParameterList.Add(new ReportParameter("rpMedReliability",
                                                    _classDataObject.MediumReliability.ToString(
                                                        CultureInfo.InvariantCulture)));
        rpt404ParameterList.Add(new ReportParameter("rpHighReliability",
                                                    _classDataObject.HighReliability.ToString(
                                                        CultureInfo.InvariantCulture)));
        // set all parameter values at once
        lrpt.SetParameters(rpt404ParameterList);

        // show the report
        rv.DataBind();
        OutputClientSireSelectionSheetsToPDF(lrpt,
                                             _classDataObject.YearBorn + "_" + _classDataObject.StrainCode +
                                             "_SireSelectionSheets",
                                             true, true, Response);
    }


    private static void OutputClientSireSelectionSheetsToPDF(LocalReport localReport, string fileName, bool landscape,
                                                             bool legal, HttpResponse response)
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
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>1.25in</MarginLeft>" +
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
        response.Clear();
        response.ContentType = mimeType;
        response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + fileNameExtension);
        response.BinaryWrite(renderedBytes);
        response.End();
    }
}