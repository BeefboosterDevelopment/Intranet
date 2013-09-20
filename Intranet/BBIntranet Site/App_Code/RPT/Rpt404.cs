using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Beefbooster.BusinessLogic;
using Beefbooster.DataAccessLibrary;
using Beefbooster.Web;


public class Rpt404_DataItem
{
    // Make sure you have a default constructor 
    public Rpt404_DataItem() { }
    public Rpt404_DataItem(int calfSN) 
    {
        CalfSN = calfSN;
    }

    public int CalfSN { get; set; }
    public string StrainCode { get; set; }
    public string HerdCode { get; set; }
    public string YrCode { get; set; }
    public int TagNum { get; set; }
    public string CalfId { get; set; }
    public DateTime BirthDate { get; set; }

    public string SelectionGroup { get; set; }
    public decimal PriceCDN { get; set; }
    public decimal PriceUSD { get; set; }
    public Int16 AgeOfDam { get; set; }
    public string Feedlot_ID { get; set; }
    public string HideColour_Code { get; set; }
    public Int16 BW_ADJ { get; set; }
    public Int16 WW_ADJ { get; set; }
    public Int16 YW_ADJ { get; set; }
    public decimal ADG_BW_ADJ { get; set; }
    public Int16 BACKFAT_ADJ { get; set; }
    public decimal SCROTCIRC_ADJ { get; set; }
    public decimal Stn_ADG { get; set; }
    public decimal BW_EBV { get; set; }
    public decimal WWD_EBV { get; set; }
    public decimal YWT_EBV { get; set; }
    public decimal SC_EBV { get; set; }
    public decimal BF_EBV { get; set; }
    public decimal WWM_EBV { get; set; }
    public decimal MW_EBV { get; set; }
    public decimal RFI_EBV { get; set; }
    public decimal H18M_EBV { get; set; }

    public decimal BW_EBV_REL { get; set; }
    public decimal WWD_EBV_REL { get; set; }
    public decimal YWT_EBV_REL { get; set; }
    public decimal SC_EBV_REL { get; set; }
    public decimal BF_EBV_REL { get; set; }
    public decimal WWM_EBV_REL { get; set; }
    public decimal MW_EBV_REL { get; set; }
    public decimal RFI_EBV_REL { get; set; }
    public decimal H18M_EBV_REL { get; set; }


    public string DOB 
    { 
        get
        {
            return BirthDate.ToString("dd-MMM");
        }
    }
}

public class Rpt404_DataObject
{
    public Rpt404_DataObject() { }

    public Rpt404_DataObject(int bornInYear, string strainCode, string reportStyle, decimal medRel, decimal highRel)
    {
        _yearBorn = bornInYear;
        _strainCode = strainCode;
        _reportStyle = reportStyle;
        _medReliability = medRel;
        _highReliability = highRel;
    }
    #region Properties

    private readonly decimal _medReliability;
    public decimal MediumReliability
    {
        get { return _medReliability; }
    }

    private readonly decimal _highReliability;
    public decimal HighReliability
    {
        get { return _highReliability; }
    }

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



