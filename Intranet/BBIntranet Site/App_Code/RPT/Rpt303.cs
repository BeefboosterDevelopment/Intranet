using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Beefbooster.BusinessLogic;
using Beefbooster.DataAccessLibrary;
using Beefbooster.DataAccessLibrary.Domain;
using Beefbooster.Web;

#region class Rpt303_DataItem
/// <summary>
/// This class holds 1 bull
/// </summary>
public class Rpt303_DataItem
{
    // Make sure you have a default constructor 
    public Rpt303_DataItem() { }
    public Rpt303_DataItem(int calfSN) 
    {
        _calf_SN = calfSN;
    }

    private int _calf_SN;
    private Int16 _calfBirthYr_Num;
    private string _strain_Code;
    private string _herd_Code;
    private string _yr_Code;
    private int _tag_Num;
    private string _calf_Id;
    private string _sireCalf_Id;
    private DateTime _birth_Date;
    private DateTime _wean_Date;
    private decimal _weanAge;
    private string _hideColour_Code;
    private bool _isClosed;
    private bool _isCulled;
    private bool _isCertified;
    private bool _isHytester;
    private int _dam_SN;
    private string _dam_Id;
    private Int16 _ageOfDam;
    private Int16 _dam_Wt;
    private string _feedlot_ID;
    private string _pen;
    private byte _teat;
    private byte _udder;
    private byte _fF;
    private byte _fL;
    private byte _hF;
    private byte _hL;

    private byte _morph;
    private byte _motil;
    private byte _conc;
    private byte _disp;
    private decimal _backFat;

    private Int16 _birth_Wt;
    private Int16 _rawBirth_Wt;
    private Int16 _wean_Wt;
    private Int16 _rawWean_Wt;
    private decimal _adg_bw;
    private Int16 _day365_Wt;
    private decimal _stn_adg;
    private decimal _scrotal_Circum;
    private decimal _rawSC;
    private Int16 _rawBack_Fat;
    private decimal _vi;



    public int Calf_SN { get { return _calf_SN; } set { _calf_SN = value; } }
    public Int16 CalfBirthYr_Num { get { return _calfBirthYr_Num; } set { _calfBirthYr_Num = value; } }
    public string Strain_Code { get { return _strain_Code; } set { _strain_Code = value; } }
    public string Herd_Code { get { return _herd_Code; } set { _herd_Code = value; } }
    public string Yr_Code { get { return _yr_Code; } set { _yr_Code = value; } }
    public int Tag_Num { get { return _tag_Num; } set { _tag_Num = value; } }
    public string Calf_Id { get { return _calf_Id; } set { _calf_Id = value; } }
    public DateTime Birth_Date { get { return _birth_Date; } set { _birth_Date = value; } }
    public DateTime Wean_Date { get { return _wean_Date; } set { _wean_Date = value; } }
    public decimal WeanAge { get { return _weanAge; } set { _weanAge = value; } }
    public string HideColour_Code { get { return _hideColour_Code; } set { _hideColour_Code = value; } }
    public bool IsClosed { get { return _isClosed; } set { _isClosed = value; } }
    public bool IsCulled { get { return _isCulled; } set { _isCulled = value; } }
    public bool IsCertified { get { return _isCertified; } set { _isCertified = value; } }
    public bool IsHytester { get { return _isHytester; } set { _isHytester = value; } }
    public int Dam_SN { get { return _dam_SN; } set { _dam_SN = value; } }
    public string Dam_Id { get { return _dam_Id; } set { _dam_Id = value; } }
    public Int16 AgeOfDam { get { return _ageOfDam; } set { _ageOfDam = value; } }
    public string Feedlot_ID { get { return _feedlot_ID; } set { _feedlot_ID = value; } }
    public string Pen { get { return _pen; } set { _pen = value; } }
    public Int16 Dam_Wt { get { return _dam_Wt; } set { _dam_Wt = value; } }
    public byte Teat { get { return _teat; } set { _teat = value; } }
    public byte Udder { get { return _udder; } set { _udder = value; } }

