using System;
using System.Collections.Generic;
using System.Data;
using Beefbooster.BusinessLogic;

#region class Rpt402_DataItem
/// <summary>
/// This class holds 1 bull
/// </summary>
public class Rpt402_DataItem
{
    // Make sure you have a default constructor 
    public Rpt402_DataItem() { }
    public Rpt402_DataItem(int calfSN) 
    {
        _calf_SN = calfSN;
    }

    private int _calf_SN;
    private string _strain_Code;
    private string _herd_Code;
    private string _yr_Code;
    private int _tag_Num;
    private string _calf_Id;
    private DateTime _birth_Date;
    //private string _tagColour;
    private string _selectionGroup;
    private decimal _priceCDN;
    private decimal _priceUSD;
    private string _dam_Id;
    private Int16 _ageOfDam;
    private Int16 _dam_Wt;
    private string _feedlot_ID;
    private Int16 _backFat;
    private Int16 _birth_Wt;
    private decimal _adg_bw;
    private Int16 _day365_Wt;
    private decimal _stn_adg;
    private decimal _scrotal_Circum;
    private Int16 _offTest_Wt;
    private decimal _bw_EBV;
    private decimal _wwd_EBV;
    private decimal _pwg_EBV;
    private decimal _sc_EBV;
    private decimal _bf_EBV;
    private decimal _wwm_EBV;
    private decimal _mw_EBV;

    public int Calf_SN { get { return _calf_SN; } set { _calf_SN = value; } }
    public string Strain_Code { get { return _strain_Code; } set { _strain_Code = value; } }
    public string Herd_Code { get { return _herd_Code; } set { _herd_Code = value; } }
    public string Yr_Code { get { return _yr_Code; } set { _yr_Code = value; } }
    public int Tag_Num { get { return _tag_Num; } set { _tag_Num = value; } }
    public string Calf_Id { get { return _calf_Id; } set { _calf_Id = value; } }
    public DateTime Birth_Date { get { return _birth_Date; } set { _birth_Date = value; } }
    //public string TagColour { get { return _tagColour; } set { _tagColour = value; } }
    public string SelectionGroup { get { return _selectionGroup; } set { _selectionGroup = value; } }
    public decimal PriceCDN { get { return _priceCDN; } set { _priceCDN = value; } }
    public decimal PriceUSD { get { return _priceUSD; } set { _priceUSD = value; } }
    public string Dam_Id { get { return _dam_Id; } set { _dam_Id = value; } }
    public Int16 AgeOfDam { get { return _ageOfDam; } set { _ageOfDam = value; } }
    public string Feedlot_ID { get { return _feedlot_ID; } set { _feedlot_ID = value; } }
    public Int16 Dam_Wt { get { return _dam_Wt; } set { _dam_Wt = value; } }
    public Int16 Birth_Wt { get { return _birth_Wt; } set { _birth_Wt = value; } }
    public Int16 OffTest_Wt { get { return _offTest_Wt; } set { _offTest_Wt = value; } }
    public decimal ADG_BW { get { return _adg_bw; } set { _adg_bw = value; } }
    public Int16 Day365_Wt { get { return _day365_Wt; } set { _day365_Wt = value; } }
    public Int16 BullBack_Fat { get { return _backFat; } set { _backFat = value; } }
    public decimal Stn_ADG { get { return _stn_adg; } set { _stn_adg = value; } }
    public decimal Scrotal_Circum { get { return _scrotal_Circum; } set { _scrotal_Circum = value; } }
    public decimal BW_EBV { get { return _bw_EBV; } set { _bw_EBV = value; } }
    public decimal WWD_EBV { get { return _wwd_EBV; } set { _wwd_EBV = value; } }
    public decimal PWG_EBV { get { return _pwg_EBV; } set { _pwg_EBV = value; } }
    public decimal SC_EBV { get { return _sc_EBV; } set { _sc_EBV = value; } }
    public decimal BF_EBV { get { return _bf_EBV; } set { _bf_EBV = value; } }
    public decimal WWM_EBV { get { return _wwm_EBV; } set { _wwm_EBV = value; } }
    public decimal MW_EBV { get { return _mw_EBV; } set { _mw_EBV = value; } }

