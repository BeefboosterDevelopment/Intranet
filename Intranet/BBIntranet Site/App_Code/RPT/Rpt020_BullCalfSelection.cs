using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Beefbooster.DataAccessLibrary;
using Beefbooster.DataAccessLibrary.Domain;
using Beefbooster.Web;

public class Rpt020_DataItem
{
    #region Properties
    private decimal? _birthWtEbv;
    private decimal? _birthWtEbvAcc;
    private decimal? _weanWtGrowthEbv;
    private decimal? _weanWtMilkEbv;
    private decimal? _weanWtGrowthEbvAcc;
    private decimal? _weanWtMilkEbvAcc;

    private decimal? _ywtEbv;
    private decimal? _ywtEbvAcc;

    private decimal? _matwtEbv;
    private decimal? _matwtEbvAcc;

    private decimal? _scEbv;
    private decimal? _scEbvAcc;

    private decimal? _bfEbv;
    private decimal? _bfEbvAcc;

    private decimal? _rfiEbv;
    private decimal? _rfiEbvAcc;
    
    public string CalfId { get; set; }
    public string BirthDate { get; set; }

    private int? _BW_ADJ;
    private int? _WW_ADJ;
    private decimal? _ADG_BW_ADJ;
    private decimal? _SEL_IDX;
    private decimal? _SEL_ACC;
    private int? _SEL_IDX_Rank;

    public int? BW_ADJ
    {
        get { return _BW_ADJ; }
        set { if (value.HasValue) _BW_ADJ = value; }
    }

    public decimal? BirthWtEbv
    {
        get { return _birthWtEbv; }
        set { if (value.HasValue) _birthWtEbv = value; }
    }

    public decimal? BirthWtEbvAcc
    {
        get { return _birthWtEbvAcc; }
        set { if (value.HasValue) _birthWtEbvAcc = value; }
    }

    public int? WW_ADJ
    {
        get { return _WW_ADJ; }
        set { if (value.HasValue) _WW_ADJ = value; }
    }

    public decimal? WeanWtGrowthEbv
    {
        get { return _weanWtGrowthEbv; }
        set { if (value.HasValue) _weanWtGrowthEbv = value; }
    }

    public decimal? WeanWtGrowthEbvAcc
    {
        get { return _weanWtGrowthEbvAcc; }
        set { if (value.HasValue) _weanWtGrowthEbvAcc = value; }
    }

    public decimal? WeanWtMilkEbv
    {
        get { return _weanWtMilkEbv; }
        set { if (value.HasValue) _weanWtMilkEbv = value; }
    }

    public decimal? WeanWtMilkEbvAcc
    {
        get { return _weanWtMilkEbvAcc; }
        set { if (value.HasValue) _weanWtMilkEbvAcc = value; }
    }

    public decimal? YwtEbv
    {
        get { return _ywtEbv; }
        set { if (value.HasValue) _ywtEbv = value; }
    }

    public decimal? YwtEbvAcc
    {
        get { return _ywtEbvAcc; }
        set { if (value.HasValue) _ywtEbvAcc = value; }
    }


    public decimal? MatwtEbv
    {
        get { return _matwtEbv; }
        set { if (value.HasValue) _matwtEbv = value; }
    }
    public decimal? MatwtEbvAcc
    {
        get { return _matwtEbvAcc; }
        set { if (value.HasValue) _matwtEbvAcc = value; }
    }


    public decimal? ScEbv
    {
        get { return _scEbv; }
        set { if (value.HasValue) _scEbv = value; }
    }
    public decimal? ScEbvAcc
    {
        get { return _scEbvAcc; }
        set { if (value.HasValue) _scEbvAcc = value; }
    }

    public decimal? BfEbv
    {
        get { return _bfEbv; }
        set { if (value.HasValue) _bfEbv = value; }
    }
    public decimal? BfEbvAcc
    {
        get { return _bfEbvAcc; }
        set { if (value.HasValue) _bfEbvAcc = value; }
    }

