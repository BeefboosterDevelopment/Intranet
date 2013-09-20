using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Beefbooster.BusinessLogic;
using Beefbooster.DataAccessLibrary;
using Beefbooster.DataAccessLibrary.Domain;
using Beefbooster.Web;

#region class Rpt304_DataItem
/// <summary>
/// This class holds 1 bull
/// </summary>
public class Rpt304_DataItem
{
    // Make sure you have a default constructor 
    public Rpt304_DataItem() { }
    public Rpt304_DataItem(int calfSN) 
    {
        Calf_SN = calfSN;
    }

    public int Calf_SN { get; set; }
    public int Dam_SN { get; set; }
    public short CalfBirthYr_Num { get; set; }
    public string Strain_Code { get; set; }
    public string Herd_Code { get; set; }
    public string Yr_Code { get; set; }
    public int Tag_Num { get; set; }
    public string Feedlot_ID { get; set; }
    public string Pen { get; set; }
    public string Calf_Id { get; set; }
    public DateTime Birth_Date { get; set; }
    public short RawBirth_Wt { get; set; }

    public decimal SEL_IDX { get; set; }
    
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

    public string DOB { get {string dob = Birth_Date.ToString("MMM dd"); return dob;} }

    public string SireCalf_Id { get; set; }

    public string Dam_Id { get; set; }
    public short AgeOfDam { get; set; }
    public short Dam_Wt { get; set; }
    public byte Teat { get; set; }
    public byte Udder { get; set; }

    public byte FF { get; set; }
    public byte FL { get; set; }
    public byte HF { get; set; }
    public byte HL { get; set; }

    public byte Morph { get; set; }
    public byte Motil { get; set; }
    public byte Conc { get; set; }
    public byte Disp { get; set; }
    public decimal Stn_ADG { get; set; }
    
    // Added Mar 2012 Adjusted Values
    public Int16 BW_ADJ { get; set; }
    public Int16 WW_ADJ { get; set; }
    public Int16 YW_ADJ { get; set; }
    public Int16 H18MW_ADJ { get; set; }
    public decimal ADG_BW_ADJ { get; set; }
    public Int16 BACKFAT_ADJ { get; set; }
    public decimal SCROTCIRC_ADJ { get; set; }
}
#endregion


public class Rpt304_DataObject
{
    public Rpt304_DataObject() { }

