using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Security;
using Microsoft.Reporting.WebForms;

using Beefbooster.Web;

public partial class Report402 : System.Web.UI.UserControl
{
    private Rpt402_DataObject rptHelper;

    #region Properties

    private BreederReportData _breederInfo = null;
    public BreederReportData BreederInfo
    {
        get
        {
            return _breederInfo;
        }
    }

    private string _strainCode; 
    public string StrainCode
    {
        get
        {
            return _strainCode;
        }
        set
        {
            _strainCode = value;
        }
    }

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

        string strain = ddlStrain.SelectedValue;
        string reportStyle = string.Empty;

        rptHelper = new Rpt402_DataObject(yearBorn, strain, reportStyle);

        rv.LocalReport.ReportPath = "ReportDefinitions/402_SireSelectionSheets.rdlc";

        rv.LocalReport.DataSources.Clear();

        // retrieve all data into a list of Rpt011_DataItem objects
        IList<Rpt402_DataItem> lst = rptHelper.GetData();

        LocalReport lrpt = rv.LocalReport;

        // set this report's data source 
        ReportDataSource Rpt402DataSource = new ReportDataSource("Rpt402_DataItem", lst);
        lrpt.DataSources.Add(Rpt402DataSource);

        IList<ReportParameter> Rpt402ParameterList = new List<ReportParameter>();
        Rpt402ParameterList.Add(new ReportParameter("rpYearBorn", rptHelper.YearBorn.ToString()));
        Rpt402ParameterList.Add(new ReportParameter("rpStrainCode", rptHelper.StrainCode));
        Rpt402ParameterList.Add(new ReportParameter("rpReportStyle", rptHelper.ReportStyle));
        Rpt402ParameterList.Add(new ReportParameter("rpReportYear", rptHelper.ReportYear));

        // set all parameter values at once
        lrpt.SetParameters(Rpt402ParameterList);

        // show the report
        rv.DataBind();

        BBWebUtility.OutputToPDF(lrpt, rptHelper.YearBorn + "_" + rptHelper.StrainCode + "_SireSelectionSheets", true, false, this.Response);

        //if (rdoOutputType.SelectedValue == "PDF")
        //    new Util().OutputToPDF(lrpt, rptHelper.StartingYearBorn + "_" + rptHelper.StrainCode + "_BullPerformance", true, true, this.Response);
        //else
        //    new Util().OutputToExcel(lrpt, rptHelper.StartingYearBorn + "_" + rptHelper.StrainCode + "_BullPerformance", this.Response);

    }

}
