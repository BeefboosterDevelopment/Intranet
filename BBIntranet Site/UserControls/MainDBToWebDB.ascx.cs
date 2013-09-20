using System;
using System.Web.UI.WebControls;


//TODO: Convert these methods to use connection strings

public partial class UserControls_MainDBToWebDB : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void UpdateInvoices(object sender, CommandEventArgs e)
    {
       
        //using (IDataProvider dp = dbUtils.DataProviderForBBPublic)
        //{
        //    try
        //    {
        //        dp.BeginTransaction();

        //        InvoicesXfr xfrInv = new InvoicesXfr(dp);
        //        xfrInv.Refresh();

        //        BullXfr xfrBull = new BullXfr(dp);
        //        xfrBull.Refresh();

        //        dp.CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        dp.AbortTransaction();
        //        throw new ApplicationException("Failed up update Invoice data on web server", ex);
        //    }
        //}
    }

    protected void UpdateCodes(object sender, CommandEventArgs e)
    {
        //StrainHerdYearXfr xfr = new StrainHerdYearXfr();
        //xfr.Refresh();
    }

}

