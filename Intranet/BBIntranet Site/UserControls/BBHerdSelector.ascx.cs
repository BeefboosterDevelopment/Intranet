using System;
using System.Web.UI.WebControls;
using Beefbooster.Web;

public partial class BBHerdSelector : System.Web.UI.UserControl
{
    private const string PLEASE_CHOOSE = "Select Herd";

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string HerdDescription
    {
        get
        {
            ListItem li = ddlHerd.SelectedItem;
            if (li.Text == PLEASE_CHOOSE) return "";
            return ddlHerd.SelectedItem.ToString();
        }
    }

    public int HerdSN
    {
        get
        {
            ListItem li = ddlHerd.SelectedItem;
            if (li.Text==PLEASE_CHOOSE) return -1;
            return int.Parse(ddlHerd.SelectedValue);
        }
    }
    protected void ddlHerd_DataBound(object sender, EventArgs e)
    {

        if (BBWebUtility.UserCache[CacheStaticValues.LoggedOnBreederHerd] != null)
        {
            string strHerdSN = BBWebUtility.UserCache[CacheStaticValues.LoggedOnBreederHerd].ToString();
            ListItem listItem = ddlHerd.Items.FindByValue(strHerdSN);
            if (listItem != null)
            {
                int idxOfItem = ddlHerd.Items.IndexOf(listItem);
                ddlHerd.SelectedIndex = idxOfItem;
                ddlHerd.Enabled = false;
            }
        }
        else
        {
            ListItem li = new ListItem(PLEASE_CHOOSE, null, true);
            ddlHerd.Items.Insert(0, li);
            ddlHerd.SelectedIndex = ddlHerd.Items.IndexOf(li);
        }
    }
}
