using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Beefbooster.DataAccessLibrary;


namespace Beefbooster.ASREML
{
    [Serializable]
    public class ASREMLExportSingleStrain
    {
        private const char TabChar = '\t';
        private readonly string _bullConnectionString;
        private readonly string _cowCalfConnectionString;
        private readonly string _dataFileFolderPath;       
        private readonly string _strainCode;

        public ASREMLExportSingleStrain(string strainCode, string datafileFolderPath,
                                        string cowCalfConnectionString, string bullConnectionString)
        {
            _strainCode = strainCode;
            _cowCalfConnectionString = cowCalfConnectionString;
            _bullConnectionString = bullConnectionString;
            _dataFileFolderPath = datafileFolderPath;
        }

        private string StrainCode
        {
            get { return _strainCode; }
        }

        private string DataFileFolderPath
        {
            get { return _dataFileFolderPath; }
        }

        public void Export()
        {
            List<ASREMLCalf> listOfCalves = GetData();

            UpdateDamWts(listOfCalves);


            /*
             * March 21 2011
             * 
             * The ASREML model software requires each calf to have a dam.
             * 
             * The only way it knows about the dam is through the Dam's Calf Serial Number
             * 
             * New herds (i.e. GJ) had their dams were added to tblDam mechanically and do not have a row in tblCalf to support them
             * 
             * This class will add a bogus calf row into the exported data set to simulate the existance of the calf row
             * 
             * The newly added bogus calf will have limited properties with data, but its mere existance
             * will enable ASREML to properly build the calf's pedigree tree (i.e. each calf has a mother)
             * 
             * Each bogus calf will have an impossible Serial Number ( something > 3 million )
             * 
             */
/*
            if (StrainCode == "M1")
            {
                // NOTE: this only works for M1 calves in the GJ herd that born in the previous calving year (current year - 1)
                var fixDamCalfSn = new FixMissingGJDamCalfSn(DateTime.Now.Year - 1, "M1", 91 /* GJ ASREML Herd Id is 91 #1#,
                                                           listOfCalves);
                List<ASREMLCalf> listToAdd = fixDamCalfSn.FilterCalvesWithoutDamCalfSNs();
                List<ASREMLCalf> newCalves = fixDamCalfSn.GenerateNewCalves(listToAdd);
                listOfCalves.AddRange(newCalves);
                listOfCalves = fixDamCalfSn.UpdateOriginalCalves(listOfCalves, newCalves);
            }

*/


            var fi = new FileInfo(DataFileFolderPath + StrainCode + ".dat");
            if (fi.Exists)
                fi.Delete();
            StreamWriter df = fi.CreateText();

            var comparer = new CalfDOBSorter();
            listOfCalves.Sort(comparer);


            foreach (ASREMLCalf calf in listOfCalves)
            {
                df.WriteLine(calf.DataRow());
            }

            df.Close();

            WriteHeaderFile(listOfCalves.Count, fi.ToString());

            return;
        }