    public string DOB 
    { 
        get 
        {
            string dob = Constants.InitializeString;
            if (_birth_Date != null)
                dob = _birth_Date.ToString("dd-MMM");
            return dob;
        } 
    }

}
#endregion

public class Rpt402_DataObject
{
    private const string REPORT_NAME = "Sire Selection Sheets";

    public Rpt402_DataObject() { }

    public Rpt402_DataObject(int bornInYear, string strainCode, string reportStyle)
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



    public List<Rpt402_DataItem> GetData()
    {
        List<Rpt402_DataItem> lst = new List<Rpt402_DataItem>();
        // Create an instance of the IDataProvider you require using the DataProviderFactory
        //using (IDataProvider dp = DataProviderFactory.GetDataProvider())
        //{
        //    try
        //    {
        //        dp.ClearParameters();
        //        dp.SetParameter("birthYr", ParameterDirection.Input, ParameterType.Int32, 4, ParameterUtils.SafeSetValue(StartingYearBorn, StartingYearBorn != Constants.InitializeInt));
        //        dp.SetParameter("strain", ParameterDirection.Input, ParameterType.Char, 2, ParameterUtils.SafeSetValue(StrainCode, StrainCode != Constants.InitializeString));
        //        using (IDataReader rdr = dp.ExecuteReader(CommandType.StoredProcedure, "[dbo].[web_SireSelectionSheets]"))
        //        {
        //            int ordCalf_SN          = rdr.GetOrdinal("Calf_SN");
        //            int ordStrain_Code      = rdr.GetOrdinal("Strain_Code");
        //            int ordHerd_Code        = rdr.GetOrdinal("Herd_Code");
        //            int ordYr_Code          = rdr.GetOrdinal("Yr_Code");
        //            int ordTag_Num          = rdr.GetOrdinal("Tag_Num");
        //            int ordCalf_Id          = rdr.GetOrdinal("Calf_Id");
        //            int ordBirth_Date       = rdr.GetOrdinal("Birth_Date");
        //            //int ordTagColour        = rdr.GetOrdinal("Tag_Colour");
        //            int ordSelGrp           = rdr.GetOrdinal("SelGrp");
        //            int ordPriceCDN         = rdr.GetOrdinal("SG_CDN");
        //            int ordPriceUSD         = rdr.GetOrdinal("SG_USD");
        //            int ordDam_Id           = rdr.GetOrdinal("Dam_Id");
        //            int ordAgeOfDam         = rdr.GetOrdinal("AgeOfDam");
        //            int ordFeedlot_ID       = rdr.GetOrdinal("Feedlot_ID");
        //            int ordDam_Wt           = rdr.GetOrdinal("Dam_Wt");
        //            int ordBullBack_Fat     = rdr.GetOrdinal("BullBack_Fat");
        //            int ordLast_Wt          = rdr.GetOrdinal("Last_Wt");
        //            int ordBirth_Wt         = rdr.GetOrdinal("Birth_Wt");
        //            int ordADG_BW           = rdr.GetOrdinal("ADG_BW");
        //            int ordDay365_Wt        = rdr.GetOrdinal("Day365_Wt");
        //            int ordStn_ADG          = rdr.GetOrdinal("Stn_ADG");
        //            int ordScrotal_Circum   = rdr.GetOrdinal("Scrotal_Circum");
        //            int ordBW_EBV           = rdr.GetOrdinal("BW_EBV");
        //            int ordWWD_EBV          = rdr.GetOrdinal("WWD_EBV");
        //            int ordPWG_EBV          = rdr.GetOrdinal("PWG_EBV");
        //            int ordSC_EBV           = rdr.GetOrdinal("SC_EBV");
        //            int ordBF_EBV           = rdr.GetOrdinal("BF_EBV");
        //            int ordWWM_EBV          = rdr.GetOrdinal("WWM_EBV");
        //            int ordMW_EBV           = rdr.GetOrdinal("MW_EBV");

        //            while (rdr.Read())
        //            {
        //                int calfSN = ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalf_SN), typeof(int), Constants.InitializeInt));
        //                Rpt402_DataItem di = new Rpt402_DataItem(calfSN);

        //                di.Strain_Code      = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordStrain_Code), typeof(string), Constants.InitializeString));
        //                di.Herd_Code        = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordHerd_Code), typeof(string), Constants.InitializeString));
        //                di.Yr_Code          = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordYr_Code), typeof(string), Constants.InitializeString));
        //                di.Tag_Num          = ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordTag_Num), typeof(int), Constants.InitializeInt));                                                
        //                di.Calf_Id          = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalf_Id), typeof(string), Constants.InitializeString));
        //                di.Birth_Date       = ((DateTime)ParameterUtils.SafeGetValue(rdr.GetValue(ordBirth_Date), typeof(DateTime), Constants.InitializeDateTime));
        //                //di.TagColour        = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordTagColour), typeof(string), Constants.InitializeString));
        //                di.SelectionGroup   = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordSelGrp), typeof(string), Constants.InitializeString));
        //                di.PriceCDN         = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordPriceCDN), typeof(decimal), Constants.InitializeDecimal));
        //                di.PriceUSD         = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordPriceUSD), typeof(decimal), Constants.InitializeDecimal));

        //                di.Dam_Id           = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordDam_Id ), typeof(string), Constants.InitializeString));
        //                di.AgeOfDam         = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordAgeOfDam), typeof(Int16), Constants.InitializeShort));
        //                di.Feedlot_ID       = ((string)ParameterUtils.SafeGetValue(rdr.GetValue(ordFeedlot_ID), typeof(string), Constants.InitializeString));
        //                di.Dam_Wt           = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordDam_Wt), typeof(Int16), Constants.InitializeShort));
        //                di.BullBack_Fat     = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordBullBack_Fat), typeof(Int16), Constants.InitializeShort));
        //                di.Birth_Wt         = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordBirth_Wt), typeof(Int16), Constants.InitializeShort));
        //                di.Day365_Wt        = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordDay365_Wt), typeof(Int16), Constants.InitializeShort));
        //                di.OffTest_Wt       = ((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordLast_Wt), typeof(Int16), Constants.InitializeShort));
        //                di.ADG_BW           = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordADG_BW), typeof(decimal), Constants.InitializeDecimal));
        //                di.Stn_ADG          = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordStn_ADG), typeof(decimal), Constants.InitializeDecimal));
        //                di.Scrotal_Circum   = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordScrotal_Circum), typeof(decimal), Constants.InitializeDecimal));
        //                di.BW_EBV           = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordBW_EBV), typeof(decimal), Constants.InitializeDecimal));
        //                di.WWD_EBV          = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordWWD_EBV), typeof(decimal), Constants.InitializeDecimal));
        //                di.PWG_EBV          = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordPWG_EBV), typeof(decimal), Constants.InitializeDecimal));
        //                di.SC_EBV           = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordSC_EBV), typeof(decimal), Constants.InitializeDecimal));
        //                di.BF_EBV           = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordBF_EBV), typeof(decimal), Constants.InitializeDecimal));
        //                di.WWM_EBV          = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordWWM_EBV), typeof(decimal), Constants.InitializeDecimal));
        //                di.MW_EBV           = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordMW_EBV), typeof(decimal), Constants.InitializeDecimal));
        //                lst.Add(di);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // do something with the exception
        //        throw new ApplicationException("Failed to retrieve data for the report:" + REPORT_NAME, ex);
        //    }

        //}
        return lst;
    }

}