    public byte FF { get { return _fF; } set { _fF = value; } }
    public byte FL { get { return _fL; } set { _fL = value; } }
    public byte HF { get { return _hF; } set { _hF = value; } }
    public byte HL { get { return _hL; } set { _hL = value; } }

    public byte Morph { get { return _morph; } set { _morph = value; } }
    public byte Motil { get { return _motil; } set { _motil = value; } }
    public byte Conc { get { return _conc; } set { _conc = value; } }
    public byte Disp { get { return _disp; } set { _disp = value; } }
    public decimal Back_Fat { get { return _backFat; } set { _backFat = value; } }

    public string SireCalf_Id { get { return _sireCalf_Id; } set { _sireCalf_Id = value; } }
    
    public Int16 Birth_Wt { get { return _birth_Wt; } set { _birth_Wt = value; } }
    public Int16 RawBirth_Wt { get { return _rawBirth_Wt; } set { _rawBirth_Wt = value; } }
    public Int16 Wean_Wt { get { return _wean_Wt; } set { _wean_Wt = value; } }
    public Int16 RawWean_Wt { get { return _rawWean_Wt; } set { _rawWean_Wt = value; } }
    public Int16 RawBack_Fat { get { return _rawBack_Fat; } set { _rawBack_Fat = value; } }
    public decimal ADG_BW { get { return _adg_bw; } set { _adg_bw = value; } }
    public Int16 Day365_Wt { get { return _day365_Wt; } set { _day365_Wt = value; } }
    public decimal Stn_ADG { get { return _stn_adg; } set { _stn_adg = value; } }
    public decimal Scrotal_Circum { get { return _scrotal_Circum; } set { _scrotal_Circum = value; } }
    public decimal RawSC { get { return _rawSC; } set { _rawSC = value; } }    
    public decimal VI { get { return _vi; } set { _vi = value; } }
    public string DOB 
    { 
        get 
        {
            string dob = _birth_Date.ToString("MMM dd");
            return dob;
        } 
    }
}
#endregion

public class Rpt303_DataObject
{
    //private const string REPORT_NAME = "Bull Teststation Performance Report";

    public Rpt303_DataObject() { }


    public Rpt303_DataObject(int bornInYear, string strain, int herdSN, string reportStyle)
    {
        if (herdSN > 0)
            _bbHerd = new BBDataHelper().GetHerd(herdSN);
        else
            _bbHerd = null;

        _strain = strain;
        _yearBorn = bornInYear;
        _reportStyle = reportStyle;
    }
    #region Properties

    private readonly int _yearBorn;
    public int YearBorn
    {
        get { return _yearBorn; }
    }

    private readonly BBHerd _bbHerd;
    public BBHerd Herd
    {
        get { return _bbHerd; }
    }

