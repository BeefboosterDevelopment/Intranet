using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Beefbooster.BusinessLogic;
using Beefbooster.DataAccessLibrary;
using Beefbooster.DataAccessLibrary.Domain;
using Beefbooster.Web;

#region class Rpt310_DataItem
public class Rpt310_DataItem
{
    public Rpt310_DataItem() { }
    public Rpt310_DataItem(int calfSN) 
    {
        Calf_SN = calfSN;
    }

    public int Calf_SN { get; set; }
    public int Dam_SN { get; set; }
    public int CalfBirthYr_Num { get; set; }
    public string Strain_Code { get; set; }
    public string Herd_Code { get; set; }
    public string Calf_Id { get; set; }
    public string Birth_Date { get; set; }
    public int RawBirth_Wt { get; set; }


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

    public string SireCalf_Id { get; set; }
    public int NumCalvesSired { get; set; }

    public string Dam_Id { get; set; }
    public int AgeOfDam { get; set; }
    public int DamWt { get; set; }

    public decimal Stn_ADG { get; set; }
    public decimal BW_ADJ { get; set; }
    public decimal WW_ADJ { get; set; }
    public decimal YW_ADJ { get; set; }
    public decimal H18MW_ADJ { get; set; }
    public decimal ADG_BW_ADJ { get; set; }
    public decimal BACKFAT_ADJ { get; set; }
    public decimal SCROTCIRC_ADJ { get; set; }
}
#endregion


public class Rpt310_DataObject
{

    public Rpt310_DataObject(string accountNo1, string accountNo2, decimal medRel, decimal highRel)
    {
        var breeders = BBDataHelper.GetBreeders().ToList();
        _breeder1 = breeders.FirstOrDefault(b => b.AccountNo == accountNo1);
        if (!string.IsNullOrEmpty(accountNo2))
            _breeder2 = breeders.FirstOrDefault(b => b.AccountNo == accountNo2);
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

    private readonly BBBreeder _breeder1;

    public BBBreeder Breeder1
    {
        get { return _breeder1; }
    }


    private readonly BBBreeder _breeder2;

    public BBBreeder Breeder2
    {
        get { return _breeder2; }
    }

    #endregion

    public IEnumerable<Rpt310_DataItem> GetData()
    {
        List<Rpt310_DataItem> lst = new List<Rpt310_DataItem>();

        if ((Breeder1 == null) && (Breeder2 == null)) return lst;
        var b1 = Breeder1 == null ? "" : Breeder1.AccountNo;
        var b2 = Breeder2 == null ? "" : Breeder2.AccountNo;

        List<SqlParameter> procParams = new List<SqlParameter>
                                            {
                                                ParameterHelper.GetVarCharPar("@breederAccountNo1", b1, 20, ParameterDirection.Input),
                                                ParameterHelper.GetVarCharPar("@breederAccountNo2", b2, 20, ParameterDirection.Input)
                                            };

        SqlDataReader rdr = DataAccess.GetDataReaderStoredProc(WebConfigSettings.Configurations.Bull_ConnectionString,
                                                               "Rpt310_Bull_BatteryPerformanceReport", procParams);
        if (rdr.HasRows)
        {
            // get the ordinal indexes into the read buffer                
            int ordCalf_SN = rdr.GetOrdinal("CalfSN");
            int ordCalfBirthYr_Num = rdr.GetOrdinal("BirthYear");
            int ordStrain_Code = rdr.GetOrdinal("Strain");
            int ordHerd_Code = rdr.GetOrdinal("Herd");
            int ordCalf_Id = rdr.GetOrdinal("CalfId");

            int ordBirth_Date = rdr.GetOrdinal("DOB");
            int ordCalfRawBirth_Wt = rdr.GetOrdinal("RawBirth_Wt");

            int ordSireCalf_Id = rdr.GetOrdinal("SireCalfId");
            int ordNumCalvesSired = rdr.GetOrdinal("NumCalvesSired");

            int ordDam_SN = rdr.GetOrdinal("Dam_SN");
            int ordDam_Id = rdr.GetOrdinal("Dam_Id");
            int ordAgeOfDam = rdr.GetOrdinal("AgeOfDam");
            int ordDamWt = rdr.GetOrdinal("Dam_Wt");

            int ordBW_EBV = rdr.GetOrdinal("BWTEBV");
            int ordWWD_EBV = rdr.GetOrdinal("WWTEBV");
            int ordYWT_EBV = rdr.GetOrdinal("YWTEBV");
            int ordSC_EBV = rdr.GetOrdinal("SCEBV");
            int ordBF_EBV = rdr.GetOrdinal("BFEBV");
            int ordWWM_EBV = rdr.GetOrdinal("MilkEBV");
            int ordMW_EBV = rdr.GetOrdinal("MWTEBV");
            int ordRFI_EBV = rdr.GetOrdinal("RFIEBV");
            int ordH18M_EBV = rdr.GetOrdinal("H18MEBV");

            int ordBW_EBV_REL = rdr.GetOrdinal("BWTReliability");
            int ordWWD_EBV_REL = rdr.GetOrdinal("WWTReliability");
            int ordYWT_EBV_REL = rdr.GetOrdinal("YWTReliability");
            int ordSC_EBV_REL = rdr.GetOrdinal("SCReliability");
            int ordBF_EBV_REL = rdr.GetOrdinal("BFReliability");
            int ordWWM_EBV_REL = rdr.GetOrdinal("MilkReliability");
            int ordMW_EBV_REL = rdr.GetOrdinal("MWTReliability");
            int ordRFI_EBV_REL = rdr.GetOrdinal("RFIReliability");
            int ordH18M_EBV_REL = rdr.GetOrdinal("H18MReliability");

            int ordBW_ADJ = rdr.GetOrdinal("BW_ADJ");
            int ordWW_ADJ = rdr.GetOrdinal("WW_ADJ");
            int ordYW_ADJ = rdr.GetOrdinal("YW_ADJ");
            int ordH18MW_ADJ = rdr.GetOrdinal("H18MW_ADJ");
            int ordADG_BW_ADJ = rdr.GetOrdinal("ADG_BW_ADJ");
            int ordBACKFAT_ADJ = rdr.GetOrdinal("BACKFAT_ADJ");
            int ordSCROTCIRC_ADJ = rdr.GetOrdinal("SCROTCIRC_ADJ");
            int ordStn_ADG = rdr.GetOrdinal("ADG_ON_TEST");

            int ordSEL_IDX = rdr.GetOrdinal("SelIdx");

            while (rdr.Read())
            {
                  int calfSN =
                    ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalf_SN), typeof(int), Constants.InitializeInt));

                Rpt310_DataItem di = new Rpt310_DataItem(calfSN);
                
                di.Dam_SN =
                    ((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordDam_SN), typeof(int), Constants.InitializeInt));

