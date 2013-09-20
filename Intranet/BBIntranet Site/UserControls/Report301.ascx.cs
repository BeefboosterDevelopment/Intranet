using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

using Beefbooster.Web;

public partial class Report301 : System.Web.UI.UserControl
{
    private Rpt301_DataObject rptHelper;
    
    protected void GenerateReport(object sender, CommandEventArgs e)
    {
        int yearBorn = Int32.Parse(ddlYearBorn.SelectedValue);
        if (yearBorn < 0)
        {
            if (DateTime.Now.Month <= 6)
                yearBorn = DateTime.Now.Year - 1;
            else
                yearBorn = DateTime.Now.Year;
        }

        string strain = ddlStrain.SelectedValue;
        string style = ddlReportStyle.SelectedValue;

        rptHelper = new Rpt301_DataObject(yearBorn, strain, style);

        if (rptHelper.ReportStyle.ToUpper() == "FEET AND LEGS")
        {
            rvWorksheet.LocalReport.ReportPath = "ReportDefinitions/301_FeetLegs.rdlc";
        }
        else if (rptHelper.ReportStyle.ToUpper() == "OFF TEST")
        {
            rvWorksheet.LocalReport.ReportPath = "ReportDefinitions/301_OffTest.rdlc";
        }
        else if (rptHelper.ReportStyle.ToUpper() == "BSE")
        {
            rvWorksheet.LocalReport.ReportPath = "ReportDefinitions/301_BSE.rdlc";
        }
        else if (rptHelper.ReportStyle.ToUpper() == "WEIGHT")
        {
            rvWorksheet.LocalReport.ReportPath = "ReportDefinitions/301_Wt.rdlc";
        }

        rvWorksheet.LocalReport.DataSources.Clear();

        // retrieve all data into a list of Rpt301_DataItem objects
        var lst = rptHelper.GetData();

        LocalReport lrpt = rvWorksheet.LocalReport;

        // set this report's data source 
        ReportDataSource rpt301DataSource = new ReportDataSource("Rpt301_DataItem", lst);
        lrpt.DataSources.Add(rpt301DataSource);

        IList<ReportParameter> rpt301ParameterList = new List<ReportParameter>();
        rpt301ParameterList.Add(new ReportParameter("rpYearBorn", rptHelper.YearBorn.ToString()));
        rpt301ParameterList.Add(new ReportParameter("rpStrainCode", rptHelper.StrainCode));
        rpt301ParameterList.Add(new ReportParameter("rpReportStyle", rptHelper.ReportStyle));
        rpt301ParameterList.Add(new ReportParameter("rpReportYear", rptHelper.ReportYear));

        // set all parameter values at once
        lrpt.SetParameters(rpt301ParameterList);

        // show the report
        rvWorksheet.DataBind();

        if (rdoOutputType.SelectedValue == "PDF")
            BBWebUtility.OutputToPDF(lrpt, rptHelper.YearBorn + "_" + rptHelper.StrainCode + "_Worksheet_" + rptHelper.ReportStyle, true, false, Response);
        else
            BBWebUtility.OutputToExcel(lrpt, rptHelper.YearBorn + "_" + rptHelper.StrainCode + "_Worksheet_" + rptHelper.ReportStyle, Response);

    }
}
