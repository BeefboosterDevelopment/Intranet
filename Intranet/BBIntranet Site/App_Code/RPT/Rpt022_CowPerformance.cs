using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Beefbooster.DataAccessLibrary;
using Beefbooster.DataAccessLibrary.Domain;
using Beefbooster.Web;

public class Rpt022_DataItem
{
    // Make sure you have a default constructor or this won't show up as a valid datasource
    //public Rpt022_DataItem() { }

    #region Properties

    public string DamId { get; set; }

    public int AgeOfDam { get; set; }

    public int TeatScore { get; set; }

    public int UdderScore { get; set; }

    public int NumCalvesBorn { get; set; }

    public int NumCalvesWeaned { get; set; }

    public int NumTestBulls { get; set; }

    public int NumPassedBulls { get; set; }

    public int NumSoldBulls { get; set; }

    public int NumDaughters { get; set; }

    private double ?_ebvBirthWt;
    public double ?EBVBirthWt
    {
        get { return _ebvBirthWt; }
        set { if (value.HasValue) _ebvBirthWt = value; }
    }

    private double? _ebvBirthWtAcc;
    public double? EBVBirthWtAcc
    {
        get { return _ebvBirthWtAcc; }
        set { if (value.HasValue) _ebvBirthWtAcc = value; }
    }

    private double ?_ebvWeanWt;
    public double ?EBVWeanWt
    {
        get { return _ebvWeanWt; }
        set { if (value.HasValue) _ebvWeanWt = value; }
    }

    private double? _ebvWeanWtAcc;
    public double? EBVWeanWtAcc
    {
        get { return _ebvWeanWtAcc; }
        set { if (value.HasValue) _ebvWeanWtAcc = value; }
    }

    private double ?_ebvWeanWtMilk;
    public double ?EBVWeanWtMilk
    {
        get { return _ebvWeanWtMilk; }
        set { if (value.HasValue) _ebvWeanWtMilk = value; }
    }

    private double? _ebvWeanWtMilkAcc;
    public double? EBVWeanWtMilkAcc
    {
        get { return _ebvWeanWtMilkAcc; }
        set { if (value.HasValue) _ebvWeanWtMilkAcc = value; }
    }

    private double ?_ebvYearlingWt;
    public double ?EBVYearlingWt
    {
        get { return _ebvYearlingWt; }
        set { if (value.HasValue) _ebvYearlingWt = value; }
    }

    private double? _ebvYearlingWtAcc;
    public double? EBVYearlingWtAcc
    {
        get { return _ebvYearlingWtAcc; }
        set { if (value.HasValue) _ebvYearlingWtAcc = value; }
    }

    private double ?_ebvMatureWt;
    public double ?EBVMatureWt
    {
        get { return _ebvMatureWt; }
        set { if (value.HasValue) _ebvMatureWt = value; }
    }

    private double? _ebvMatureWtAcc;
    public double? EBVMatureWtAcc
    {
        get { return _ebvMatureWtAcc; }
        set { if (value.HasValue) _ebvMatureWtAcc = value; }
    }

    private double ?_ebvScrotal;
    public double ?EBVScrotal
    {
        get { return _ebvScrotal; }
        set { if (value.HasValue) _ebvScrotal = value; }
    }

    private double? _ebvScrotalAcc;
    public double? EBVScrotalAcc
    {
        get { return _ebvScrotalAcc; }
        set { if (value.HasValue) _ebvScrotalAcc = value; }
    }

    private double ?_ebvBackFat;
    public double ?EBVBackFat
    {
        get { return _ebvBackFat; }
        set { if (value.HasValue) _ebvBackFat = value; }
    }

    private double? _ebvBackFatAcc;
    public double? EBVBackFatAcc
    {
        get { return _ebvBackFatAcc; }
        set { if (value.HasValue) _ebvBackFatAcc = value; }
    }

