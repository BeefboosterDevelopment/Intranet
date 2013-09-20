using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Beefbooster.DataAccessLibrary;
using Beefbooster.DataAccessLibrary.Domain;
using Beefbooster.Web;

namespace App_Code.RPT
{
    public class Rpt024HeiferSelection
    {
        public Rpt024HeiferSelection(int herdSN, int yearBorn)
        {
            BeefboosterHerd = new BBDataHelper().GetHerd(herdSN);
            YearBorn = yearBorn;
        }

        #region Properties

        public int NumberQualifyingCalves { get; set; }

        public BBHerd BeefboosterHerd { get; set; }

        public int YearBorn { get; set; }

        public bool IncludePulledCalves { get; set; }

        public bool IncludeCalvesFromHeifers { get; set; }

        public int MaxBWT { get; set; }

        public int MinBWT { get; set; }

        public int TopN { get; set; }

        public string ReportMode { get; set; }

        #endregion

        public void SetDefaults()
        {
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
                int ordInclPulled = rdr.GetOrdinal("AssistedBirth_Flag");
                int ordInclHeifer = rdr.GetOrdinal("FromHeifer_Flag");
                if (rdr.Read())
                {
                    MinBWT = rdr.GetInt16(ordMinBWT);
                    MaxBWT = rdr.GetInt16(ordMaxBWT);
                    IncludePulledCalves = rdr.GetBoolean(ordInclPulled);
                    IncludeCalvesFromHeifers = rdr.GetBoolean(ordInclHeifer);
                }
            }
        }


