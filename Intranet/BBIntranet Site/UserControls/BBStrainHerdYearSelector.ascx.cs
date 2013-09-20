using System;
using System.Web.UI.WebControls;


public partial class UserControls_StrainHerdYearSelector : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string HerdDescription
    {
        get
        {
            if (ddlHerd.SelectedValue == "ALL")
                return "";
            return ddlHerd.SelectedItem.ToString();
        }
    }

    public int HerdSN
    {
        get
        {
            if (ddlHerd.SelectedValue == "ALL")
                return 0;
            return int.Parse(ddlHerd.SelectedValue);
        }
    }

    public string StrainCode
    {
        get
        {
            return ddlStrain.SelectedItem.ToString();
        }
    }

    public int YearNumber
    {
        get
        {
            return int.Parse(ddlYear.SelectedValue);
        }
        set
        {
            ListItem listItem = ddlYear.Items.FindByValue(value.ToString());
            if (listItem != null)
            {
                int idxOfItem = ddlYear.Items.IndexOf(listItem);
                ddlYear.SelectedIndex = idxOfItem;
            }
        }
    }
    private int _defaultYear = DateTime.MinValue.Year;
    public string DefaultYearNumber
    {
        get
        {
            if (_defaultYear == DateTime.MinValue.Year)
            {
                if (DateTime.Now.Month <= 6)
                    return (DateTime.Now.Year - 1).ToString();
                else
                    return DateTime.Now.Year.ToString();
            }            
            return _defaultYear.ToString();
        }
        set
        {
            _defaultYear = int.Parse(value);
        }
    }
    protected void ddlHerd_DataBound(object sender, EventArgs e)
    {
        ListItem li = new ListItem("ALL", null, true);
        ddlHerd.Items.Insert(0, li);
        ddlHerd.SelectedIndex = ddlHerd.Items.IndexOf(li);
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