    public decimal? RfiEbv
    {
        get { return _rfiEbv; }
        set { if (value.HasValue) _rfiEbv = value; }
    }
    public decimal? RfiEbvAcc
    {
        get { return _rfiEbvAcc; }
        set { if (value.HasValue) _rfiEbvAcc = value; }
    }

    public decimal? ADG_BW_ADJ
    {
        get { return _ADG_BW_ADJ; }
        set { if (value.HasValue) _ADG_BW_ADJ = value; }
    }

    public decimal? SEL_IDX
    {
        get { return _SEL_IDX; }
        set { if (value.HasValue) _SEL_IDX = value; }
    }

    public decimal? SEL_ACC
    {
        get { return _SEL_ACC; }
        set { if (value.HasValue) _SEL_ACC = value; }
    }

    public int? SEL_IDX_Rank
    {
        get { return _SEL_IDX_Rank; }
        set { if (value.HasValue) _SEL_IDX_Rank = value; }
    }

    public string SireId { get; set; }
    public string DamId { get; set; }
    public int AgeOfDam { get; set; }
    public int TeatScore { get; set; }
    public int UdderScore { get; set; }
    public int NumberWeaned { get; set; }

    #endregion

    // Make sure you have a default constructor or this won't show up as a valid datasource
}


public class Rpt020_BullCalfSelection
{
    public Rpt020_BullCalfSelection(int herdSN, int yearBorn)
    {
        BeefboosterHerd = new BBDataHelper().GetHerd(herdSN);
        YearBorn = yearBorn;
        SetDefaults();
    }

    #region Properties

    private decimal? _minADG;
    public BBHerd BeefboosterHerd { get; private set; }

    public int YearBorn { get; private set; }

    public int MaxBirthMonth { get; set; }

    public int MaxBirthDay { get; set; }

    public DateTime MaxBirthDate { get; private set; }

    public bool IncludePulledCalves { get; set; }

    public bool IncludeCalvesFromHeifers { get; set; }

    public int MaxBWT { get; set; }

    public int MinBWT { get; set; }

    public decimal? MinADG
    {
        get { return _minADG; }
        set { if (value.HasValue) _minADG = value; }
    }

    public int TopN { get; set; }

    public string ReportMode { get; set; }

    public int NumberQualifyingCalves { get; private set; }

    #endregion

    private void SetDefaults()
    {
        MinBWT = 80;
        MaxBWT = 110;
        MaxBirthMonth = 5;
        MaxBirthDay = 15;
        MaxBirthDate = DateTime.Parse("15-MAY-" + YearBorn);
        IncludePulledCalves = false;
        IncludeCalvesFromHeifers = false;
        MinADG = (Decimal) 2.0;
        ReportMode = "SI";
        TopN = 20;

        var procParams = new SqlParameter[2];
        procParams[0] = ParameterHelper.GetCharPar("@strain", BeefboosterHerd.Strain, 2, ParameterDirection.Input);
        procParams[1] = ParameterHelper.GetCharPar("@seltype", "HEIFER", 20, ParameterDirection.Input);

        SqlDataReader rdr = DataAccess.GetDataReaderStoredProc(
            WebConfigSettings.Configurations.CowCalf_ConnectionString,
            "RptSelection_GetDefaults", procParams, null);

        if (rdr.HasRows)
        {
            // get the ordinal indexes into the read buffer
            int ordMinBWT = rdr.GetOrdinal("Min_BWT");
            int ordMaxBWT = rdr.GetOrdinal("Max_BWT");
            int ordMinADG = rdr.GetOrdinal("Min_ADG");
            int ordMaxBirthMonth = rdr.GetOrdinal("Max_BirthDateMonth");
            int ordMaxBirthDay = rdr.GetOrdinal("Max_BirthDateDay");
            int ordInclPulled = rdr.GetOrdinal("AssistedBirth_Flag");
            int ordInclHeifer = rdr.GetOrdinal("FromHeifer_Flag");
            if (rdr.Read())
            {
                MinBWT = rdr.GetInt16(ordMinBWT);
                MaxBWT = rdr.GetInt16(ordMaxBWT);
                _minADG = rdr.GetDecimal(ordMinADG);
                MaxBirthMonth = rdr.GetInt16(ordMaxBirthMonth);
                MaxBirthDay = rdr.GetInt16(ordMaxBirthDay);
                IncludePulledCalves = rdr.GetBoolean(ordInclPulled);
                IncludeCalvesFromHeifers = rdr.GetBoolean(ordInclHeifer);
            }
            string strMinBirthDate = MaxBirthDay + "-" + GetMonth(MaxBirthMonth) + "-" + YearBorn;
            MaxBirthDate = DateTime.Parse(strMinBirthDate);
        }
    }

