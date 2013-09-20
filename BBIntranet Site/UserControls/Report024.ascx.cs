using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code.RPT;
using Beefbooster.BusinessLogic;
using Beefbooster.Web;
using Microsoft.Reporting.WebForms;

public partial class UserControls_Report024 : UserControl
{
    private const string RptParams = "Rpt024_reportParameters";
    private Rpt024HeiferSelection _rptHelper;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Remove(RptParams);
        }
        else
        {
            _rptHelper = (Rpt024HeiferSelection) Session[RptParams];
        }
    }

    private void PutFormData()
    {
        tbMinBWT.Text = _rptHelper.MinBWT.ToString(CultureInfo.InvariantCulture);
        tbMaxBWT.Text = _rptHelper.MaxBWT.ToString(CultureInfo.InvariantCulture);
        tbNumCalves.Text = _rptHelper.TopN.ToString(CultureInfo.InvariantCulture);
        cbIncludePulled.Checked = (_rptHelper.IncludePulledCalves);
        cbIncludeHeiferCalves.Checked = (_rptHelper.IncludeCalvesFromHeifers);
        cboSortOrder.SelectedIndex = cboSortOrder.Items.IndexOf(cboSortOrder.Items.FindByValue("SI"));
    }

    private void GetFormData()
    {
        _rptHelper.MinBWT = int.Parse(tbMinBWT.Text);
        _rptHelper.MaxBWT = int.Parse(tbMaxBWT.Text);
        _rptHelper.TopN = int.Parse(tbNumCalves.Text);
        _rptHelper.IncludePulledCalves = cbIncludePulled.Checked;
        _rptHelper.IncludeCalvesFromHeifers = cbIncludeHeiferCalves.Checked;
        _rptHelper.ReportMode = cboSortOrder.SelectedValue;
    }

    protected void btnDefaults_Click(object sender, EventArgs e)
    {
        if (ucBBHerdYearSelector.HerdSN == Constants.InitializeInt)
        {
            tbMinBWT.Text = "70";
            tbMaxBWT.Text = "150";
            tbNumCalves.Text = "100";
            cbIncludePulled.Checked = true;
            cbIncludeHeiferCalves.Checked = true;
            cboSortOrder.SelectedIndex = cboSortOrder.Items.IndexOf(cboSortOrder.Items.FindByValue("SI"));
        }
        else
        {
            _rptHelper = new Rpt024HeiferSelection(ucBBHerdYearSelector.HerdSN, ucBBHerdYearSelector.YearNumber);
            _rptHelper.SetDefaults();
            Session.Add(RptParams, _rptHelper);
            PutFormData();
        }
    }


    protected void GenerateReport(object sender, CommandEventArgs e)
    {
        if (_rptHelper == null)
        {
            _rptHelper = new Rpt024HeiferSelection(ucBBHerdYearSelector.HerdSN, ucBBHerdYearSelector.YearNumber);
            Session.Add(RptParams, _rptHelper);
            PutFormData();
        }

        GetFormData();

        rv024.LocalReport.ReportPath = "ReportDefinitions/24_HeiferSelection.rdlc";
        rv024.LocalReport.DataSources.Clear();

        // retrieve all data into a list of objects
        IEnumerable<Rpt024DataItem> lst = _rptHelper.GetData();

        LocalReport lrpt = rv024.LocalReport;

        // set this report's data source 
        var rpt20DataSource = new ReportDataSource("Rpt024DataItem", lst);
        lrpt.DataSources.Add(rpt20DataSource);

        IList<ReportParameter> rpt24ParameterList = new List<ReportParameter>();
        rpt24ParameterList.Add(new ReportParameter("rpStrainCode", _rptHelper.BeefboosterHerd.Strain));
        rpt24ParameterList.Add(new ReportParameter("rpHerdCode", _rptHelper.BeefboosterHerd.Code));
        rpt24ParameterList.Add(new ReportParameter("rpYearBorn",
                                                   _rptHelper.YearBorn.ToString(CultureInfo.InvariantCulture)));
        rpt24ParameterList.Add(new ReportParameter("rpRanchName", _rptHelper.BeefboosterHerd.RanchName));
        rpt24ParameterList.Add(new ReportParameter("rpBreederName", _rptHelper.BeefboosterHerd.BreederName));
        rpt24ParameterList.Add(new ReportParameter("rpBWT", _rptHelper.MinBWT + " to " + _rptHelper.MaxBWT));
        string strTop = _rptHelper.TopN + " of " + _rptHelper.NumberQualifyingCalves + " qualified";
        rpt24ParameterList.Add(new ReportParameter("rpNTop", strTop));
        rpt24ParameterList.Add(new ReportParameter("rpNumberQualifyingCalves", "100"));

        rpt24ParameterList.Add(new ReportParameter("rpIncludePulledCalves",
                                                   (_rptHelper.IncludePulledCalves) ? "Yes" : "No"));
        rpt24ParameterList.Add(new ReportParameter("rpIncludeHeiferCalves",
                                                   (_rptHelper.IncludeCalvesFromHeifers) ? "Yes" : "No"));

        // set all parameter values at once
        lrpt.SetParameters(rpt24ParameterList);

        // show the report
        rv024.DataBind();

        if (rdoOutputType.SelectedValue == "PDF")
            BBWebUtility.OutputToPDF(lrpt,
                                     _rptHelper.YearBorn + "_" + _rptHelper.BeefboosterHerd.Code + "_BullCalf_Selection",
                                     true, true, Response);
        else
            BBWebUtility.OutputToExcel(lrpt,
                                       _rptHelper.YearBorn + "_" + _rptHelper.BeefboosterHerd.Code +
                                       "_BullCalf_Selection", Response);
    }
}