    private double? _vi;
    public double? VI
    {
        get { return _vi; }
        set { if (value.HasValue) _vi = value; }
    }

    private double? _viAcc;
    public double? VIAcc
    {
        get { return _viAcc; }
        set { if (value.HasValue) _viAcc = value; }
    }

    #endregion
}

public class Rpt022_CowPerformance
{
    // Make sure you have a default constructor or this won't show up as a valid datasource
    //public Rpt022_CowPerformance() { }
    public Rpt022_CowPerformance(int herdSN, int yearBorn, int sortOrder)
    {
        _yearBorn = yearBorn;
        _sortOrder = sortOrder;
        _bbHerd = new BBDataHelper().GetHerd(herdSN);
    }


    #region Properties

    private readonly BBHerd _bbHerd;
    public BBHerd BeefboosterHerd
    {
        get { return _bbHerd; }
    }

    private readonly int _sortOrder;
    public int SortOrder
    {
        get { return _sortOrder; }
    }

    private readonly int _yearBorn;
    public int YearBorn
    {
        get { return _yearBorn; }
    }
    #endregion


    public IEnumerable<Rpt022_DataItem> GetData()
    {
        List<Rpt022_DataItem> lst = new List<Rpt022_DataItem>();
        SqlDataReader rdr=null;

        SqlParameter[] procParams = new SqlParameter[3];
        procParams[0] = ParameterHelper.GetIntegerPar("@herdSN", BeefboosterHerd.SN, ParameterDirection.Input);
        procParams[1] = ParameterHelper.GetIntegerPar("@birthYear", YearBorn, ParameterDirection.Input);
        procParams[2] = ParameterHelper.GetIntegerPar("@sortOrder", SortOrder, ParameterDirection.Input);

        rdr = DataAccess.GetDataReaderStoredProc(WebConfigSettings.Configurations.CowCalf_ConnectionString,
                                                      "Rpt022_Weaning_CowPerformance", procParams, rdr);

        if (rdr.HasRows)
        {
            // get the ordinal indexes into the read buffer                
            int ordDamId = rdr.GetOrdinal("Dam_Id");
            int ordDamAge = rdr.GetOrdinal("DamAge");
            int ordTeatScore = rdr.GetOrdinal("Teat_Score");
            int ordUdderScore = rdr.GetOrdinal("Udder_Score");

            int ordNumCalvesBorn = rdr.GetOrdinal("calvesBorn");
            int ordWeanedCalves = rdr.GetOrdinal("weanedCalves");
            int ordSelectedBullCalves = rdr.GetOrdinal("selectedBullCalves");
            int ordPassedBullCalves = rdr.GetOrdinal("passedBullCalves");
            int ordSoldBullCalves = rdr.GetOrdinal("soldBullCalves");
            int ordDaughters = rdr.GetOrdinal("daughters");

            int ordBwebv = rdr.GetOrdinal("BWTEBV");
            int ordBwebvAcc = rdr.GetOrdinal("BWTEBV_Acc");
            int ordWwdEBV = rdr.GetOrdinal("WWTEBV");
            int ordWwdEBVAcc = rdr.GetOrdinal("WWTEBV_Acc");
            int ordWwmEBV = rdr.GetOrdinal("MilkEBV");
            int ordWwmEBVAcc = rdr.GetOrdinal("MilkEBV_Acc");
            int ordYwtEBV = rdr.GetOrdinal("YWTEBV");
            int ordYwtEBVAcc = rdr.GetOrdinal("YWTEBV_ACC");
            int ordScEBV = rdr.GetOrdinal("SCEBV");
            int ordScEBVAcc = rdr.GetOrdinal("SCEBV_ACC");
            int ordBfEBV = rdr.GetOrdinal("BFEBV");
            int ordBfEBVAcc = rdr.GetOrdinal("BFEBV_ACC");
            int ordMwEBV = rdr.GetOrdinal("MWTEBV");
            int ordMwEBVAcc = rdr.GetOrdinal("MWTEBV_ACC");
            int ordVI = rdr.GetOrdinal("SelIdx");
            int ordVIAcc = rdr.GetOrdinal("SelIdx_ACC");
            while (rdr.Read())
            {
                Rpt022_DataItem di = new Rpt022_DataItem
                                         {
                                             DamId = rdr.GetString(ordDamId),
                                             AgeOfDam = rdr.GetInt32(ordDamAge),
                                             TeatScore = DataAccess.SafeGetTinyInt(rdr, ordTeatScore, 0),
                                             UdderScore = DataAccess.SafeGetTinyInt(rdr, ordUdderScore, 0),
                                             NumCalvesBorn = DataAccess.SafeGetInt32(rdr, ordNumCalvesBorn, 0),
                                             NumCalvesWeaned = DataAccess.SafeGetInt32(rdr, ordWeanedCalves, 0),
                                             NumTestBulls = DataAccess.SafeGetInt32(rdr, ordSelectedBullCalves, 0),
                                             NumPassedBulls = DataAccess.SafeGetInt32(rdr, ordPassedBullCalves, 0),
                                             NumSoldBulls = DataAccess.SafeGetInt32(rdr, ordSoldBullCalves, 0),
                                             NumDaughters = DataAccess.SafeGetInt32(rdr, ordDaughters, 0)

                                         };

                if (!rdr.IsDBNull(ordBwebv))
                    di.EBVBirthWt = rdr.GetDouble(ordBwebv);
                if (!rdr.IsDBNull(ordBwebvAcc))
                    di.EBVBirthWtAcc = rdr.GetDouble(ordBwebvAcc);


                if (!rdr.IsDBNull(ordWwdEBV))
                    di.EBVWeanWt = rdr.GetDouble(ordWwdEBV);
                if (!rdr.IsDBNull(ordWwdEBVAcc))
                    di.EBVWeanWtAcc = rdr.GetDouble(ordWwdEBVAcc);


                if (!rdr.IsDBNull(ordWwmEBV))
                    di.EBVWeanWtMilk = rdr.GetDouble(ordWwmEBV);
                if (!rdr.IsDBNull(ordWwmEBVAcc))
                    di.EBVWeanWtMilkAcc = rdr.GetDouble(ordWwmEBVAcc);


                if (!rdr.IsDBNull(ordYwtEBV))
                    di.EBVYearlingWt = rdr.GetDouble(ordYwtEBV);
                if (!rdr.IsDBNull(ordYwtEBVAcc))
                    di.EBVYearlingWtAcc = rdr.GetDouble(ordYwtEBVAcc);


                if (!rdr.IsDBNull(ordBfEBV))
                    di.EBVBackFat = rdr.GetDouble(ordBfEBV);
                if (!rdr.IsDBNull(ordBfEBVAcc))
                    di.EBVBackFatAcc = rdr.GetDouble(ordBfEBVAcc);


                if (!rdr.IsDBNull(ordScEBV))
                    di.EBVScrotal = rdr.GetDouble(ordScEBV);
                if (!rdr.IsDBNull(ordScEBVAcc))
                    di.EBVScrotalAcc = rdr.GetDouble(ordScEBVAcc);


                if (!rdr.IsDBNull(ordMwEBV))
                    di.EBVMatureWt = rdr.GetDouble(ordMwEBV);
                if (!rdr.IsDBNull(ordMwEBVAcc))
                    di.EBVMatureWtAcc = rdr.GetDouble(ordMwEBVAcc);


                if (!rdr.IsDBNull(ordVI))
                    di.VI = rdr.GetDouble(ordVI);
                if (!rdr.IsDBNull(ordVIAcc))
                    di.VIAcc = rdr.GetDouble(ordVIAcc);

                lst.Add(di);
            }
        }
        return lst;
    }

}