    private static string GetMonth(int m)
    {
        switch (m)
        {
            case 1:
                return "Jan";
            case 2:
                return "Feb";
            case 3:
                return "Mar";
            case 4:
                return "Apr";
            case 5:
                return "May";
            case 6:
                return "Jun";
            case 7:
                return "Jul";
            case 8:
                return "Aug";
            case 9:
                return "Sep";
            case 10:
                return "Oct";
            case 11:
                return "Nov";
            case 12:
                return "Dec";
        }
        return "???";
    }

    /// <summary>
    /// Run the stored procedure and collect the data
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Rpt020_DataItem> GetData()
    {
        var lst = new List<Rpt020_DataItem>();

        var procParams = new SqlParameter[11];
        procParams[0] = ParameterHelper.GetIntegerPar("@herdSN", BeefboosterHerd.SN, ParameterDirection.Input);
        procParams[1] = ParameterHelper.GetIntegerPar("@birthYear", YearBorn, ParameterDirection.Input);
        procParams[2] = ParameterHelper.GetIntegerPar("@minBWT", MinBWT, ParameterDirection.Input);
        procParams[3] = ParameterHelper.GetIntegerPar("@maxBWT", MaxBWT, ParameterDirection.Input);
        procParams[4] = ParameterHelper.GetDateTimePar("@maxBDate", MaxBirthDate, ParameterDirection.Input);
        if (MinADG != null)
            procParams[5] = ParameterHelper.GetDecimalPar("@minADG", MinADG.Value, ParameterDirection.Input);
        procParams[6] = ParameterHelper.GetIntegerPar("@nTop", TopN, ParameterDirection.Input);
        procParams[7] = ParameterHelper.GetBitPar("@discardHeiferCalves", !IncludeCalvesFromHeifers,
                                                  ParameterDirection.Input);
        procParams[8] = ParameterHelper.GetBitPar("@discardPulledCalves", !IncludePulledCalves, ParameterDirection.Input);
        procParams[9] = ParameterHelper.GetVarCharPar("@ReportMode", ReportMode, 5, ParameterDirection.Input);
        procParams[10] = new SqlParameter
                             {
                                 ParameterName = "@NumberQualifyingCalves",
                                 Direction = ParameterDirection.Output,
                                 SqlDbType = SqlDbType.Int
                             };

        var objCommand = new SqlCommand
                             {CommandText = "Rpt020_BullCalfSelection", CommandType = CommandType.StoredProcedure};

        using (var objConnection = new SqlConnection(WebConfigSettings.Configurations.CowCalf_ConnectionString))
        {
            objCommand.Connection = objConnection;
            foreach (SqlParameter objPar in procParams)
                objCommand.Parameters.Add(objPar);
            objConnection.Open();

            SqlDataReader rdr = objCommand.ExecuteReader();
            if (rdr.HasRows)
            {
                // get the ordinal indexes into the read buffer                
                int ordCalfId = rdr.GetOrdinal("ShortCalf_Id");
                int ordBirthDate = rdr.GetOrdinal("Birth_Date");

                int ordBWTEbv = rdr.GetOrdinal("BWT_EBV");
                int ordBWTEbvAcc = rdr.GetOrdinal("BWT_EBV_REL");

                int ordWeanWtGrowthEbv = rdr.GetOrdinal("WW_GROWTH_EBV");
                int ordWeanWtMilkEbv = rdr.GetOrdinal("WW_MILK_EBV");

                int ordWeanWtGrowthEbvAcc = rdr.GetOrdinal("WW_GROWTH_EBV_REL");
                int ordWeanWtMilkEbvAcc = rdr.GetOrdinal("WW_MILK_EBV_REL");

                int ordYwtEbv = rdr.GetOrdinal("YWT_EBV");
                int ordYwtEbvAcc = rdr.GetOrdinal("YWT_EBV_REL");

                int ordMatwtEbv = rdr.GetOrdinal("MW_EBV");
                int ordMatwtEbvAcc = rdr.GetOrdinal("MW_EBV_REL");

                int ordScEbv = rdr.GetOrdinal("SC_EBV");
                int ordScEbvAcc = rdr.GetOrdinal("SC_EBV_REL");

                int ordBfEbv = rdr.GetOrdinal("BF_EBV");
                int ordBfEbvAcc = rdr.GetOrdinal("BF_EBV_REL");

                int ordRfiEbv = rdr.GetOrdinal("RFI_EBV");
                int ordRfiEbvAcc = rdr.GetOrdinal("RFI_EBV_REL");

                int ordBW_ADJ = rdr.GetOrdinal("BW_ADJ");
                int ordWW_ADJ = rdr.GetOrdinal("WW_ADJ");
                int ordADG_BW_ADJ = rdr.GetOrdinal("ADG_BW_ADJ");

                int ordSEL_IDX = rdr.GetOrdinal("SEL_IDX");
                int ordSEL_ACC = rdr.GetOrdinal("SEL_EBV_REL");
                int ordSEL_IDX_Rank = rdr.GetOrdinal("SEL_IDX_Rank");

                int ordSireCalfId = rdr.GetOrdinal("Sire_Calf_Id");
                int ordDamId = rdr.GetOrdinal("Dam_Id");
                int ordDamAge = rdr.GetOrdinal("DamAge");
                int ordUdderScore = rdr.GetOrdinal("Udder_Score");
                int ordTeatScore = rdr.GetOrdinal("Teat_Score");
                int ordNumCalvesWeaned = rdr.GetOrdinal("NumCalvesWeaned");

                while (rdr.Read())
                {
                    var di = new Rpt020_DataItem {CalfId = rdr.GetString(ordCalfId)};
                    DateTime bdate = rdr.GetDateTime(ordBirthDate);
                    di.BirthDate = bdate.Day + "-" + GetMonth(bdate.Month);

                    if (!rdr.IsDBNull(ordBW_ADJ))
                        di.BW_ADJ = rdr.GetInt32(ordBW_ADJ);

                    if (!rdr.IsDBNull(ordBWTEbv))
                        di.BirthWtEbv = rdr.GetDecimal(ordBWTEbv);

                    if (!rdr.IsDBNull(ordBWTEbvAcc))
                        di.BirthWtEbvAcc = GetAccuracyValue(rdr, ordBWTEbvAcc);

                    if (!rdr.IsDBNull(ordWW_ADJ))
                        di.WW_ADJ = rdr.GetInt32(ordWW_ADJ);

                    if (!rdr.IsDBNull(ordWeanWtGrowthEbv))
                        di.WeanWtGrowthEbv = rdr.GetDecimal(ordWeanWtGrowthEbv);

                    if (!rdr.IsDBNull(ordWeanWtMilkEbv))
                        di.WeanWtMilkEbv = rdr.GetDecimal(ordWeanWtMilkEbv);

                    if (!rdr.IsDBNull(ordWeanWtMilkEbvAcc))
                        di.WeanWtMilkEbvAcc = GetAccuracyValue(rdr, ordWeanWtMilkEbvAcc);

                    if (!rdr.IsDBNull(ordWeanWtGrowthEbvAcc))
                        di.WeanWtGrowthEbvAcc = GetAccuracyValue(rdr, ordWeanWtGrowthEbvAcc); 

                    if (!rdr.IsDBNull(ordYwtEbv))
                        di.YwtEbv = rdr.GetDecimal(ordYwtEbv);
                    if (!rdr.IsDBNull(ordYwtEbvAcc))
                        di.YwtEbvAcc = GetAccuracyValue(rdr, ordYwtEbvAcc); 

                    if (!rdr.IsDBNull(ordScEbv))
                        di.ScEbv = rdr.GetDecimal(ordScEbv);
                    if (!rdr.IsDBNull(ordScEbvAcc))
                        di.ScEbvAcc = GetAccuracyValue(rdr, ordScEbvAcc); 

                    if (!rdr.IsDBNull(ordMatwtEbv))
                        di.MatwtEbv = rdr.GetDecimal(ordMatwtEbv);
                    if (!rdr.IsDBNull(ordMatwtEbvAcc))
                        di.MatwtEbvAcc = GetAccuracyValue(rdr, ordMatwtEbvAcc);

                    if (!rdr.IsDBNull(ordBfEbv))
                        di.BfEbv = rdr.GetDecimal(ordBfEbv);
                    if (!rdr.IsDBNull(ordBfEbvAcc))
                        di.BfEbvAcc = GetAccuracyValue(rdr, ordBfEbvAcc);

                    if (!rdr.IsDBNull(ordRfiEbv))
                        di.RfiEbv = rdr.GetDecimal(ordRfiEbv);
                    if (!rdr.IsDBNull(ordRfiEbvAcc))
                        di.RfiEbvAcc = GetAccuracyValue(rdr, ordRfiEbvAcc);

                    if (!rdr.IsDBNull(ordADG_BW_ADJ))
                        di.ADG_BW_ADJ = rdr.GetDecimal(ordADG_BW_ADJ);

                    if (!rdr.IsDBNull(ordSEL_IDX))
                    {
                        di.SEL_IDX = decimal.Round(rdr.GetDecimal(ordSEL_IDX), 0); 
                        di.SEL_IDX_Rank = rdr.GetInt32(ordSEL_IDX_Rank);
                        if (!rdr.IsDBNull(ordSEL_ACC))
                            di.SEL_ACC = GetAccuracyValue(rdr, ordSEL_ACC); 
                    }


                    if (!rdr.IsDBNull(ordSireCalfId))
                        di.SireId = rdr.GetString(ordSireCalfId);

                    di.DamId = rdr.GetString(ordDamId);
                    di.AgeOfDam = rdr.GetInt32(ordDamAge);
                    di.TeatScore = !rdr.IsDBNull(ordTeatScore) ? rdr.GetByte(ordTeatScore) : 0;
                    di.UdderScore = !rdr.IsDBNull(ordUdderScore) ? rdr.GetByte(ordUdderScore) : 0;
                    di.NumberWeaned = !rdr.IsDBNull(ordNumCalvesWeaned) ? rdr.GetInt32(ordNumCalvesWeaned) : 0;
                    lst.Add(di);
                }
            }
        }

        // Retrieve the output parameter - AFTER the Read() method
        NumberQualifyingCalves = (Int32) objCommand.Parameters["@NumberQualifyingCalves"].Value;

        return lst;
    }

    private decimal GetAccuracyValue(SqlDataReader rdr, int ord)
    {
        return decimal.Round(rdr.GetDecimal(ord) * 100M, 0, MidpointRounding.AwayFromZero);
    }

}