using System;
using System.Web.UI.WebControls;


public partial class UserControls_BBYearSelector : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    ListItem listItem = ddlYear.Items.FindByValue(DefaultYearNumber);
        //    if (listItem != null)
        //    {
        //        int idxOfItem = ddlYear.Items.IndexOf(listItem);
        //        ddlYear.SelectedIndex = idxOfItem;
        //    }
        //}
    }
    private int _defaultYear = DateTime.Now.Year;
    public string DefaultYearNumber
    {
        get
        {
            return _defaultYear.ToString();
        }
        set
        {
            _defaultYear = int.Parse(value);
        }
    }
    public int YearNumber
    {
        get
        {
            return int.Parse(ddlYear.SelectedValue);
        }
    }
    protected void ddlYear_DataBound(object sender, EventArgs e)
    {
        ListItem listItem = ddlYear.Items.FindByValue(DefaultYearNumber);
        if (listItem != null)
        {
            int idxOfItem = ddlYear.Items.IndexOf(listItem);
            ddlYear.SelectedIndex = idxOfItem;
        }
    }
}