    public Rpt304_DataObject(int bornInYear, string strain, int herdSN, string reportStyle, int sortOrder, decimal medRel, decimal highRel)
    {
        _bbHerd = herdSN > 0 ? new BBDataHelper().GetHerd(herdSN) : null;

        _strain = strain;
        _yearBorn = bornInYear;
        _reportStyle = reportStyle;
        _sortOrder = sortOrder;
        _medReliability = medRel;
        _highReliability = highRel;
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

    private readonly int _sortOrder;
    public int SortOrder
    {
        get { return _sortOrder; }
    }

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

    public string ReportYear
    {
        get
        {
            int toYear = _yearBorn + 1;
            return string.Format("{0}-{1}", _yearBorn, toYear);
        }
    }

    #endregion

    public List<Rpt304_DataItem> GetData()
    {
        List<Rpt304_DataItem> lst = new List<Rpt304_DataItem>();
        SqlDataReader rdr = null;

        SqlParameter[] procParams = new SqlParameter[4];
        procParams[0] = ParameterHelper.GetVarCharPar("@strain", Strain, 2, ParameterDirection.Input);
        procParams[1] = Herd != null ? ParameterHelper.GetIntegerPar("@herdSN", Herd.SN, ParameterDirection.Input) : ParameterHelper.GetIntegerPar("@herdSN", 0, ParameterDirection.Input);
        procParams[2] = ParameterHelper.GetIntegerPar("@birthYr", YearBorn, ParameterDirection.Input);
        procParams[3] = ParameterHelper.GetIntegerPar("@sortOrder", SortOrder, ParameterDirection.Input);

        rdr = DataAccess.GetDataReaderStoredProc(WebConfigSettings.Configurations.Bull_ConnectionString,
                                                      "Rpt304_Bull_PerformanceReport", procParams, rdr);
        if (rdr.HasRows)
        {
            // get the ordinal indexes into the read buffer                
            int ordCalf_SN = rdr.GetOrdinal("Calf_SN");
            int ordDam_SN = rdr.GetOrdinal("Dam_SN");
            int ordCalfBirthYr_Num = rdr.GetOrdinal("CalfBirthYr_Num");
            int ordStrain_Code = rdr.GetOrdinal("Strain_Code");
            int ordHerd_Code = rdr.GetOrdinal("Herd_Code");
            int ordYr_Code = rdr.GetOrdinal("Yr_Code");
            int ordTag_Num = rdr.GetOrdinal("Tag_Num");
            int ordFeedlot_ID = rdr.GetOrdinal("Feedlot_ID");
            int ordPen = rdr.GetOrdinal("Pen");
            int ordCalf_Id = rdr.GetOrdinal("Calf_Id");

            int ordSEL_IDX = rdr.GetOrdinal("SEL_IDX");
            
            int ordBirth_Date = rdr.GetOrdinal("Birth_Date");
            int ordCalfRawBirth_Wt = rdr.GetOrdinal("RawBirth_Wt");
            
            int ordSireCalf_Id = rdr.GetOrdinal("SireCalf_Id");

            int ordDam_Id = rdr.GetOrdinal("Dam_Id");
            int ordAgeOfDam = rdr.GetOrdinal("AgeOfDam");
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

            int ordBW_ADJ = rdr.GetOrdinal("BW_ADJ");
            int ordWW_ADJ = rdr.GetOrdinal("WW_ADJ");
            int ordYW_ADJ = rdr.GetOrdinal("YW_ADJ");
            int ordH18MW_ADJ = rdr.GetOrdinal("H18MW_ADJ");
            int ordADG_BW_ADJ = rdr.GetOrdinal("ADG_BW_ADJ");
            int ordBACKFAT_ADJ = rdr.GetOrdinal("BACKFAT_ADJ");
            int ordSCROTCIRC_ADJ = rdr.GetOrdinal("SCROTCIRC_ADJ");

            int ordStn_ADG = rdr.GetOrdinal("Stn_ADG");

            while (rdr.Read())
            {
                int calfSN =
                    ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalf_SN), typeof(int), Constants.InitializeInt));
                
                Rpt304_DataItem di = new Rpt304_DataItem(calfSN);

                di.Dam_SN =
                    ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordDam_SN), typeof(int), Constants.InitializeInt));

                di.CalfBirthYr_Num =((Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalfBirthYr_Num), typeof(Int16),
                                                 Constants.InitializeShort));

                di.RawBirth_Wt = (Int16)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalfRawBirth_Wt), typeof(Int16),
                                                        Constants.InitializeShort);
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
                
                di.Feedlot_ID =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordFeedlot_ID), typeof(string),
                                                 Constants.InitializeString));
                di.Pen =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordPen), typeof(string), Constants.InitializeString));
                di.Calf_Id =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordCalf_Id), typeof(string), Constants.InitializeString));

                // Added Mar 2012
                di.SEL_IDX =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordSEL_IDX), typeof(decimal), Constants.InitializeDecimal));

                di.Birth_Date =
                    ((DateTime)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordBirth_Date), typeof(DateTime),
                                                 Constants.InitializeDateTime));
                di.BW_EBV =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordBW_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.WWD_EBV =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordWWD_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.YWT_EBV =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordYWT_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.SC_EBV =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordSC_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.BF_EBV =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordBF_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.WWM_EBV =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordWWM_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.MW_EBV =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordMW_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.RFI_EBV =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordRFI_EBV), typeof(decimal), Constants.InitializeDecimal));
                di.H18M_EBV =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordH18M_EBV), typeof(decimal), Constants.InitializeDecimal));

                di.BW_EBV_REL =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordBW_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.WWD_EBV_REL =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordWWD_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.YWT_EBV_REL =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordYWT_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.SC_EBV_REL =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordSC_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.BF_EBV_REL =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordBF_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.WWM_EBV_REL =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordWWM_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.MW_EBV_REL =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordMW_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.RFI_EBV_REL =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordRFI_EBV_REL), typeof(decimal), Constants.InitializeDecimal));
                di.H18M_EBV_REL =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordH18M_EBV_REL), typeof(decimal), Constants.InitializeDecimal));

                di.Morph =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordMorph), typeof(byte), Constants.InitializeByte));
                di.Motil =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordMotil), typeof(byte), Constants.InitializeByte));
                di.Conc =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordConc), typeof(byte), Constants.InitializeByte));
                di.Disp =
                    ((byte)ParameterUtils.SafeGetValue(rdr.GetValue(ordDisp), typeof(byte), Constants.InitializeByte));
                di.SireCalf_Id =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordSireCalf_Id), typeof(string), Constants.InitializeString));
                di.Dam_Id =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordDam_Id), typeof(string), Constants.InitializeString));
                di.AgeOfDam =
                    ((Int16)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordAgeOfDam), typeof(Int16), Constants.InitializeShort));
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


                // Added Mar 2012 Adjusted Values
                di.BW_ADJ = ((Int16) ParameterUtils.SafeGetValue(rdr.GetValue(ordBW_ADJ), typeof(Int16), Constants.InitializeShort));
                di.WW_ADJ = ((Int16) ParameterUtils.SafeGetValue(rdr.GetValue(ordWW_ADJ), typeof(Int16), Constants.InitializeShort));
                di.YW_ADJ = ((Int16) ParameterUtils.SafeGetValue(rdr.GetValue(ordYW_ADJ), typeof(Int16), Constants.InitializeShort));
                di.H18MW_ADJ = ((Int16) ParameterUtils.SafeGetValue(rdr.GetValue(ordH18MW_ADJ), typeof(Int16), Constants.InitializeShort));
                di.ADG_BW_ADJ = ((decimal) ParameterUtils.SafeGetValue(rdr.GetValue(ordADG_BW_ADJ), typeof(decimal), Constants.InitializeDecimal));
                di.BACKFAT_ADJ = ((Int16) ParameterUtils.SafeGetValue(rdr.GetValue(ordBACKFAT_ADJ), typeof(Int16), Constants.InitializeShort));
                di.SCROTCIRC_ADJ = ((decimal) ParameterUtils.SafeGetValue(rdr.GetValue(ordSCROTCIRC_ADJ), typeof(decimal), Constants.InitializeDecimal));
                di.Stn_ADG = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordStn_ADG), typeof(decimal), Constants.InitializeDecimal));

                lst.Add(di);
            }
        }
        return lst;
    }
}