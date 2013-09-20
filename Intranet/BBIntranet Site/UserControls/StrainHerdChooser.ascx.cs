using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class UserControls_StrainHerdChooser : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ddlStrain_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    public string StrainCode
    {
        get
        {
            return ddlStrain.SelectedItem.ToString();
        }
        set
        {
            ddlStrain.SelectedIndex = ddlStrain.Items.IndexOf(ddlStrain.Items.FindByText(value));

        }
    }

    public string HerdCode
    {
        get
        {
            return ddlHerd.SelectedItem.ToString();
        }
    }
    public int HerdSN
    {
        get
        {
            return Int32.Parse(ddlHerd.SelectedValue);
        }
        set
        {
            ddlHerd.SelectedIndex = ddlHerd.Items.IndexOf(ddlHerd.Items.FindByText(value.ToString()));
        }
    }
    public bool Enabled
    {
        get
        {
            return ddlStrain.Enabled;
        }
        set
        {
            ddlStrain.Enabled = value;
        }
    }

}
