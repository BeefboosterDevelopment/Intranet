/// <summary>
/// Summary description for Breeder
/// </summary>
public class BreederReportData
{
/*    public BreederReportData(string herdCode)
    {
    }*/

    private string _accountNo;
    public string AccountNo
    {
        get { return _accountNo; }
        set { _accountNo = value; }
    }

    private int _herd_SN;
    public int Herd_SN
    {
        get { return _herd_SN; }
        set { _herd_SN = value; }
    }

    private string _herd_Code;
    public string Herd_Code
    {
        get { return _herd_Code; }
        set { _herd_Code = value; }
    }

    private string _strain_Code;
    public string Strain_Code
    {
        get { return _strain_Code; }
        set { _strain_Code = value; }
    }

    private string _ranch_Name;
    public string Ranch_Name
    {
        get { return _ranch_Name; }
        set { _ranch_Name = value; }
    }
    private string _breeder_Name;
    public string Breeder_Name
    {
        get { return _breeder_Name; }
        set { _breeder_Name = value; }
    }
}
