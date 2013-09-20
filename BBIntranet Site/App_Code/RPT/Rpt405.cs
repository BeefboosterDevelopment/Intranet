using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Beefbooster.BusinessLogic;
using Beefbooster.DataAccessLibrary;
using Beefbooster.Web;


public class Rpt405_DataItem
{
    // Make sure you have a default constructor 
    public Rpt405_DataItem() { }
    public Rpt405_DataItem(int calfSN) 
    {
        Calf_SN = calfSN;
    }


    public int CCID { get; set; }
    public int Calf_SN { get; set; }
    public string Strain_Code { get; set; }
    public string Herd_Code { get; set; }
    public string Yr_Code { get; set; }
    public int Tag_Num { get; set; }
    public string Calf_Id { get; set; }
    public string HideColour_Code { get; set; }
    public DateTime Birth_Date { get; set; }

    public string BreederName { get; set; }
    public string BreederCompany { get; set; }

    public string SelectionGroup { get; set; }
    public decimal PriceCDN { get; set; }
    public decimal PriceUSD { get; set; }
    public short AgeOfDam { get; set; }
    public string Feedlot_ID { get; set; }
    public short BW_ADJ { get; set; }
    public short OffTest_Wt { get; set; }
    public decimal ADG_BW_ADJ { get; set; }
    public short YW_ADJ { get; set; }
    public short BACKFAT_ADJ { get; set; }
    public decimal Stn_ADG { get; set; }
    public decimal SCROTCIRC_ADJ { get; set; }

    public decimal BW_EBV { get; set; }
    public decimal WWD_EBV { get; set; }
    public decimal YWT_EBV { get; set; }
    public decimal SC_EBV { get; set; }
    public decimal BF_EBV { get; set; }
    public decimal WWM_EBV { get; set; }
    public decimal MW_EBV { get; set; }
    public decimal H18M_EBV { get; set; }
    public decimal RFI_EBV { get; set; }


    public string DOB 
    { 
        get 
        {
           return Birth_Date.ToString("dd-MMM-yyyy");
        } 
    }
}

public class Rpt405_DataObject
{
    private const string REPORT_NAME = "Sire Certificates";

    public Rpt405_DataObject() { }

    public Rpt405_DataObject(int bornInYear, string strainCode, string reportStyle)
    {
        _yearBorn = bornInYear;
        _strainCode = strainCode;
        _reportStyle = reportStyle;
    }
    #region Properties

    private readonly string _strainCode;
    public string StrainCode
    {
        get { return _strainCode; }
    }

    private readonly int _yearBorn;
    public int YearBorn
    {
        get { return _yearBorn; }
    }

    private readonly string _reportStyle = string.Empty;
    public string ReportStyle
    {
        get { return _reportStyle; }
    }

    public string ReportYear
    {
        get
        {
            int toYear = _yearBorn+1;
            return toYear.ToString();
        }
    }

    #endregion