        /// <summary>
        /// Run the stored procedure and collect the data
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Rpt024DataItem> GetData()
        {
            var lst = new List<Rpt024DataItem>();

            var procParams = new SqlParameter[9];
            procParams[0] = ParameterHelper.GetIntegerPar("@herdSN", BeefboosterHerd.SN, ParameterDirection.Input);
            procParams[1] = ParameterHelper.GetIntegerPar("@birthYear", YearBorn, ParameterDirection.Input);
            procParams[2] = ParameterHelper.GetIntegerPar("@minBWT", MinBWT, ParameterDirection.Input);
            procParams[3] = ParameterHelper.GetIntegerPar("@maxBWT", MaxBWT, ParameterDirection.Input);
            procParams[4] = ParameterHelper.GetIntegerPar("@nTop", TopN, ParameterDirection.Input);
            procParams[5] = ParameterHelper.GetBitPar("@discardHeiferCalves", !IncludeCalvesFromHeifers,
                                                      ParameterDirection.Input);
            procParams[6] = ParameterHelper.GetBitPar("@discardPulledCalves", !IncludePulledCalves,
                                                      ParameterDirection.Input);
            procParams[7] = ParameterHelper.GetVarCharPar("@ReportMode", ReportMode, 5, ParameterDirection.Input);
            procParams[8] = new SqlParameter
                                {
                                    ParameterName = "@NumberQualifyingCalves",
                                    Direction = ParameterDirection.Output,
                                    SqlDbType = SqlDbType.Int
                                };

            var objCommand = new SqlCommand
                                 {CommandText = "Rpt024_HeiferSelection", CommandType = CommandType.StoredProcedure};

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

                    int ordH18MEbv = rdr.GetOrdinal("H18M_EBV");
                    int ordH18MEbvAcc = rdr.GetOrdinal("H18M_EBV_REL");

                    int ordAdjBWT = rdr.GetOrdinal("BW_ADJ");
                    int ordAdjWeanWt = rdr.GetOrdinal("WW_ADJ");
                    int ordADGBW = rdr.GetOrdinal("ADG_BW_ADJ");

                    int ordSelIndex = rdr.GetOrdinal("Selection_Index");
                    //int ordSelIndexAcc = rdr.GetOrdinal("Selection_Index_Accuracy");
                    int ordSelIndexRank = rdr.GetOrdinal("Selection_Index_Rank");

                    int ordSireCalfId = rdr.GetOrdinal("Sire_Calf_Id");
                    int ordDamId = rdr.GetOrdinal("Dam_Id");
                    int ordDamAge = rdr.GetOrdinal("DamAge");
                    int ordUdderScore = rdr.GetOrdinal("Udder_Score");
                    int ordTeatScore = rdr.GetOrdinal("Teat_Score");
                    int ordNumCalvesWeaned = rdr.GetOrdinal("NumCalvesWeaned");

                    int ordH18MWt = rdr.GetOrdinal("H18MW_ADJ");
                    int ordYearlingWt = rdr.GetOrdinal("YW_ADJ");
                    int ordPostWeanGain = rdr.GetOrdinal("PostWeanGain");

                    while (rdr.Read())
                    {
                        var di = new Rpt024DataItem {CalfId = rdr.GetString(ordCalfId)};
                        DateTime bdate = rdr.GetDateTime(ordBirthDate);
                        //di.BirthDate = bdate.Day + "-" + GetMonth(bdate.Month);
                        di.BirthDate = bdate.ToString("dd-MMM");

                        if (!rdr.IsDBNull(ordAdjBWT))
                            di.AdjBirthWt = rdr.GetInt32(ordAdjBWT);

                        if (!rdr.IsDBNull(ordBWTEbv))
                            di.BirthWtEbv = rdr.GetDecimal(ordBWTEbv);

                        if (!rdr.IsDBNull(ordBWTEbvAcc))
                            di.BirthWtEbvAcc = GetAccuracyValue(rdr, ordBWTEbvAcc);

                        if (!rdr.IsDBNull(ordAdjWeanWt))
                            di.AdjWeanWt = rdr.GetInt32(ordAdjWeanWt);

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

                        if (!rdr.IsDBNull(ordH18MEbv))
                            di.H18MEbv = rdr.GetDecimal(ordH18MEbv);
                        if (!rdr.IsDBNull(ordH18MEbvAcc))
                            di.H18MEbvAcc = GetAccuracyValue(rdr, ordH18MEbvAcc);

                        if (!rdr.IsDBNull(ordADGBW))
                            di.ADGBW = rdr.GetDecimal(ordADGBW);

                        if (!rdr.IsDBNull(ordSelIndex))
                        {
                            di.SelectionIndex = decimal.Round(rdr.GetDecimal(ordSelIndex), 0);
                            di.SelectionRank = rdr.GetInt32(ordSelIndexRank);
                            //if (!rdr.IsDBNull(ordSelIndexAcc))
                            //    di.SelectionAcc = GetAccuracyValue(rdr, ordSelIndexAcc);
                            di.SelectionAcc = null;
                        }


                        if (!rdr.IsDBNull(ordSireCalfId))
                            di.SireId = rdr.GetString(ordSireCalfId);

                        di.DamId = rdr.GetString(ordDamId);
                        di.AgeOfDam = rdr.GetInt32(ordDamAge);
                        di.TeatScore = !rdr.IsDBNull(ordTeatScore) ? rdr.GetByte(ordTeatScore) : 0;
                        di.UdderScore = !rdr.IsDBNull(ordUdderScore) ? rdr.GetByte(ordUdderScore) : 0;
                        di.NumberWeaned = !rdr.IsDBNull(ordNumCalvesWeaned) ? rdr.GetInt32(ordNumCalvesWeaned) : 0;

                        if (!rdr.IsDBNull(ordH18MWt))
                            di.H18MWt = rdr.GetInt32(ordH18MWt);
                        if (!rdr.IsDBNull(ordYearlingWt))
                            di.YearlingWt = rdr.GetInt32(ordYearlingWt);
                        if (!rdr.IsDBNull(ordPostWeanGain))
                            di.PostWeanGain = rdr.GetDecimal(ordPostWeanGain);

                        lst.Add(di);
                    }
                }
            }

            // Retrieve the output parameter - AFTER the Read() method
            NumberQualifyingCalves = (Int32) objCommand.Parameters["@numberQualifyingCalves"].Value;

            return lst;
        }

        private decimal GetAccuracyValue(SqlDataReader rdr, int ord)
        {
            return decimal.Round(rdr.GetDecimal(ord)*100M, 0, MidpointRounding.AwayFromZero);
        }
    }
}