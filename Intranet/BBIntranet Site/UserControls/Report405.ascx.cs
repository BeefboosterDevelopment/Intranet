using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

using Beefbooster.Web;

public partial class Report405 : System.Web.UI.UserControl
{
    private Rpt405_DataObject rptHelper;

    #region Properties

    private readonly BreederReportData _breederInfo = null;
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

        rptHelper = new Rpt405_DataObject(yearBorn, strain, reportStyle);

        rv.LocalReport.ReportPath = "ReportDefinitions/405_SireCertificates.rdlc";

        rv.LocalReport.DataSources.Clear();

        // retrieve all data into a list of Rpt011_DataItem objects
        IList<Rpt405_DataItem> lst = rptHelper.GetData();

        LocalReport lrpt = rv.LocalReport;

        // set this report's data source 
        ReportDataSource Rpt405DataSource = new ReportDataSource("Rpt405_DataItem", lst);
        lrpt.DataSources.Add(Rpt405DataSource);

        IList<ReportParameter> Rpt405ParameterList = new List<ReportParameter>();
        Rpt405ParameterList.Add(new ReportParameter("rpYearBorn", rptHelper.YearBorn.ToString()));
        Rpt405ParameterList.Add(new ReportParameter("rpStrainCode", rptHelper.StrainCode));
        Rpt405ParameterList.Add(new ReportParameter("rpReportStyle", rptHelper.ReportStyle));
        Rpt405ParameterList.Add(new ReportParameter("rpReportYear", rptHelper.ReportYear));

        // set all parameter values at once
        lrpt.SetParameters(Rpt405ParameterList);

        // show the report
        rv.DataBind();

        BBWebUtility.OutputToPDF(lrpt, rptHelper.YearBorn + "_" + rptHelper.StrainCode + "_SireCertificates", false, false, Response);

    }

}
