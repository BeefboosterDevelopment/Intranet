using System;

public partial class BBHerdYearSelector : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string HerdDescription
    {
        get
        {
            return ucBBHerdSelector.HerdDescription;
        }
    }
    public int HerdSN
    {
        get
        {
            return ucBBHerdSelector.HerdSN;
        }
    }
    public int YearNumber
    {
        get
        {
            return ucBBYearSelector.YearNumber;
        }
    }
}