        private void UpdateDamWts(IEnumerable<ASREMLCalf> lst)
        {
            int currentDamSn = -1;
            var sortedList = new SortedList();
            var damWtList = new List<ASREMLDamWeight>();

            try
            {
                var objPars = new SqlParameter[1];
                objPars[0] = ParameterHelper.GetVarCharPar("@strain", StrainCode, 2, ParameterDirection.Input);

                // execute the procedure or query
                using (var objDataReader =  DataAccess.GetDataReaderStoredProc(_cowCalfConnectionString, "ASREML_GetDamWtData", objPars, null))
                {
                    if (objDataReader.HasRows)
                    {
                        int ordDamSn = objDataReader.GetOrdinal("Dam_SN");
                        int ordWt = objDataReader.GetOrdinal("Dam_Wt");
                        int ordAge = objDataReader.GetOrdinal("AgeInDays");

                        while (objDataReader.Read())
                        {
                            Int32 damSn = objDataReader.GetInt32(ordDamSn) + 4000000;
                            Int16 damWt = objDataReader.GetInt16(ordWt);
                            Int32 damAge = objDataReader.GetInt32(ordAge);

                            if (damSn != currentDamSn)
                            {
                                // save current wt set into the sorted list
                                if ((currentDamSn > 0) && (damWtList.Count > 0))
                                    sortedList.Add(currentDamSn, damWtList);
                                // and reset for new Dam SN
                                damWtList = new List<ASREMLDamWeight>();
                                currentDamSn = damSn;
                            }

                            // add current Wt into our current list
                            damWtList.Add(new ASREMLDamWeight(damWt, damAge));
                        }
                    }
                    if (!objDataReader.IsClosed)
                        objDataReader.Close();
                }
            }
            catch (SqlException sqlx)
            {
                throw new ApplicationException(
                    "Failed to get data for ASREML due to database error while reading dam weights. See stored procedure ASREML_GetDamWtData.",
                    sqlx);
            }
            catch (Exception ex)
            {
                // do something with the exception
                throw new ApplicationException(
                    "Failed to get data for ASREML. Error while reading dam weights. See stored procedure ASREML_GetDamWtData.",
                    ex);
            }


            foreach (var calf in lst.Where(c => c.DamSn > 0).ToList())
            {
                calf.DamWts = sortedList.ContainsKey(calf.DamSn)
                                    ? (List<ASREMLDamWeight>) sortedList[calf.DamSn]
                                    : new List<ASREMLDamWeight>();

            }
        }

        private static Decimal GetMaxOrDecimal(IDataRecord rd, int columnOrdinal)
        {
            return rd.IsDBNull(columnOrdinal) ? decimal.MaxValue : rd.GetDecimal(columnOrdinal);
        }

        private static DateTime GetMaxOrDateTime(IDataRecord rd, int columnOrdinal)
        {
            return rd.IsDBNull(columnOrdinal) ? DateTime.MaxValue : rd.GetDateTime(columnOrdinal);
        }

        private static Boolean GetBoolean(IDataRecord rd, int columnOrdinal, Boolean defaultValue)
        {
            return rd.IsDBNull(columnOrdinal) ? defaultValue : rd.GetBoolean(columnOrdinal);
        }

        //public static byte GetMaxOrTinyInt(IDataRecord rd, int columnOrdinal)
        //{
        //    return rd.IsDBNull(columnOrdinal) ? byte.MaxValue : rd.GetByte(columnOrdinal);
        //}

        private static int GetMaxOrInt32(IDataRecord rd, int columnOrdinal)
        {
            return rd.IsDBNull(columnOrdinal) ? int.MaxValue : rd.GetInt32(columnOrdinal);
        }

        private static short GetMaxOrShort(IDataRecord rd, int columnOrdinal)
        {
            return rd.IsDBNull(columnOrdinal) ? short.MaxValue : rd.GetInt16(columnOrdinal);
        }

        //public static double GetMaxOrDouble(IDataRecord rd, int columnOrdinal)
        //{
        //    return rd.IsDBNull(columnOrdinal) ? double.MaxValue : rd.GetDouble(columnOrdinal);
        //}

        private static char GetChar(IDataRecord rd, int columnOrdinal, char defaultValue)
        {
            if (rd.IsDBNull(columnOrdinal))
                return defaultValue;
            string v = rd.GetString(columnOrdinal);
            char[] va = v.ToCharArray();
            if (v.Length > 0)
                return va[0];

            return defaultValue;
        }