    private readonly string _strain;
    public string Strain
    {
        get { return _strain; }
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
            int toYear = _yearBorn + 1;
            return string.Format("{0}-{1}", _yearBorn, toYear);
        }
    }

    #endregion

    public List<Rpt303_DataItem> GetData()
    {
        List<Rpt303_DataItem> lst = new List<Rpt303_DataItem>();
        SqlDataReader rdr = null;

        SqlParameter[] procParams = new SqlParameter[3];
        procParams[0] = ParameterHelper.GetVarCharPar("@strain", Strain, 2, ParameterDirection.Input);
        if (Herd != null)
            procParams[1] = ParameterHelper.GetIntegerPar("@herdSN", Herd.SN, ParameterDirection.Input);
        else
            procParams[1] = ParameterHelper.GetIntegerPar("@herdSN", 0, ParameterDirection.Input);

        procParams[2] = ParameterHelper.GetIntegerPar("@birthYr", YearBorn, ParameterDirection.Input);

        rdr = DataAccess.GetDataReaderStoredProc(WebConfigSettings.Configurations.Bull_ConnectionString,
                                                      "Rpt303_Bull_PerformanceReport", procParams, rdr);
        if (rdr.HasRows)
        {
            // get the ordinal indexes into the read buffer                
            int ordCalf_SN = rdr.GetOrdinal("Calf_SN");
            int ordCalfBirthYr_Num = rdr.GetOrdinal("CalfBirthYr_Num");
            int ordStrain_Code = rdr.GetOrdinal("Strain_Code");
            int ordHerd_Code = rdr.GetOrdinal("Herd_Code");
            int ordYr_Code = rdr.GetOrdinal("Yr_Code");
            int ordTag_Num = rdr.GetOrdinal("Tag_Num");
            int ordCalf_Id = rdr.GetOrdinal("Calf_Id");
            int ordBirth_Date = rdr.GetOrdinal("Birth_Date");
            int ordWean_Date = rdr.GetOrdinal("Wean_Date");
            int ordWeanAge = rdr.GetOrdinal("WeanAge");
            int ordHideColour_Code = rdr.GetOrdinal("HideColour_Code");
            int ordIsClosed = rdr.GetOrdinal("IsClosed");
            int ordIsCulled = rdr.GetOrdinal("IsCulled");
            int ordIsCertified = rdr.GetOrdinal("IsCertified");
            int ordIsHytester = rdr.GetOrdinal("IsHytester");
            int ordDam_SN = rdr.GetOrdinal("Dam_SN");
            int ordDam_Id = rdr.GetOrdinal("Dam_Id");
            int ordAgeOfDam = rdr.GetOrdinal("AgeOfDam");
            int ordFeedlot_ID = rdr.GetOrdinal("Feedlot_ID");
            int ordPen = rdr.GetOrdinal("Pen");
            int ordDam_Wt = rdr.GetOrdinal("Dam_Wt");
            int ordTeat = rdr.GetOrdinal("Teat");
            int ordUdder = rdr.GetOrdinal("Udder");
            int ordFF = rdr.GetOrdinal("FF");
            int ordFL = rdr.GetOrdinal("FL");
            int ordHF = rdr.GetOrdinal("HF");
            int ordHL = rdr.GetOrdinal("HL");
            int ordMorph = rdr.GetOrdinal("Morph");
            int ordMotil = rdr.GetOrdinal("Motil");
            int ordConc = rdr.GetOrdinal("Conc");
            int ordDisp = rdr.GetOrdinal("Disp");
            int ordSireCalf_Id = rdr.GetOrdinal("SireCalf_Id");
            int ordBack_Fat = rdr.GetOrdinal("Backfat");
            int ordRawBack_Fat  = rdr.GetOrdinal("BullBack_Fat");
            int ordBirth_Wt = rdr.GetOrdinal("Birth_Wt");
            int ordRawBirth_Wt = rdr.GetOrdinal("RawBirth_Wt");
            int ordWean_Wt = rdr.GetOrdinal("Wean_Wt");
            int ordRawWean_Wt = rdr.GetOrdinal("RawWean_Wt");
            int ordADG_BW = rdr.GetOrdinal("ADG_BW");
            int ordDay365_Wt = rdr.GetOrdinal("Day365_Wt");
            int ordStn_ADG = rdr.GetOrdinal("Stn_ADG");
            int ordRawSC = rdr.GetOrdinal("RawSC");
            int ordScrotal_Circum = rdr.GetOrdinal("Scrotal_Circum");
            int ordVI = rdr.GetOrdinal("VI");

            while (rdr.Read())
            {
                int calfSN =
                    ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalf_SN), typeof(int), Constants.InitializeInt));
                Rpt303_DataItem di = new Rpt303_DataItem(calfSN);

                di.CalfBirthYr_Num =
                    ((Int16)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordCalfBirthYr_Num), typeof(Int16),
                                                 Constants.InitializeShort));
                di.Strain_Code =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordStrain_Code), typeof(string),
                                                 Constants.InitializeString));
                di.Herd_Code =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordHerd_Code), typeof(string), Constants.InitializeString));
                di.Yr_Code =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordYr_Code), typeof(string), Constants.InitializeString));
                di.Tag_Num =
                    ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordTag_Num), typeof(int), Constants.InitializeInt));
                di.Calf_Id =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordCalf_Id), typeof(string), Constants.InitializeString));
                di.Birth_Date =
                    ((DateTime)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordBirth_Date), typeof(DateTime),
                                                 Constants.InitializeDateTime));
                di.Wean_Date =
                    ((DateTime)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordWean_Date), typeof(DateTime),
                                                 Constants.InitializeDateTime));
                 di.WeanAge =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordWeanAge), typeof(decimal), Constants.InitializeDecimal));

                di.HideColour_Code =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordHideColour_Code), typeof(string),
                                                 Constants.InitializeString));
                di.IsClosed = ((bool)ParameterUtils.SafeGetValue(rdr.GetValue(ordIsClosed), typeof(bool), false));
                di.IsCulled = ((bool)ParameterUtils.SafeGetValue(rdr.GetValue(ordIsCulled), typeof(bool), false));
                di.IsCertified =
                    ((bool)ParameterUtils.SafeGetValue(rdr.GetValue(ordIsCertified), typeof(bool), false));
                di.IsHytester = ((bool)ParameterUtils.SafeGetValue(rdr.GetValue(ordIsHytester), typeof(bool), false));
                di.Dam_SN =
                    ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordDam_SN), typeof(int), Constants.InitializeInt));
                di.Dam_Id =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordDam_Id), typeof(string), Constants.InitializeString));

                di.SireCalf_Id =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordSireCalf_Id), typeof(string), Constants.InitializeString));

                di.AgeOfDam =
                    ((Int16)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordAgeOfDam), typeof(Int16), Constants.InitializeShort));
                di.Feedlot_ID =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordFeedlot_ID), typeof(string),
                                                 Constants.InitializeString));
                di.Pen =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordPen), typeof(string), Constants.InitializeString));

                di.Dam_Wt =
                    ((Int16)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordDam_Wt), typeof(Int16), Constants.InitializeShort));
                di.Teat =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordTeat), typeof(byte), Constants.InitializeByte));
                di.Udder =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordUdder), typeof(byte), Constants.InitializeByte));

                di.FF =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordFF), typeof(byte), Constants.InitializeByte));
                di.FL =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordFL), typeof(byte), Constants.InitializeByte));
                di.HF =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordHF), typeof(byte), Constants.InitializeByte));
                di.HL =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordHL), typeof(byte), Constants.InitializeByte));

                di.Morph =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordMorph), typeof(byte), Constants.InitializeByte));
                di.Motil =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordMotil), typeof(byte), Constants.InitializeByte));
                di.Conc =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordConc), typeof(byte), Constants.InitializeByte));

                di.Disp =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordDisp), typeof(byte), Constants.InitializeByte));
                di.RawBack_Fat =
                    ((Int16)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordRawBack_Fat), typeof(Int16), Constants.InitializeShort));

                di.Back_Fat =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordBack_Fat), typeof(decimal), Constants.InitializeDecimal));

                di.Birth_Wt =
                    ((Int16)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordBirth_Wt), typeof(Int16), Constants.InitializeShort));

                di.RawBirth_Wt =
                    ((Int16)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordRawBirth_Wt), typeof(Int16), Constants.InitializeShort));

                di.Wean_Wt =
                    ((Int16)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordWean_Wt), typeof(Int16), Constants.InitializeShort));

                di.RawWean_Wt =
                    ((Int16)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordRawWean_Wt), typeof(Int16), Constants.InitializeShort));

                di.Day365_Wt =
                    ((Int16)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordDay365_Wt), typeof(Int16), Constants.InitializeShort));

                di.ADG_BW =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordADG_BW), typeof(decimal), Constants.InitializeDecimal));
                di.Stn_ADG =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordStn_ADG), typeof(decimal), Constants.InitializeDecimal));
                di.Scrotal_Circum =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordScrotal_Circum), typeof(decimal),
                                                 Constants.InitializeDecimal));
                di.RawSC =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordRawSC), typeof(decimal), Constants.InitializeDecimal));

                //di.DamMatureWt_Exp =
                //    ((Int16)
                //     ParameterUtils.SafeGetValue(rdr.GetValue(ordDamMatureWt_Exp), typeof(Int16), Constants.InitializeShort));

                di.VI =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordVI), typeof(decimal), Constants.InitializeDecimal));
                lst.Add(di);
            }
        }
        return lst;
    }
}