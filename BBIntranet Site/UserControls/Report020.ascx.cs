using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using Beefbooster.BusinessLogic;
using Beefbooster.Web;
using Microsoft.Reporting.WebForms;

public partial class UserControls_Report020 : System.Web.UI.UserControl
{
    private const string RptParams = "Rpt020_reportParameters";
    private Rpt020_BullCalfSelection _rptHelper;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Remove(RptParams);
        }
        else
        {
            _rptHelper = (Rpt020_BullCalfSelection)Session[RptParams];
        }
    }
    private void PutFormData()
    {
        ddlMinBDateMonth.SelectedIndex = ddlMinBDateMonth.Items.IndexOf(ddlMinBDateMonth.Items.FindByValue(_rptHelper.MaxBirthMonth.ToString(CultureInfo.InvariantCulture)));
        ddlMinBDateDay.SelectedIndex = ddlMinBDateDay.Items.IndexOf(ddlMinBDateDay.Items.FindByValue(_rptHelper.MaxBirthDay.ToString(CultureInfo.InvariantCulture)));
        tbMinBWT.Text = _rptHelper.MinBWT.ToString(CultureInfo.InvariantCulture);
        tbMaxBWT.Text = _rptHelper.MaxBWT.ToString(CultureInfo.InvariantCulture);
        if (_rptHelper.MinADG != null) tbMinADG.Text = _rptHelper.MinADG.Value.ToString(CultureInfo.InvariantCulture);
        tbNumCalves.Text = _rptHelper.TopN.ToString(CultureInfo.InvariantCulture);
        cbIncludePulled.Checked = (_rptHelper.IncludePulledCalves);
        cbIncludeHeiferCalves.Checked = (_rptHelper.IncludeCalvesFromHeifers);
        cboSortOrder.SelectedIndex = cboSortOrder.Items.IndexOf(cboSortOrder.Items.FindByValue("SI"));
    }
    private void GetFormData()
    {
        _rptHelper.MaxBirthMonth = int.Parse(ddlMinBDateMonth.SelectedValue);
        _rptHelper.MaxBirthDay = int.Parse(ddlMinBDateDay.SelectedValue);
        _rptHelper.MinBWT = int.Parse(tbMinBWT.Text);
        _rptHelper.MaxBWT = int.Parse(tbMaxBWT.Text);
        _rptHelper.MinADG = decimal.Parse(tbMinADG.Text);
        _rptHelper.TopN = int.Parse(tbNumCalves.Text);
        _rptHelper.IncludePulledCalves = cbIncludePulled.Checked;
        _rptHelper.IncludeCalvesFromHeifers = cbIncludeHeiferCalves.Checked;
        _rptHelper.ReportMode = cboSortOrder.SelectedValue;
    }

    protected void SetDefaults(object sender, EventArgs e)
    {
        if (ucBBHerdYearSelector.HerdSN == Constants.InitializeInt)
        {
            ddlMinBDateMonth.SelectedIndex = ddlMinBDateMonth.Items.IndexOf(ddlMinBDateMonth.Items.FindByValue("MAY"));
            ddlMinBDateDay.SelectedIndex = ddlMinBDateDay.Items.IndexOf(ddlMinBDateDay.Items.FindByValue("15"));
            tbMinBWT.Text = "70";
            tbMaxBWT.Text = "150";
            tbMinADG.Text = "1.5";
            tbNumCalves.Text = "100";
            cbIncludePulled.Checked = true;
            cbIncludeHeiferCalves.Checked = true;
            cboSortOrder.SelectedIndex = cboSortOrder.Items.IndexOf(cboSortOrder.Items.FindByValue("SI"));
        }
        else
        {
            _rptHelper = new Rpt020_BullCalfSelection(ucBBHerdYearSelector.HerdSN, ucBBHerdYearSelector.YearNumber);
            Session.Add(RptParams, _rptHelper);
            PutFormData();
        }
    }

    protected void GenerateReport(object sender, CommandEventArgs e)
    {
        if (_rptHelper == null)
        {
            _rptHelper = new Rpt020_BullCalfSelection(ucBBHerdYearSelector.HerdSN, ucBBHerdYearSelector.YearNumber);
            Session.Add(RptParams, _rptHelper);
            PutFormData();
        }

        GetFormData();

        rv020.LocalReport.ReportPath = "ReportDefinitions/20_BullCalfSelection.rdlc";
        rv020.LocalReport.DataSources.Clear();

        // retrieve all data into a list of Rpt020_DataItem objects
        IEnumerable<Rpt020_DataItem> lst = _rptHelper.GetData();

        LocalReport lrpt = rv020.LocalReport;

        // set this report's data source 
        var rpt20DataSource = new ReportDataSource("Rpt020_DataItem", lst);
        lrpt.DataSources.Add(rpt20DataSource);

        IList<ReportParameter> rpt20ParameterList = new List<ReportParameter>();
        rpt20ParameterList.Add(new ReportParameter("rpStrainCode", _rptHelper.BeefboosterHerd.Strain));
        rpt20ParameterList.Add(new ReportParameter("rpHerdCode", _rptHelper.BeefboosterHerd.Code));
        rpt20ParameterList.Add(new ReportParameter("rpYearBorn", _rptHelper.YearBorn.ToString(CultureInfo.InvariantCulture)));
        rpt20ParameterList.Add(new ReportParameter("rpRanchName", _rptHelper.BeefboosterHerd.RanchName));
        rpt20ParameterList.Add(new ReportParameter("rpBreederName", _rptHelper.BeefboosterHerd.BreederName));
        rpt20ParameterList.Add(new ReportParameter("rpMaxBDate", _rptHelper.MaxBirthDate.ToString("dd-MMM-yyyy")));
        rpt20ParameterList.Add(new ReportParameter("rpBWT", _rptHelper.MinBWT + " to " + _rptHelper.MaxBWT));
        if (_rptHelper.MinADG != null)
            rpt20ParameterList.Add(new ReportParameter("rpMinADG", _rptHelper.MinADG.Value.ToString(CultureInfo.InvariantCulture)));
        var strTop = _rptHelper.TopN + " of " + _rptHelper.NumberQualifyingCalves + " qualified";
        rpt20ParameterList.Add(new ReportParameter("rpNTop", strTop));
        rpt20ParameterList.Add(new ReportParameter("rpNumberQualifyingCalves", "100"));

        rpt20ParameterList.Add(new ReportParameter("rpIncludePulledCalves", (_rptHelper.IncludePulledCalves) ? "Yes" : "No"));
        rpt20ParameterList.Add(new ReportParameter("rpIncludeHeiferCalves", (_rptHelper.IncludeCalvesFromHeifers) ? "Yes" : "No"));

        // set all parameter values at once
        lrpt.SetParameters(rpt20ParameterList);

        // show the report
        rv020.DataBind();

        if (rdoOutputType.SelectedValue == "PDF")
            BBWebUtility.OutputToPDF(lrpt, _rptHelper.YearBorn + "_" + _rptHelper.BeefboosterHerd.Code + "_BullCalf_Selection", true, true, Response);
        else
            BBWebUtility.OutputToExcel(lrpt, _rptHelper.YearBorn + "_" + _rptHelper.BeefboosterHerd.Code + "_BullCalf_Selection", Response);

    }
}