    public List<Rpt405_DataItem> GetData()
    {
        List<Rpt405_DataItem> lst = new List<Rpt405_DataItem>();
        SqlDataReader rdr = null;

        SqlParameter[] procParams = new SqlParameter[2];
        procParams[0] = ParameterHelper.GetIntegerPar("@birthYr", YearBorn, ParameterDirection.Input);
        procParams[1] = ParameterHelper.GetVarCharPar("@strain", StrainCode, 2, ParameterDirection.Input);

        rdr = DataAccess.GetDataReaderStoredProc(WebConfigSettings.Configurations.Bull_ConnectionString,
                                                      "Rpt405_Sale_SireCertificates", procParams, rdr);
        if (rdr.HasRows)
        {
            int ordCalf_SN = rdr.GetOrdinal("Calf_SN");
            int ordStrain_Code = rdr.GetOrdinal("Strain_Code");
            int ordHerd_Code = rdr.GetOrdinal("Herd_Code");
            int ordYr_Code = rdr.GetOrdinal("Yr_Code");
            int ordTag_Num = rdr.GetOrdinal("Tag_Num");
            int ordCalf_Id = rdr.GetOrdinal("Calf_Id");
            int ordHideColour_Code = rdr.GetOrdinal("HideColour_Code");
            int ordBirth_Date = rdr.GetOrdinal("Birth_Date");
            int ordBirth_Wt = rdr.GetOrdinal("BW_ADJ");
            int ordADG_BW = rdr.GetOrdinal("ADG_BW_ADJ");
            int ordAgeOfDam = rdr.GetOrdinal("AgeOfDam");
            int ordCCID = rdr.GetOrdinal("CCID");

            int ordBreederName = rdr.GetOrdinal("BreederName");
            int ordBreederCompany = rdr.GetOrdinal("BreederCompany");

            int ordFeedlot_ID = rdr.GetOrdinal("Feedlot_ID");
            int ordLast_Wt = rdr.GetOrdinal("Last_Wt");
            int ordBackFat = rdr.GetOrdinal("BACKFAT_ADJ");
            int ordDay365_Wt = rdr.GetOrdinal("YW_ADJ");
            int ordStn_ADG = rdr.GetOrdinal("Stn_ADG");
            int ordScrotal_Circum = rdr.GetOrdinal("SCROTCIRC_ADJ");
            
            int ordBW_EBV = rdr.GetOrdinal("BW_EBV");
            int ordWWD_EBV = rdr.GetOrdinal("WWD_EBV");
            int ordYWT_EBV = rdr.GetOrdinal("YWT_EBV");
            int ordSC_EBV = rdr.GetOrdinal("SC_EBV");
            int ordBF_EBV = rdr.GetOrdinal("BF_EBV");
            int ordWWM_EBV = rdr.GetOrdinal("WWM_EBV");
            int ordMW_EBV = rdr.GetOrdinal("MW_EBV");
            int ordH18M_EBV = rdr.GetOrdinal("H18M_EBV"); 
            int ordRFI_EBV = rdr.GetOrdinal("RFI_EBV");


            int ordSelGrp = rdr.GetOrdinal("SelGrp");
            int ordPriceCDN = rdr.GetOrdinal("SG_CDN");
            int ordPriceUSD = rdr.GetOrdinal("SG_USD");

            while (rdr.Read())
            {
                int calfSN = ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalf_SN), typeof(int), Constants.InitializeInt));
                Rpt405_DataItem di = new Rpt405_DataItem(calfSN);

                di.Strain_Code = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordStrain_Code), typeof(string), Constants.InitializeString));
                di.Herd_Code = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordHerd_Code), typeof(string), Constants.InitializeString));
                di.Yr_Code = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordYr_Code), typeof(string), Constants.InitializeString));
                di.Tag_Num = ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordTag_Num), typeof(int), Constants.InitializeInt));
                di.Calf_Id =         ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalf_Id), typeof(string), Constants.InitializeString));
                di.HideColour_Code = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordHideColour_Code), typeof(string), Constants.InitializeString));
                di.Birth_Date = ((DateTime)ParameterUtils.SafeGetValue(rdr.GetValue(ordBirth_Date), typeof(DateTime), Constants.InitializeDateTime));
                di.SelectionGroup = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordSelGrp), typeof(string), Constants.InitializeString));
                di.PriceCDN = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordPriceCDN), typeof(decimal), Constants.InitializeDecimal));
                di.PriceUSD = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordPriceUSD), typeof(decimal), Constants.InitializeDecimal));

                di.BreederName = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordBreederName), typeof(string), Constants.InitializeString));
                di.BreederCompany = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordBreederCompany), typeof(string), Constants.InitializeString));

                di.AgeOfDam = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordAgeOfDam), typeof(Int16), Constants.InitializeShort));

                di.CCID = ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordCCID), typeof(int), Constants.InitializeInt));

                di.Feedlot_ID = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordFeedlot_ID), typeof(string), Constants.InitializeString));
                di.BACKFAT_ADJ = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordBackFat), typeof(Int16), Constants.InitializeShort));
                di.BW_ADJ = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordBirth_Wt), typeof(Int16), Constants.InitializeShort));
                di.YW_ADJ = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordDay365_Wt), typeof(Int16), Constants.InitializeShort));
                di.OffTest_Wt = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordLast_Wt), typeof(Int16), Constants.InitializeShort));
                di.ADG_BW_ADJ = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordADG_BW), typeof(decimal), Constants.InitializeDecimal));
                di.Stn_ADG = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordStn_ADG), typeof(decimal), Constants.InitializeDecimal));
                di.SCROTCIRC_ADJ = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordScrotal_Circum), typeof(decimal), Constants.InitializeDecimal));
                di.BW_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordBW_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.WWD_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordWWD_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.YWT_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordYWT_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.SC_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordSC_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.BF_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordBF_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.WWM_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordWWM_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.MW_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordMW_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.H18M_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordH18M_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.RFI_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordRFI_EBV), typeof(decimal), Constants.InitializeDecimal));

                lst.Add(di);
            }
        }
        return lst;
    }

}