        private List<ASREMLCalf> GetData()
        {
            var lst = new List<ASREMLCalf>();

            try
            {
                var objPars = new SqlParameter[1];
                objPars[0] = ParameterHelper.GetVarCharPar("@strain", StrainCode, 2, ParameterDirection.Input);

                var cnxn = new SqlConnection(_bullConnectionString);
                var cmd = new SqlCommand();
                cmd.Parameters.Add(new SqlParameter("@strain", StrainCode));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ASReml_Get_Data_For_Strain";
                cmd.Connection = cnxn;
                cmd.CommandTimeout = 500;
                cnxn.Open();
                using (var objDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (objDataReader.HasRows)
                    {
                        int ordStrainCode = objDataReader.GetOrdinal("Strain_Code");
                        int ordCalfSn = objDataReader.GetOrdinal("Calf_SN");
                        int ordDamSn = objDataReader.GetOrdinal("Dam_SN");
                        int ordCalfHerdId = objDataReader.GetOrdinal("CalfHerd_Id");
                        int ordBirthDate = objDataReader.GetOrdinal("Birth_Date");
                        int ordSireSn = objDataReader.GetOrdinal("Sire_SN");
                        int ordSexCode = objDataReader.GetOrdinal("Sex_Code");
                        int ordBirthWt = objDataReader.GetOrdinal("RawBirth_Wt");
                        int ordTwinFlag = objDataReader.GetOrdinal("Twin_Flag");
                        int ordWeanWt = objDataReader.GetOrdinal("RawWean_Wt");
                        int ordWeanDate = objDataReader.GetOrdinal("Wean_Date");
                        int ordWeanAge = objDataReader.GetOrdinal("WeanAge");

                        int ordHeiferYearlingWtAge = objDataReader.GetOrdinal("HeifYearlingWt_Age");
                        int ordHeiferYearlingWt = objDataReader.GetOrdinal("HeifYearling_Wt");
                        int ordHeifer18MonthWtAge = objDataReader.GetOrdinal("Heif18MonthWt_Age");
                        int ordHeifer18MonthWt = objDataReader.GetOrdinal("Heif18Month_Wt");

                        int ordAgeOfDam = objDataReader.GetOrdinal("AgeOfDam");
                        int ordDamAgeGroup = objDataReader.GetOrdinal("DamAgeGroup");
                        int ordDamBirthYear = objDataReader.GetOrdinal("DamBirth_Year");
                        int ordDamCalfSn = objDataReader.GetOrdinal("DamCalf_SN");
                        int ordDamHerdId = objDataReader.GetOrdinal("DamHerd_Id");

                        int ordOntestWt = objDataReader.GetOrdinal("Ontest_Wt");
                        int ordLastTestWt = objDataReader.GetOrdinal("LastTest_Wt");
                        int ordLastTestWtAge = objDataReader.GetOrdinal("LastTestWt_Age");

                        int ordDaysOnTest = objDataReader.GetOrdinal("DaysOnTest");
                        int ordBullBackFat = objDataReader.GetOrdinal("BullBack_Fat");
                        int ordBullBackFatAge = objDataReader.GetOrdinal("BullBackFat_Age");
                        int ordRawSc = objDataReader.GetOrdinal("RawSC");
                        int ordRawScAge = objDataReader.GetOrdinal("RawSC_Age");

                        int ordRfi = objDataReader.GetOrdinal("RFI");
                        int ordRfiFat = objDataReader.GetOrdinal("RFI_Fat");
                        int ordRfiAge = objDataReader.GetOrdinal("RFI_Age");

                        int ordCGCode = objDataReader.GetOrdinal("ContemporaryGroupCode");


                        while (objDataReader.Read())
                        {
                            string strainCode = objDataReader.GetString(ordStrainCode);
                            int calfSn = objDataReader.GetInt32(ordCalfSn);


                            int damSn = objDataReader.IsDBNull(ordDamSn)
                                            ? 0
                                            : GetMaxOrInt32(objDataReader, ordDamSn) + 4000000;
                                        
    
                            // Sep 22 2011 make sure Dams are unique among calves
                            int herdId = GetMaxOrInt32(objDataReader, ordCalfHerdId);
                            int sireSn = GetMaxOrInt32(objDataReader, ordSireSn);
                            char sexCode = GetChar(objDataReader, ordSexCode, 'X');
                            bool isTwin = GetBoolean(objDataReader, ordTwinFlag, false);
                            DateTime birthDate = GetMaxOrDateTime(objDataReader, ordBirthDate);
                            short birthWt = GetMaxOrShort(objDataReader, ordBirthWt);

                            // create a new calf data object
                            var di = new ASREMLCalf(strainCode, calfSn, damSn, herdId, sireSn, birthDate, birthWt,
                                                    sexCode,
                                                    isTwin)
                                         {
                                             WeanDate = GetMaxOrDateTime(objDataReader, ordWeanDate),
                                             WeanAge = GetMaxOrShort(objDataReader, ordWeanAge),
                                             WeanWt = GetMaxOrShort(objDataReader, ordWeanWt)
                                         };

                            // Jan 18 2008 - add the yearling weights for heifer calves
                            if (sexCode == 'F')
                            {
                                di.HeiferYearlingWtAge = GetMaxOrShort(objDataReader, ordHeiferYearlingWtAge);
                                if (di.HeiferYearlingWtAge > 0)
                                    di.HeiferYearlingWt = GetMaxOrShort(objDataReader, ordHeiferYearlingWt);
                                di.Heifer18MonthWtAge = GetMaxOrShort(objDataReader, ordHeifer18MonthWtAge);
                                if (di.Heifer18MonthWtAge > 0)
                                    di.Heifer18MonthWt = GetMaxOrShort(objDataReader, ordHeifer18MonthWt);
                            }

                            di.AgeOfDam = GetMaxOrShort(objDataReader, ordAgeOfDam);
                            di.DamAgeGroup = GetMaxOrShort(objDataReader, ordDamAgeGroup);
                            di.DamBirthYear = GetMaxOrShort(objDataReader, ordDamBirthYear);
                            di.DamCalfSn = GetMaxOrInt32(objDataReader, ordDamCalfSn);
                            di.DamHerdId = GetMaxOrInt32(objDataReader, ordDamHerdId);

                            di.ContemporaryGroupCode = objDataReader.IsDBNull(ordCGCode) ? "NA" : objDataReader.GetString(ordCGCode);

                            // initialize all bull test data to "N/A" 
                            //  - to cover all females, steers and untested bulls
                            di.DaysOnTest = short.MaxValue;
                            di.Rfi = decimal.MaxValue;
                            di.RfiFat = decimal.MaxValue;
                            di.RfiAge = short.MaxValue;
                            di.OnTestWt = short.MaxValue;
                            di.LastTestWtAge = short.MaxValue;
                            di.LastTestWt = short.MaxValue;
                            di.BullBackFatAge = short.MaxValue;
                            di.BullBackFat = short.MaxValue;
                            di.ScrotalCircumAge = short.MaxValue;
                            di.ScrotalCircum = decimal.MaxValue;

                            // Bull data
                            if (sexCode == 'M')
                            {
                                // Dec 2010 - get the RFI data items (from Olds College)
                                di.Rfi = GetMaxOrDecimal(objDataReader, ordRfi);
                                di.RfiFat = GetMaxOrDecimal(objDataReader, ordRfiFat);
                                di.RfiAge = GetMaxOrShort(objDataReader, ordRfiAge);

                                // if di.OnTest_Wt is 0 then just forget the rest
                                di.OnTestWt = GetMaxOrShort(objDataReader, ordOntestWt);

                                if (di.OnTestWt > 0)
                                {
                                    if (di.DaysOnTest > 0)
                                        di.DaysOnTest = GetMaxOrShort(objDataReader, ordDaysOnTest);

                                    di.LastTestWtAge = GetMaxOrShort(objDataReader, ordLastTestWtAge);
                                    if (di.LastTestWtAge > 0)
                                        di.LastTestWt = GetMaxOrShort(objDataReader, ordLastTestWt);

                                    di.BullBackFatAge = GetMaxOrShort(objDataReader, ordBullBackFatAge);
                                    if (di.BullBackFatAge > 0)
                                        di.BullBackFat = GetMaxOrShort(objDataReader, ordBullBackFat);
                                    di.ScrotalCircumAge = GetMaxOrShort(objDataReader, ordRawScAge);
                                    if (di.ScrotalCircumAge > 0)
                                        di.ScrotalCircum = GetMaxOrDecimal(objDataReader, ordRawSc);
                                }
                            }
                            lst.Add(di);
                        }
                    }
                    if (!objDataReader.IsClosed)
                        objDataReader.Close();
                }
            }
            catch (SqlException sqlx)
            {
                throw new ApplicationException("Failed to get data for ASREML due to a database error.", sqlx);
            }
            catch (Exception ex)
            {
                // do something with the exception
                throw new ApplicationException("Failed to get data for ASREML.", ex);
            }

            return lst;
        }

