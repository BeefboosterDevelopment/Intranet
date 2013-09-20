using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Beefbooster.Web;
using Microsoft.Reporting.WebForms;


public partial class UserControls_Report022 : System.Web.UI.UserControl
{
    private const string RPT_PARAMS = "Rpt022_reportParameters";
    private Rpt022_CowPerformance rptHelper;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Remove(RPT_PARAMS);
        }
        else
            rptHelper = (Rpt022_CowPerformance)Session[RPT_PARAMS];
    }

    protected void GenerateReport(object sender, CommandEventArgs e)
    {
        int herdSN = BBHerdYearSelector1.HerdSN;
        int yearBorn = BBHerdYearSelector1.YearNumber;
        int sortOrder = int.Parse(ddlSortOrder.SelectedValue);
        rptHelper = new Rpt022_CowPerformance(herdSN, yearBorn, sortOrder);

        Session.Add(RPT_PARAMS, rptHelper);

        rv.LocalReport.ReportPath = "ReportDefinitions/22_CowPerformance.rdlc";
        rv.LocalReport.DataSources.Clear();


        // 
        // retrieve all data into a list of Rpt011_DataItem objects
        //
        IEnumerable<Rpt022_DataItem> lst = rptHelper.GetData();

        LocalReport lrpt = rv.LocalReport;

        //
        // set this report's data source 
        //
        ReportDataSource rpt22DataSource = new ReportDataSource("Rpt022_DataItem", lst);
        lrpt.DataSources.Add(rpt22DataSource);

        IList<ReportParameter> rpt22ParameterList = new List<ReportParameter>();
        rpt22ParameterList.Add(new ReportParameter("rpStrainCode", rptHelper.BeefboosterHerd.Strain));
        rpt22ParameterList.Add(new ReportParameter("rpHerdCode", rptHelper.BeefboosterHerd.Code));
        rpt22ParameterList.Add(new ReportParameter("rpYearBorn", rptHelper.YearBorn.ToString()));
        rpt22ParameterList.Add(new ReportParameter("rpRanchName", rptHelper.BeefboosterHerd.RanchName));
        rpt22ParameterList.Add(new ReportParameter("rpBreederName", rptHelper.BeefboosterHerd.BreederName));

        // set all parameter values at once
        lrpt.SetParameters(rpt22ParameterList);

        // show the report
        rv.DataBind();

        if (rdoOutputType.SelectedValue == "PDF")
            BBWebUtility.OutputToPDF(lrpt, rptHelper.YearBorn + "_" + rptHelper.BeefboosterHerd.Code + "_Cow_Performance", true, true, Response);
        else
            BBWebUtility.OutputToExcel(lrpt, rptHelper.YearBorn + "_" + rptHelper.BeefboosterHerd.Code + "_Cow_Performance", Response);

    }

}
