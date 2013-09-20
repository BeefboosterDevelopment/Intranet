using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Beefbooster.DataAccessLibrary;
using Beefbooster.DataAccessLibrary.Domain;
using Beefbooster.Web;

public class Rpt021_DataItem
{
    // Make sure you have a default constructor or this won't show up as a valid datasource
    public Rpt021_DataItem() { }

    #region Properties

    private string _calfId;
    public string CalfId
    {
        get { return _calfId; }
        set { _calfId = value; }
    }
    private string _birthDate;
    public string BirthDate
    {
        get { return _birthDate; }
        set { _birthDate = value; }
    }

    private int _birthWt;
    public int BirthWt
    {
        get { return _birthWt; }
        set { _birthWt = value; }
    }

    private int _weanWt;
    public int WeanWt
    {
        get { return _weanWt; }
        set { _weanWt = value; }
    }
    private double _adgBW;
    public double ADGBW
    {
        get { return _adgBW; }
        set { _adgBW = value; }
    }
    private double _vi;
    public double VI
    {
        get { return _vi; }
        set { _vi = value; }
    }
    private int _viRank;
    public int VIRank
    {
        get { return _viRank; }
        set { _viRank = value; }
    }

    private string _pulled;
    public string Pulled
    {
        get { return _pulled; }
        set { _pulled = value; }
    }

    private string _damId;
    public string DamId
    {
        get { return _damId; }
        set { _damId = value; }
    }

    private string _ageOfDam;
    public string AgeOfDam
    {
        get { return _ageOfDam; }
        set { _ageOfDam = value; }
    }
    private int _teatScore;
    public int TeatScore
    {
        get { return _teatScore; }
        set { _teatScore = value; }
    }
    private int _udderScore;
    public int UdderScore
    {
        get { return _udderScore; }
        set { _udderScore = value; }
    }
    #endregion
}


public class Rpt021_WeaningSummary
{
    // Make sure you have a default constructor or this won't show up as a valid datasource
    public Rpt021_WeaningSummary() { }
    //public Rpt021_WeaningSummary(string strain, int herdSN, int yearBorn, char sex, bool hasWeaningData, string reportMode)
    public Rpt021_WeaningSummary(string strain, int herdSN, int yearBorn)
    {
        _strain = strain;
        if ( herdSN > 0 )           
            _bbHerd = new BBDataHelper().GetHerd(herdSN);
        else
            _bbHerd = null;
        _yearBorn = yearBorn;

        //_sex = sex;

        //_hasWeaningData = hasWeaningData;

        //_reportMode = reportMode;
    }


    #region Properties

    private bool _hasWeaningData;
    public bool HasWeaningData
    {
        get
        {
            return _hasWeaningData;
        }
        set
        {
            _hasWeaningData = value;
        }
    }

    private  char _sex;
    public char Sex
    {
        get
        {
            return _sex;
        }
        set
        {
            _sex = value;
        }
    }
    private string _reportMode;
    public string ReportMode
    {
        get
        {
            return _reportMode;
        }
        set
        {
            _reportMode = value;
        }
    }
    private readonly string _strain;
    public string Strain
    {
        get
        {
            return _strain;
        }
    }
    private readonly BBHerd _bbHerd;
    public BBHerd BeefboosterHerd
    {
        get
        { 
            return _bbHerd; 
        }
    }

    private readonly int _yearBorn;
    public int YearBorn
    {
        get { return _yearBorn; }
    }

    #endregion

    /// <summary>
    /// Run the stored procedure and collect the data
    /// </summary>
    /// <returns></returns>
    public List<Rpt021_DataItem> GetData()
    {
        List<Rpt021_DataItem> lst = new List<Rpt021_DataItem>();
        SqlDataReader rdr = null;

        SqlParameter[] procParams = new SqlParameter[6];
        procParams[0] = ParameterHelper.GetVarCharPar("@strain", Strain, 2, ParameterDirection.Input);
        procParams[1] = ParameterHelper.GetIntegerPar("@birthYear", YearBorn, ParameterDirection.Input);

        if (BeefboosterHerd != null)
            procParams[2] = ParameterHelper.GetIntegerPar("@herdSN", BeefboosterHerd.SN, ParameterDirection.Input);
        else
            procParams[2] = ParameterHelper.GetIntegerPar("@herdSN", 0, ParameterDirection.Input);

        procParams[3] = ParameterHelper.GetCharPar("@sex", "B",2, ParameterDirection.Input);
        procParams[4] = ParameterHelper.GetBitPar("@hasWeaningData", false, ParameterDirection.Input);
        procParams[5] = ParameterHelper.GetVarCharPar("@ReportMode", "FULL", 4, ParameterDirection.Input);

        rdr = DataAccess.GetDataReaderStoredProc(WebConfigSettings.Configurations.CowCalf_ConnectionString,
                                                      "Rpt021_WeaningSummary", procParams, rdr);
        if (rdr.HasRows)
        {
            // get the ordinal indexes into the read buffer                
            int ordCalfId = rdr.GetOrdinal("ShortCalf_Id");
            int ordBirth_Date = rdr.GetOrdinal("Birth_Date");
            int ordBirth_Wt = rdr.GetOrdinal("Birth_Wt");
            int ordWean_Wt = rdr.GetOrdinal("Wean_Wt");
            int ordADGBW = rdr.GetOrdinal("ADG_BW");
            int ordVI = rdr.GetOrdinal("VI");
            int ordVIRank = rdr.GetOrdinal("VI_Rank");
            int ordDamId = rdr.GetOrdinal("Dam_Id");
            int ordUdderScore = rdr.GetOrdinal("Udder_Score");
            int ordTeatScore = rdr.GetOrdinal("Teat_Score");

            while (rdr.Read())
            {
                Rpt021_DataItem di = new Rpt021_DataItem();
                di.CalfId = rdr.GetString(ordCalfId);

                if (!rdr.IsDBNull(ordBirth_Wt))
                    di.BirthWt = rdr.GetInt32(ordBirth_Wt);
                else
                    di.BirthWt = 0;

                if (!rdr.IsDBNull(ordWean_Wt))
                {
                    di.WeanWt = rdr.GetInt32(ordWean_Wt);
                    di.ADGBW = Math.Round(rdr.GetDouble(ordADGBW), 2);
                }
                else
                {
                    di.WeanWt = 0;
                    di.ADGBW = 0;
                }

                if (!rdr.IsDBNull(ordVI))
                {
                    di.VI = Math.Round(rdr.GetDouble(ordVI), 2);
                    di.VIRank = rdr.GetInt32(ordVIRank);
                }
                else
                {
                    di.VI = 0;
                    di.VIRank = 0;
                }

                di.DamId = rdr.GetString(ordDamId);

                if (!rdr.IsDBNull(ordTeatScore))
                    di.TeatScore = rdr.GetByte(ordTeatScore);
                else
                    di.TeatScore = 0;

                if (!rdr.IsDBNull(ordUdderScore))
                    di.UdderScore = rdr.GetByte(ordUdderScore);
                else
                    di.UdderScore = 0;

                lst.Add(di);
            }
        }
        return lst;
    }

}

