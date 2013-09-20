using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Beefbooster.DataAccessLibrary.Domain;
using Microsoft.Reporting.WebForms;

using Beefbooster.Web;

public partial class Report303 : System.Web.UI.UserControl
{
    private Rpt303_DataObject rptHelper;

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

        rptHelper = new Rpt303_DataObject(yearBorn, strain, ucBBStrainHerdYearSelector.HerdSN, style);

        rv.LocalReport.ReportPath = "ReportDefinitions/303_BullPerfData.rdlc";
        rv.LocalReport.DataSources.Clear();

        // retrieve all data into a list of Rpt303_DataItem objects
        IList<Rpt303_DataItem> lst = rptHelper.GetData();

        LocalReport lrpt = rv.LocalReport;

        // set this report's data source 
        ReportDataSource Rpt303DataSource = new ReportDataSource("Rpt303_DataItem", lst);
        lrpt.DataSources.Add(Rpt303DataSource);

        IList<ReportParameter> Rpt303ParameterList = new List<ReportParameter>();
        Rpt303ParameterList.Add(new ReportParameter("rpYearBorn", rptHelper.YearBorn.ToString()));
        Rpt303ParameterList.Add(new ReportParameter("rpStrainCode", rptHelper.Strain));
        Rpt303ParameterList.Add(new ReportParameter("rpReportStyle", rptHelper.ReportStyle));
        Rpt303ParameterList.Add(new ReportParameter("rpReportYear", rptHelper.ReportYear));
        if (rptHelper.Herd != null)
        {
            Rpt303ParameterList.Add(new ReportParameter("rpHerdCode", rptHelper.Herd.Code));
            Rpt303ParameterList.Add(new ReportParameter("rpHerdDescription", rptHelper.Herd.Description));
            Rpt303ParameterList.Add(new ReportParameter("rpBreederName", rptHelper.Herd.BreederName));
        }
        else
        {
            Rpt303ParameterList.Add(new ReportParameter("rpHerdCode", ""));
            Rpt303ParameterList.Add(new ReportParameter("rpHerdDescription", ""));
            Rpt303ParameterList.Add(new ReportParameter("rpBreederName", ""));
        }

        // set all parameter values at once
        lrpt.SetParameters(Rpt303ParameterList);

        // show the report
        rv.DataBind();

        string rptName;
        if (rptHelper.Herd != null)
            rptName = rptHelper.YearBorn + "_" + rptHelper.Strain + "_" + rptHelper.Herd.Code + "_BullPerfData";
        else
            rptName = rptHelper.YearBorn + "_" + rptHelper.Strain + "_BullPerfData";

        if (rdoOutputType.SelectedValue == "PDF")
        {
            BBWebUtility.OutputToPDF(lrpt, rptName, true, true, Response);
        }
        else
        {
            BBWebUtility.OutputToExcel(lrpt, rptName, Response);
        }

    }

}