    public List<Rpt404_DataItem> GetData()
    {
        List<Rpt404_DataItem> lst = new List<Rpt404_DataItem>();
        SqlDataReader rdr = null;

        SqlParameter[] procParams = new SqlParameter[2];
        procParams[0] = ParameterHelper.GetIntegerPar("@birthYr", YearBorn, ParameterDirection.Input);
        procParams[1] = ParameterHelper.GetVarCharPar("@strain", StrainCode, 2, ParameterDirection.Input);

        rdr = DataAccess.GetDataReaderStoredProc(WebConfigSettings.Configurations.Bull_ConnectionString,
                                                      "Rpt404_Sale_ClientSireSelection", procParams, rdr);
        if (rdr.HasRows)
        {
            int ordCalf_SN = rdr.GetOrdinal("Calf_SN");
            int ordStrain_Code = rdr.GetOrdinal("Strain_Code");
            int ordHerd_Code = rdr.GetOrdinal("Herd_Code");
            int ordYr_Code = rdr.GetOrdinal("Yr_Code");
            int ordTag_Num = rdr.GetOrdinal("Tag_Num");
            int ordCalf_Id = rdr.GetOrdinal("Calf_Id");
            int ordBirth_Date = rdr.GetOrdinal("Birth_Date");

            int ordBW_ADJ = rdr.GetOrdinal("BW_ADJ");
            int ordWW_ADJ = rdr.GetOrdinal("WW_ADJ");
            int ordYW_ADJ = rdr.GetOrdinal("YW_ADJ");
            int ordADG_BW_ADJ = rdr.GetOrdinal("ADG_BW_ADJ");
            int ordBACKFAT_ADJ = rdr.GetOrdinal("BACKFAT_ADJ");
            int ordSCROTCIRC_ADJ = rdr.GetOrdinal("SCROTCIRC_ADJ");

            int ordAgeOfDam = rdr.GetOrdinal("AgeOfDam");
            int ordFeedlot_ID = rdr.GetOrdinal("Feedlot_ID");
            int ordHideColour_Code = rdr.GetOrdinal("HideColour_Code");
            int ordStn_ADG = rdr.GetOrdinal("Stn_ADG");
            
            int ordBW_EBV = rdr.GetOrdinal("BW_EBV");
            int ordWWD_EBV = rdr.GetOrdinal("WWD_EBV");
            int ordYWT_EBV = rdr.GetOrdinal("YWT_EBV");
            int ordSC_EBV = rdr.GetOrdinal("SC_EBV");
            int ordBF_EBV = rdr.GetOrdinal("BF_EBV");
            int ordWWM_EBV = rdr.GetOrdinal("WWM_EBV");
            int ordMW_EBV = rdr.GetOrdinal("MW_EBV");
            int ordRFI_EBV = rdr.GetOrdinal("RFI_EBV");
            int ordH18M_EBV = rdr.GetOrdinal("H18M_EBV");

            int ordBW_EBV_REL = rdr.GetOrdinal("BW_EBV_REL");
            int ordWWD_EBV_REL = rdr.GetOrdinal("WWD_EBV_REL");
            int ordYWT_EBV_REL = rdr.GetOrdinal("YWT_EBV_REL");
            int ordSC_EBV_REL = rdr.GetOrdinal("SC_EBV_REL");
            int ordBF_EBV_REL = rdr.GetOrdinal("BF_EBV_REL");
            int ordWWM_EBV_REL = rdr.GetOrdinal("WWM_EBV_REL");
            int ordMW_EBV_REL = rdr.GetOrdinal("MW_EBV_REL");
            int ordRFI_EBV_REL = rdr.GetOrdinal("RFI_EBV_REL");
            int ordH18M_EBV_REL = rdr.GetOrdinal("H18M_EBV_REL");

            int ordSelGrp = rdr.GetOrdinal("SelGrp");
            int ordPriceCDN = rdr.GetOrdinal("SG_CDN");
            int ordPriceUSD = rdr.GetOrdinal("SG_USD");

            while (rdr.Read())
            {
                int calfSN = ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalf_SN), typeof(int), Constants.InitializeInt));
                Rpt404_DataItem di = new Rpt404_DataItem(calfSN);

                di.StrainCode = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordStrain_Code), typeof(string), Constants.InitializeString));
                di.HerdCode = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordHerd_Code), typeof(string), Constants.InitializeString));
                di.YrCode = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordYr_Code), typeof(string), Constants.InitializeString));
                di.TagNum = ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordTag_Num), typeof(int), Constants.InitializeInt));
                di.CalfId = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalf_Id), typeof(string), Constants.InitializeString));
                di.BirthDate = ((DateTime)ParameterUtils.SafeGetValue(rdr.GetValue(ordBirth_Date), typeof(DateTime), Constants.InitializeDateTime));
                di.SelectionGroup = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordSelGrp), typeof(string), Constants.InitializeString));
                di.PriceCDN = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordPriceCDN), typeof(decimal), Constants.InitializeDecimal));
                di.PriceUSD = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordPriceUSD), typeof(decimal), Constants.InitializeDecimal));

                di.AgeOfDam = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordAgeOfDam), typeof(Int16), Constants.InitializeShort));
                di.Feedlot_ID = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordFeedlot_ID), typeof(string), Constants.InitializeString));
                di.HideColour_Code = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordHideColour_Code), typeof(string), Constants.InitializeString));

                di.BW_ADJ = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordBW_ADJ), typeof(Int16), Constants.InitializeShort));
                di.WW_ADJ = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordWW_ADJ), typeof(Int16), Constants.InitializeShort));
                di.YW_ADJ = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordYW_ADJ), typeof(Int16), Constants.InitializeShort));
                di.ADG_BW_ADJ = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordADG_BW_ADJ), typeof(decimal), Constants.InitializeDecimal));
                di.BACKFAT_ADJ = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordBACKFAT_ADJ), typeof(Int16), Constants.InitializeShort));
                di.SCROTCIRC_ADJ = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordSCROTCIRC_ADJ), typeof(decimal), Constants.InitializeDecimal));

                di.Stn_ADG = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordStn_ADG), typeof(decimal), Constants.InitializeDecimal));
                di.BW_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordBW_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.WWD_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordWWD_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.YWT_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordYWT_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.SC_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordSC_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.BF_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordBF_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.WWM_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordWWM_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.MW_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordMW_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.RFI_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordRFI_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.H18M_EBV = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordH18M_EBV), typeof(decimal), Constants.InitializeDecimal));

                di.BW_EBV_REL = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordBW_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.WWD_EBV_REL = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordWWD_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.YWT_EBV_REL = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordYWT_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.SC_EBV_REL = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordSC_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.BF_EBV_REL = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordBF_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.WWM_EBV_REL = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordWWM_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.MW_EBV_REL = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordMW_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.RFI_EBV_REL = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordRFI_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.H18M_EBV_REL = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordH18M_EBV_REL), typeof(decimal), Constants.InitializeDecimal));

                lst.Add(di);
            }
        }
        return lst;
    }

}