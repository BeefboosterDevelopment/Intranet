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

public partial class UserControls_EditProfile : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadProfileIntoPage();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
/*
            // Create an empty Profile for the newly created user
            ProfileCommon p = (ProfileCommon)ProfileCommon.Create(Profile.UserName, true);

            p.FirstName = txtGivenName.Text;
            p.LastName = txtSurname.Text;
            p.CompanyName = txtCompanyName.Text;
            p.CompanyURL = txtCompanyUrl.Text;
            p.Address.Street = txtStreet.Text;
            p.Address.City = txtCity.Text;
            p.Address.ProvState = txtProvince.Text;
            p.Address.PostalCode = txtPostalCode.Text;
            p.Address.Country = "Canada";

            p.EmailAddress = txtEmail.Text;
            p.PhoneNumber = txtPhone.Text;
            p.FaxNumber = txtFAX.Text;

            // Save profile - must be done since we explicitly created it

            p.Save();
*/
            lblResults.Text = "Profile Changes Have Been Saved.";
        }
        catch (Exception ex)
        {
            lblResults.Text = "Error while saving profile..." + ex.Message;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        LoadProfileIntoPage();
    }

    private void LoadProfileIntoPage()
    {
/*
        // Create an empty Profile for the newly created user
        ProfileCommon p = (ProfileCommon)ProfileCommon.Create(Profile.UserName, true);

        txtGivenName.Text = p.FirstName;
        txtSurname.Text = p.LastName;
        txtCompanyName.Text = p.CompanyName;
        txtCompanyUrl.Text = p.CompanyURL;
        txtStreet.Text = p.Address.Street;
        txtCity.Text = p.Address.City;
        txtProvince.Text = p.Address.ProvState;
        txtPostalCode.Text = p.Address.PostalCode;

        txtEmail.Text = p.EmailAddress;
        txtPhone.Text = p.PhoneNumber;
        txtFAX.Text = p.FaxNumber;
 */
    }
}
