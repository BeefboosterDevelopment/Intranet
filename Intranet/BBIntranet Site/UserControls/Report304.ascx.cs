using System;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Beefbooster.DataAccessLibrary.Domain;
using Microsoft.Reporting.WebForms;
using Beefbooster.Web;

public partial class Report304 : System.Web.UI.UserControl
{
    private Rpt304_DataObject _detailsDataObject;
    private Rpt304StrainSummaryDataObject _strainSummaryDataObject;

    #region Properties

    private readonly BBHerd _bbHerd = null;
    public BBHerd Herd
    {
        get
        {
            return _bbHerd;
        }
    }
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        // default to current year
        if (!IsPostBack)
            ucBBStrainHerdYearSelector.YearNumber = DateTime.Now.Year;
    }

    protected void GenerateReport(object sender, CommandEventArgs e)
    {
        string style = string.Empty;
        string strain = ucBBStrainHerdYearSelector.StrainCode;
        int yearBorn = ucBBStrainHerdYearSelector.YearNumber;
        int sortOrder = int.Parse(ddlSortOrder.SelectedValue);
        decimal medRel = decimal.Parse(tbMedReliability.Text);
        decimal highRel = decimal.Parse(tbHighReliability.Text);


        rv.LocalReport.ReportPath = "ReportDefinitions/304_BullPerfEBV.rdlc";
        rv.LocalReport.DataSources.Clear();

        // fill up the data objects
        _detailsDataObject = new Rpt304_DataObject(yearBorn, strain, ucBBStrainHerdYearSelector.HerdSN, style, sortOrder, medRel, highRel);
        IList<Rpt304_DataItem> lstDetails = _detailsDataObject.GetData();

        _strainSummaryDataObject = new Rpt304StrainSummaryDataObject(lstDetails);
        IEnumerable<Rpt304StrainSummaryItem> strainSummary = _strainSummaryDataObject.Summarize();

        LocalReport localReport = rv.LocalReport;

        // -----------------------------
        // set the reports data sources
        // -----------------------------
        // details
        ReportDataSource rpt304DetailDataSource = new ReportDataSource("Rpt304_DataItem", lstDetails);
        localReport.DataSources.Add(rpt304DetailDataSource);

        // strain summary
        ReportDataSource rpt304StrainSummaryDataSource = new ReportDataSource("Rpt304StrainSummaryItem", strainSummary);
        localReport.DataSources.Add(rpt304StrainSummaryDataSource);

        IList<ReportParameter> rpt304ParameterList = new List<ReportParameter>();
        rpt304ParameterList.Add(new ReportParameter("rpYearBorn", _detailsDataObject.YearBorn.ToString(CultureInfo.InvariantCulture)));
        rpt304ParameterList.Add(new ReportParameter("rpStrainCode", _detailsDataObject.Strain));
        rpt304ParameterList.Add(new ReportParameter("rpReportStyle", _detailsDataObject.ReportStyle));
        rpt304ParameterList.Add(new ReportParameter("rpReportYear", _detailsDataObject.ReportYear));
        rpt304ParameterList.Add(new ReportParameter("rpMedReliability", _detailsDataObject.MediumReliability.ToString(CultureInfo.InvariantCulture)));
        rpt304ParameterList.Add(new ReportParameter("rpHighReliability", _detailsDataObject.HighReliability.ToString(CultureInfo.InvariantCulture)));

        if (_detailsDataObject.Herd != null)
        {
            rpt304ParameterList.Add(new ReportParameter("rpHerdCode", _detailsDataObject.Herd.Code));
            rpt304ParameterList.Add(new ReportParameter("rpHerdDescription", _detailsDataObject.Herd.Description));
            rpt304ParameterList.Add(new ReportParameter("rpBreederName", _detailsDataObject.Herd.BreederName));
        }
        else
        {
            rpt304ParameterList.Add(new ReportParameter("rpHerdCode", ""));
            rpt304ParameterList.Add(new ReportParameter("rpHerdDescription", ""));
            rpt304ParameterList.Add(new ReportParameter("rpBreederName", ""));
        }

        // set all parameter values at once
        localReport.SetParameters(rpt304ParameterList);

        // show the report
        rv.DataBind();

        string rptName;
        if (_detailsDataObject.Herd != null)
            rptName = _detailsDataObject.YearBorn + "_" + _detailsDataObject.Strain + "_" + _detailsDataObject.Herd.Code + "_BullTestPerformance";
        else
            rptName = _detailsDataObject.YearBorn + "_" + _detailsDataObject.Strain + "_BullTestPerformance";

        if (rdoOutputType.SelectedValue == "PDF")
        {
            BBWebUtility.OutputToPDF(localReport, rptName, true, true, Response);
        }
        else
        {
            BBWebUtility.OutputToExcel(localReport, rptName, Response);
        }

    }

}
