using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Reporting.WebForms;


public partial class UserControls_Report011 : System.Web.UI.UserControl
{
    private const string RPT_PARAMS = "reportParameters";
    private Rpt011_PreWeanBullCalfQualifier rptHelper;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            // add the report scope items
            ddlReportScope.Items.Clear();
            for (int i = 0; i < Rpt011_PreWeanBullCalfQualifier.ReportScopeValues.Length; i++)
                ddlReportScope.Items.Add(new ListItem(Rpt011_PreWeanBullCalfQualifier.ReportScopeValues[i], i.ToString()));

            Session.Remove(RPT_PARAMS);
        }
        else
            rptHelper = (Rpt011_PreWeanBullCalfQualifier)Session[RPT_PARAMS];
    }
    private void putFormData()
    {

        this.ddlMinBDateMonth.SelectedIndex = ddlMinBDateMonth.Items.IndexOf(ddlMinBDateMonth.Items.FindByValue(rptHelper.MaxBirthMonth.ToString()));
        this.ddlMinBDateDay.SelectedIndex = ddlMinBDateDay.Items.IndexOf(ddlMinBDateDay.Items.FindByValue(rptHelper.MaxBirthDay.ToString()));
        this.tbMinBWT.Text = rptHelper.MinBWT.ToString();
        this.tbMaxBWT.Text = rptHelper.MaxBWT.ToString();
        this.cbIncludePulled.Checked = (rptHelper.IncludePulledCalves == 1);
        this.cbIncludeHeiferCalves.Checked = (rptHelper.IncludeCalvesFromHeifers == 1);
    }
    private void getFormData()
    {
        rptHelper.MinBWT = int.Parse(this.tbMinBWT.Text);
        rptHelper.MaxBWT = int.Parse(this.tbMaxBWT.Text);
        rptHelper.IncludePulledCalves = this.cbIncludePulled.Checked ? 1 : 0;
        rptHelper.IncludeCalvesFromHeifers = this.cbIncludeHeiferCalves.Checked ? 1 : 0;
        rptHelper.ReportScope = int.Parse(ddlReportScope.SelectedValue);
    }
    protected void GenerateReport(object sender, CommandEventArgs e)
    {

        getFormData();

        rv011.LocalReport.ReportPath = "ReportDefinitions/11_PreliminaryBullCalfQualifier.rdlc";
        rv011.LocalReport.DataSources.Clear();


        // 
        // retrieve all data into a list of Rpt011_DataItem objects
        //
        IEnumerable<Rpt011_DataItem> lst = rptHelper.GetData();

        LocalReport lrpt = rv011.LocalReport;

        //
        // set this report's data source 
        //
        ReportDataSource rpt11DataSource = new ReportDataSource("Rpt011_DataItem", lst);
        lrpt.DataSources.Add(rpt11DataSource);



        IList<ReportParameter> rpt11ParameterList = new List<ReportParameter>();
        rpt11ParameterList.Add(new ReportParameter("rpScope", rptHelper.ReportScopeDescription));
        rpt11ParameterList.Add(new ReportParameter("rpStrainCode", rptHelper.StrainCode));
        rpt11ParameterList.Add(new ReportParameter("rpHerdCode", rptHelper.HerdCode));
        rpt11ParameterList.Add(new ReportParameter("rpYearBorn", rptHelper.YearBorn.ToString()));
        rpt11ParameterList.Add(new ReportParameter("rpRanchName", rptHelper.RanchName));
        rpt11ParameterList.Add(new ReportParameter("rpBreederName", rptHelper.BreederName));
        rpt11ParameterList.Add(new ReportParameter("rpMaxBirthDate", rptHelper.MaxBirthDate.ToString("dd-MMM-yyyy")));
        rpt11ParameterList.Add(new ReportParameter("rpMaxBWT", rptHelper.MaxBWT.ToString()));
        rpt11ParameterList.Add(new ReportParameter("rpMinBWT", rptHelper.MinBWT.ToString()));
        bool inclPulled = (rptHelper.IncludePulledCalves == 1);
        rpt11ParameterList.Add(new ReportParameter("rpIncludePulled", inclPulled.ToString()));
        bool inclHeifer = (rptHelper.IncludeCalvesFromHeifers == 1);
        rpt11ParameterList.Add(new ReportParameter("rpIncludeHeiferCalves", inclHeifer.ToString()));

        // set all parameter values at once
        lrpt.SetParameters(rpt11ParameterList);


        // show the report
        rv011.DataBind();

    }
    protected void SetDefaults(object sender, CommandEventArgs e)
    {
        int herdSN = int.Parse(ddlHerd.SelectedValue);
        int yearBorn = int.Parse(ddlYear.SelectedValue);
        int rptScope = int.Parse(ddlReportScope.SelectedValue);

        rptHelper = new Rpt011_PreWeanBullCalfQualifier(herdSN, yearBorn, rptScope);

        Session.Add(RPT_PARAMS, rptHelper);
        putFormData();
    }

}
