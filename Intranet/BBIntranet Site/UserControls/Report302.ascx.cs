using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Beefbooster.DataAccessLibrary.Domain;
using Microsoft.Reporting.WebForms;

using Beefbooster.Web;

public partial class Report302 : System.Web.UI.UserControl
{
    private Rpt302_DataObject rptHelper;

    #region Properties

    private readonly BBHerd _bbHerd = null;
    public BBHerd Herd
    {
        get
        {
            return _bbHerd;
        }
    }


    //private BreederReportData _breederInfo = null;
    //public BreederReportData BreederInfo
    //{
    //    get
    //    {
    //        return _breederInfo;
    //    }
    //}

    //private string _herdCode;    // = Profile.BBHerdCode;
    //public string HerdCode
    //{
    //    get
    //    {
    //        return _herdCode;
    //    }
    //    set
    //    {
    //        _herdCode = value;
    //    }
    //}

    //private string _strainCode;  // = Profile.BreederStrainCode;
    //public string StrainCode
    //{
    //    get
    //    {
    //        return _strainCode;
    //    }
    //    set
    //    {
    //        _strainCode = value;
    //    }
    //}

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
        //int herdSN = ucBBStrainHerdYearSelector.HerdSN;
        int yearBorn = ucBBStrainHerdYearSelector.YearNumber;

        rptHelper = new Rpt302_DataObject(yearBorn, strain, ucBBStrainHerdYearSelector.HerdSN, style);

        rv.LocalReport.ReportPath = "ReportDefinitions/302_BullPerformance.rdlc";
        rv.LocalReport.DataSources.Clear();

        // retrieve all data into a list of Rpt302_DataItem objects
        IList<Rpt302_DataItem> lst = rptHelper.GetData();

        LocalReport lrpt = rv.LocalReport;

        // set this report's data source 
        ReportDataSource Rpt302DataSource = new ReportDataSource("Rpt302_DataItem", lst);
        lrpt.DataSources.Add(Rpt302DataSource);

        IList<ReportParameter> Rpt302ParameterList = new List<ReportParameter>();
        Rpt302ParameterList.Add(new ReportParameter("rpYearBorn", rptHelper.YearBorn.ToString()));
        Rpt302ParameterList.Add(new ReportParameter("rpStrainCode", rptHelper.Strain));
        Rpt302ParameterList.Add(new ReportParameter("rpReportStyle", rptHelper.ReportStyle));
        Rpt302ParameterList.Add(new ReportParameter("rpReportYear", rptHelper.ReportYear));
        if (rptHelper.Herd != null)
        {
            Rpt302ParameterList.Add(new ReportParameter("rpHerdCode", rptHelper.Herd.Code));
            Rpt302ParameterList.Add(new ReportParameter("rpHerdDescription", rptHelper.Herd.Description));
            Rpt302ParameterList.Add(new ReportParameter("rpBreederName", rptHelper.Herd.BreederName));
        }
        else
        {
            Rpt302ParameterList.Add(new ReportParameter("rpHerdCode", ""));
            Rpt302ParameterList.Add(new ReportParameter("rpHerdDescription", ""));
            Rpt302ParameterList.Add(new ReportParameter("rpBreederName", ""));
        }

        // set all parameter values at once
        lrpt.SetParameters(Rpt302ParameterList);

        // show the report
        rv.DataBind();

        string rptName;
        if (rptHelper.Herd != null)
            rptName = rptHelper.YearBorn + "_" + rptHelper.Strain + "_" + rptHelper.Herd.Code + "_BullPerformance";
        else
            rptName = rptHelper.YearBorn + "_" + rptHelper.Strain + "_BullPerformance";

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