                di.CalfBirthYr_Num =((int)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalfBirthYr_Num), typeof(int),
                                                 Constants.InitializeInt));

                di.RawBirth_Wt = (int)ParameterUtils.SafeGetValue(rdr.GetValue(ordCalfRawBirth_Wt), typeof(int),
                                                        Constants.InitializeInt);
                di.Strain_Code =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordStrain_Code), typeof(string),
                                                 Constants.InitializeString));
                di.Herd_Code =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordHerd_Code), typeof(string), Constants.InitializeString));
                                
                di.Calf_Id =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordCalf_Id), typeof(string), Constants.InitializeString));

                di.Birth_Date =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordBirth_Date), typeof(string),
                                                 Constants.InitializeString));
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

                di.SEL_IDX =
                    ((decimal)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordSEL_IDX), typeof(decimal), Constants.InitializeDecimal));


                di.SireCalf_Id =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordSireCalf_Id), typeof(string), Constants.InitializeString));
                di.NumCalvesSired =
                    ((int)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordNumCalvesSired), typeof(int), Constants.InitializeInt));


                di.Dam_Id =
                    ((string)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordDam_Id), typeof(string), Constants.InitializeString));
                di.AgeOfDam =
                    ((int)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordAgeOfDam), typeof(int), Constants.InitializeInt));
                di.DamWt =
                    ((int)
                     ParameterUtils.SafeGetValue(rdr.GetValue(ordDamWt), typeof(int), Constants.InitializeInt));

                
                di.BW_ADJ = ((decimal) ParameterUtils.SafeGetValue(rdr.GetValue(ordBW_ADJ), typeof(decimal), Constants.InitializeDecimal));
                di.WW_ADJ = ((decimal) ParameterUtils.SafeGetValue(rdr.GetValue(ordWW_ADJ), typeof(decimal), Constants.InitializeDecimal));
                di.YW_ADJ = ((decimal) ParameterUtils.SafeGetValue(rdr.GetValue(ordYW_ADJ), typeof(decimal), Constants.InitializeDecimal));
                di.H18MW_ADJ = ((decimal) ParameterUtils.SafeGetValue(rdr.GetValue(ordH18MW_ADJ), typeof(decimal), Constants.InitializeDecimal));
                di.ADG_BW_ADJ = ((decimal) ParameterUtils.SafeGetValue(rdr.GetValue(ordADG_BW_ADJ), typeof(decimal), Constants.InitializeDecimal));
                di.BACKFAT_ADJ = ((decimal) ParameterUtils.SafeGetValue(rdr.GetValue(ordBACKFAT_ADJ), typeof(decimal), Constants.InitializeDecimal));
                di.SCROTCIRC_ADJ = ((decimal) ParameterUtils.SafeGetValue(rdr.GetValue(ordSCROTCIRC_ADJ), typeof(decimal), Constants.InitializeDecimal));
                di.Stn_ADG = ((decimal)ParameterUtils.SafeGetValue(rdr.GetValue(ordStn_ADG), typeof(decimal), Constants.InitializeDecimal));
                lst.Add(di);
            }
        }
        return lst;
    }
}