        #region Header File

        private void WriteHeaderFile(int numWritten, string dataFileName)
        {
            var fi = new FileInfo(string.Format(DataFileFolderPath + "{0}_DataStructure.txt", StrainCode));
            if (fi.Exists)
                fi.Delete();
            StreamWriter df = fi.CreateText();
            df.WriteLine("Strain: " + StrainCode);
            df.WriteLine("Data File: " + dataFileName);
            df.WriteLine("As At: " + DateTime.Now);
            df.WriteLine("Number of Rows: " + numWritten);
            df.WriteLine();

            df.WriteLine("Missing data values have an NA as a place holder.\n");
            df.WriteLine("Column Definitions");
            int colNum = 1;
            WriteHeaderRow(df, colNum++, "Strain            ", "Strain code", ""); //
            WriteHeaderRow(df, colNum++, "Calf Animal Id    ", "Calf serial number", ""); //
            WriteHeaderRow(df, colNum++, "Dam Animal Id     ", "Serial number of dam when she was a calf", ""); //
            WriteHeaderRow(df, colNum++, "Sire Animal Id    ", "Serial number of sire when he was a calf", ""); //
            WriteHeaderRow(df, colNum++, "Sex               ", "Male=1,Female=2,Steer=3", "1 - 3"); //
            WriteHeaderRow(df, colNum++, "Herd Id           ", "Calf Herd Id (2 digit for ASREML only)", ""); //
            WriteHeaderRow(df, colNum++, "Twin              ", "This calf has a twin", "1=No 2=Yes"); //
            WriteHeaderRow(df, colNum++, "Birth Date        ", "Calf birth date (dd-mm-ccyy)", ""); //
            WriteHeaderRow(df, colNum++, "Birth Day         ", "Day number of birth date", ""); //
            WriteHeaderRow(df, colNum++, "Birth Month       ", "Month number of birth date", ""); //
            WriteHeaderRow(df, colNum++, "Birth Year        ", "Year of birth date", ""); //
            WriteHeaderRow(df, colNum++, "Birth Weight      ", "Raw birth weight of calf", ""); //
            WriteHeaderRow(df, colNum++, "Wean Date         ", "Wean date (dd-mm-ccyy)", ""); //
            WriteHeaderRow(df, colNum++, "Wean Day          ", "Day number of wean date", ""); //
            WriteHeaderRow(df, colNum++, "Wean Month        ", "Month number of wean date", ""); //
            WriteHeaderRow(df, colNum++, "Wean Year         ", "Year of wean date", "");
            WriteHeaderRow(df, colNum++, "Wean Age          ", "Age in days at weaning", "");
            WriteHeaderRow(df, colNum++, "Wean Weight       ", "Raw weaning weight of calf", "");
            WriteHeaderRow(df, colNum++, "Age Of Dam        ", "Age in years of dam when this calf was born", "");
            WriteHeaderRow(df, colNum++, "Dam's Age Group   ", "0-2 yrs  3 yrs  4 yrs   5-10 yrs   11+ yrs", "1 - 5");
            WriteHeaderRow(df, colNum++, "Dam's Birth Year  ", "The year the dam was born", "");
            WriteHeaderRow(df, colNum++, "Dam's Alt Ref Id  ", "Dam SN (unique within dams)", "");
            WriteHeaderRow(df, colNum++, "Dam's Herd SN     ", "Dam herd id (2 digit for ASREML only)", "");

            WriteHeaderRow(df, colNum++, "Yearling Wt Age   ", "Age in days of yearling wt (heifers only)", "");
            WriteHeaderRow(df, colNum++, "Yearling Wt       ", "Yearling weight (heifers only)", "");

            WriteHeaderRow(df, colNum++, "18 Month Wt Age   ", "Age in days of 18 month wt (heifers only)", "");
            WriteHeaderRow(df, colNum++, "18 Month Wt       ", "18 month weight (heifers only)", "");

            WriteHeaderRow(df, colNum++, "Dam Weight 1      ", "First weight of this dam", "");
            WriteHeaderRow(df, colNum++, "Dam Weight Age 1  ", "Age of dam (in days) when first weight was recorded", "");
            WriteHeaderRow(df, colNum++, "Dam Weight 2      ", "Second weight of this dam", "");
            WriteHeaderRow(df, colNum++, "Dam Weight Age 2  ", "Age of dam (in days) when second weight was recorded",
                           "");
            WriteHeaderRow(df, colNum++, "Dam Weight 3      ", "Third weight of this dam", "");
            WriteHeaderRow(df, colNum++, "Dam Weight Age 3  ", "Age of dam (in days) when third weight was recorded", "");
            WriteHeaderRow(df, colNum++, "Dam Weight 4      ", "Fourth weight of this dam", "");
            WriteHeaderRow(df, colNum++, "Dam Weight Age 4  ", "Age of dam (in days) when fourth weight was recorded",
                           "");
            WriteHeaderRow(df, colNum++, "Start Test Weight ", "", "");
            WriteHeaderRow(df, colNum++, "Last Test Wt      ", "Last weight observation (off-test wt in most cases)", "");
            WriteHeaderRow(df, colNum++, "Last Test Wt Age  ", "Age in days when last weight observation was recorded",
                           "");
            WriteHeaderRow(df, colNum++, "Days On Test      ",
                           "Number of days between start test weight and end test weight", "");
            WriteHeaderRow(df, colNum++, "Bull Back Fat     ", "Back fat", "");
            WriteHeaderRow(df, colNum++, "Bull Back Fat Age ", "Age in days when bull back fat measurment was recorded",
                           "");
            WriteHeaderRow(df, colNum++, "Scrotal Circum    ", "Scrotal circumference", "");
            WriteHeaderRow(df, colNum++, "Scrotal Circum Age ", "Age in days when scrotal circumference was recorded",
                           "");

            WriteHeaderRow(df, colNum++, "RFI               ", "RFI (source: Olds College)", "");
            WriteHeaderRow(df, colNum++, "RFI Fat           ", "RFI: fat adjusted", "");
            WriteHeaderRow(df, colNum++, "RFI Age           ", "RFI Age", "");

            WriteHeaderRow(df, colNum,   "Contemorary Group ", "Optional contemporary group code (e.g. M4 RFI)", "");

            df.Close();
        }

        private static void WriteHeaderRow(TextWriter sw, int colNumber, string colName, string colDesc,
                                           string rangeDescr)
        {
            if (sw == null) throw new ArgumentNullException("sw");
            sw.Write(colNumber);
            sw.Write(TabChar);
            sw.Write(colName);
            sw.Write(TabChar);
            sw.Write(colDesc);
            sw.Write(TabChar);
            if (!string.IsNullOrEmpty(rangeDescr))
                sw.Write("Valid Range: " + rangeDescr);
            sw.WriteLine();
        }

        #endregion
    }


    public class CalfDOBSorter : IComparer<ASREMLCalf>
    {
        #region IComparer<ASREMLCalf> Members

        public int Compare(ASREMLCalf x, ASREMLCalf y)
        {
            return DateTime.Compare(x.BirthDate, y.BirthDate);
        }

        #endregion
    }
}