using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beefbooster.Web;

namespace UserControls
{
    public partial class BBBreederSelector : UserControl
    {
        private const string PleaseChoose = "Select Breeder";

        public string AccountDescription
        {
            get
            {
                ListItem li = ddlBreeder.SelectedItem;
                if (li.Text == PleaseChoose) return "";
                return ddlBreeder.SelectedItem.ToString();
            }
        }

        public string AccountNo
        {
            get
            {
                ListItem li = ddlBreeder.SelectedItem;
                if (li.Text == PleaseChoose) return "";
                return ddlBreeder.SelectedValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ddlBreeder_DataBound(object sender, EventArgs e)
        {
            if (BBWebUtility.UserCache[CacheStaticValues.LoggedOnBreederHerd] != null)
            {
                string strHerdSN = BBWebUtility.UserCache[CacheStaticValues.LoggedOnBreederHerd].ToString();
                ListItem listItem = ddlBreeder.Items.FindByValue(strHerdSN);
                if (listItem != null)
                {
                    int idxOfItem = ddlBreeder.Items.IndexOf(listItem);
                    ddlBreeder.SelectedIndex = idxOfItem;
                    ddlBreeder.Enabled = false;
                }
            }
            else
            {
                var li = new ListItem(PleaseChoose, null, true);
                ddlBreeder.Items.Insert(0, li);
                ddlBreeder.SelectedIndex = ddlBreeder.Items.IndexOf(li);
            }
        }
    }
}