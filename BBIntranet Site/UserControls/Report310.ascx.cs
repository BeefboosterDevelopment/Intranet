using System;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using Beefbooster.Web;

namespace UserControls
{
    public partial class Report310 : System.Web.UI.UserControl
    {
        private Rpt310_DataObject _detailsDataObject;

        protected void Page_Load(object sender, EventArgs e)
        {
            // default to 15 years ago
//            if (!IsPostBack)
//                ucBBBreederSelector.
        }

        protected void GenerateReport(object sender, CommandEventArgs e)
        {
            string accountNo1 = ucBBBreederSelector1.AccountNo;
            string accountNo2 = ucBBBreederSelector2.AccountNo;
            decimal medRel = decimal.Parse(tbMedReliability.Text);
            decimal highRel = decimal.Parse(tbHighReliability.Text);

            
            rv.LocalReport.ReportPath = "ReportDefinitions/310_BullBattery.rdlc";
            rv.LocalReport.DataSources.Clear();

            // fill up the data objects
            _detailsDataObject = new Rpt310_DataObject(accountNo1, accountNo2, medRel, highRel);
            IEnumerable<Rpt310_DataItem> lstDetails = _detailsDataObject.GetData();

            LocalReport localReport = rv.LocalReport;

            // -----------------------------
            // set the reports data sources
            // -----------------------------
            // details
            ReportDataSource rpt310DetailDataSource = new ReportDataSource("Rpt310_DataItem", lstDetails);
            localReport.DataSources.Add(rpt310DetailDataSource);

            IList<ReportParameter> rpt310ParameterList = new List<ReportParameter>();
            if (_detailsDataObject.Breeder1 != null)
                rpt310ParameterList.Add(new ReportParameter("rpBreederName1", _detailsDataObject.Breeder1.ContactName));
            if (_detailsDataObject.Breeder2 != null)
                rpt310ParameterList.Add(new ReportParameter("rpBreederName2", _detailsDataObject.Breeder2.ContactName));
            rpt310ParameterList.Add(new ReportParameter("rpMedReliability", _detailsDataObject.MediumReliability.ToString(CultureInfo.InvariantCulture)));
            rpt310ParameterList.Add(new ReportParameter("rpHighReliability", _detailsDataObject.HighReliability.ToString(CultureInfo.InvariantCulture)));

            // set all parameter values at once
            localReport.SetParameters(rpt310ParameterList);

            // show the report
            rv.DataBind();

            if (_detailsDataObject.Breeder1 != null)
            {
                string rptName = _detailsDataObject.Breeder1.Company + "_BullBatteryPerformance";

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

    }
}
