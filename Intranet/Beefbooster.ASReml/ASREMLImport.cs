using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Beefbooster.BusinessLogic;
using Beefbooster.DataAccessLibrary;

namespace Beefbooster.ASREML
{
    public class ASREMLImport
    {
        private const string ASREMLPrefix = "ASReml_Import_";
        private readonly FileInfo _dataFile;
        private readonly string _ebvConnectionString;
        private readonly string _localBulkCopyFolder;
        private readonly string _sqlServerBulkCopyFolder;
        private readonly string _strain;
        private int _controlNumber = Constants.InitializeInt;

        public ASREMLImport(string filePath, string sqlServerBulkCopyFolder, string localBulkCopyFolder,
                             string ebvConnString, int controlNumber)
        {
            if (filePath == null)
                throw new ArgumentNullException(string.Format("Null file name specified"));
            if (filePath.Length == 0)
                throw new ArgumentException("No file name specified");

            _sqlServerBulkCopyFolder = sqlServerBulkCopyFolder;
            _localBulkCopyFolder = localBulkCopyFolder;
            _ebvConnectionString = ebvConnString;
            _controlNumber = controlNumber;
           

            _dataFile = new FileInfo(filePath);
            if (!_dataFile.Exists)
                throw new ApplicationException(string.Format("Selected ASReml data file does not exist:{0}.", filePath));

            // Lookup the strain for this session
            _strain = DiscoverStrain();
            Util.ValidStrain(_strain);
        }

        #region Public Properties

        public string Strain
        {
            get { return _strain; }
        }

        private FileInfo DataFile
        {
            get { return _dataFile; }
        }

        private int ControlNumber
        {
            get
            {
                //Session["EBVControlNumber"] = "abc";// controlNumber;
                // not there? dig it out of the users session
                // if (_controlNumber != Constants.InitializeInt)
                return _controlNumber;


/*
                if (BBWebUtility.UserCache[CacheStaticValues.EBVControlNumber] == null)
                    return Constants.InitializeInt;
                string strControlNumber = BBWebUtility.UserCache[CacheStaticValues.EBVControlNumber].ToString();
                if (string.IsNullOrEmpty(strControlNumber))
                    throw new ApplicationException("No EBV control number found.");
                return (Convert.ToInt32(strControlNumber));
*/

            }
            set
            {
                _controlNumber = value;
                // stuff it into the users session
                //BBWebUtility.UserCache.Insert(CacheStaticValues.EBVControlNumber, _controlNumber);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public string GetStatusMessage()
        {
            string returnValue = "OK";
            var objPars = new SqlParameter[1];
            objPars[0] = ParameterHelper.GetIntegerPar("@controlNumber", ControlNumber, ParameterDirection.Input);
            SqlDataReader rdr = DataAccess.GetDataReaderSingleRowStoredProc(_ebvConnectionString,
                                                                            "GetStatusMessage", objPars, null);
            if (rdr.Read())
                returnValue = rdr.GetString(rdr.GetOrdinal("StatusMessage"));
            return returnValue;
        }

        public bool Validate()
        {
            return (DataFile.Exists);
        }

        // return value of 1 means success
        public int ImportData()
        {
            if (_dataFile == null)
                throw new ApplicationException("No datafile specified");
            if (_dataFile.Length == 0)
                throw new ApplicationException("No datafile specified");

            int status = 1;
            string statusMesasge = "OK";
            int nRowsImported = 0;

            try
            {
                ControlNumber = InsertControlRow();

                FileInfo sqlServerFileInfo = CreateNewCsvFileInfo(_sqlServerBulkCopyFolder);
                FileInfo localFileInfo = CreateNewCsvFileInfo(_localBulkCopyFolder);

                UpdateControlBulkInsertFileInfo(sqlServerFileInfo);

                try
                {
                    // create a new CSV (text) file
                    using (StreamWriter localCsvFile = localFileInfo.CreateText())
                    {
                        // open it 
                        StreamReader inEbvFile = _dataFile.OpenText();

                        // skip the header row
                        if (!inEbvFile.EndOfStream)
                            inEbvFile.ReadLine();

                        // write the data rows into it
                        while (!inEbvFile.EndOfStream)
                        {
                            localCsvFile.WriteLine(string.Format("{0},{1}", ControlNumber, SetNullValues(inEbvFile.ReadLine())));
                            nRowsImported++;
                        }
                        // done - close input file
                        inEbvFile.Close();

                        // and close the output file
                        localCsvFile.Close();
                    }

                    // init a new FileInfo with the freshly written CSV file
                    var fi = new FileInfo(localFileInfo.FullName);

                    if (!fi.Exists)
                        throw new ApplicationException(string.Format("Target file does not exist: {0}", fi.FullName));

                    if (sqlServerFileInfo.Directory != null && !sqlServerFileInfo.Directory.Exists)
                        throw new ApplicationException(
                            string.Format("Destination folder does not exist: {0}", sqlServerFileInfo.Directory.FullName));

                    fi.MoveTo(sqlServerFileInfo.FullName);

                    // insert into the SQL database
                    int nRowsInserted = DoBulkCopy();

                    if (nRowsImported != nRowsInserted)
                        throw new ApplicationException(
                            string.Format(
                                "Number of rows inserted ({0}) was not equal to number of rows in data file ({1}).",
                                nRowsInserted, nRowsImported));

                    // clean up our mess 
                    sqlServerFileInfo.Delete();
                }
                catch (Exception ex)
                {
                    status = Constants.InitializeInt;
                    statusMesasge = string.Format("FAIL. Control Number:{0}   Reason:{1}", ControlNumber, ex.Message);
                }

                if (UpdateControlStatus(status, statusMesasge) != 1)
                    throw new ApplicationException(
                        string.Format("Failed to update status for control number {0}. Status Message:{1}",
                                      ControlNumber, statusMesasge));
            }
            catch (SqlException sqlex)
            {
                throw new ApplicationException("SQL Failure:" + sqlex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("General Failure:" + ex.Message);
            }

            // reduce the number of details by getting rid of old or erronous runs
            if (status == 1)
                Purge();

            return status;
        }

        #endregion Public Methods

        private int DoBulkCopy()
        {
            var objPars = new SqlParameter[2];
            objPars[0] = ParameterHelper.GetIntegerPar("@controlNumber", ControlNumber, ParameterDirection.Input);
            objPars[1] =
                ParameterHelper.GetIntegerPar("@RowCount", Constants.InitializeInt, ParameterDirection.ReturnValue);
            try
            {
                DataAccess.ExecuteNonQueryStoredProc(_ebvConnectionString, "BulkInsert",
                                                     objPars);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 4834)
                    throw new ApplicationException("The user must have the bulkadmin fixed server role.\n" + ex.Message);
                throw;
            }
            return (int) objPars[1].Value;
        }

        private void Purge()
        {
            var objPars = new SqlParameter[1];
            objPars[0] = ParameterHelper.GetVarCharPar("@strain", Strain, 2, ParameterDirection.Input);
            DataAccess.ExecuteNonQueryStoredProc(_ebvConnectionString, "Purge", objPars);
        }

        private FileInfo CreateNewCsvFileInfo(string destinationFolder)
        {
            string fileName = ASREMLPrefix + ControlNumber + "_" + _strain + "_" + DateTime.Now.Year + "_" +
                              DateTime.Now.Month + "_" +
                              DateTime.Now.Day + "_" + DateTime.Now.Ticks;
            fileName = Path.Combine(destinationFolder, fileName);
            fileName = Path.ChangeExtension(fileName, "CSV");
            var fi = new FileInfo(fileName);
            if (fi.Exists) fi.Delete();
            return fi;
        }

        private static string SetNullValues(string inString)
        {
            var outString = inString.Contains(",NA") ? inString.Replace(",NA", ",") : inString;
            if (outString.Contains(",.")) outString = outString.Replace(",.", ",");
            return outString;
        }


/*        private void WriteOneDetailLine(string lineOfData, TextWriter bcpFile)
        {
            string tabsToCommasLine;

            //  - we want to store NULL where we get an "NA"
            if (lineOfData.Contains("\tNA"))
            {
                string noNaLine = lineOfData.Replace("\tNA", ",");
                tabsToCommasLine = string.Format("{0},{1}", ControlNumber, noNaLine.Replace('\t', ','));
            }
            else
            {
                tabsToCommasLine = string.Format("{0},{1}", ControlNumber, lineOfData.Replace('\t', ','));
            }
            bcpFile.WriteLine(tabsToCommasLine);
        }*/




        #region Control Table Mehtods

        private int InsertControlRow()
        {
            var objPars = new SqlParameter[3];
            objPars[0] = ParameterHelper.GetVarCharPar("@strain", Strain, 2, ParameterDirection.Input);
            objPars[1] =
                ParameterHelper.GetIntegerPar("@controlNumber", Constants.InitializeInt, ParameterDirection.Output);
            objPars[2] =
                ParameterHelper.GetIntegerPar("@RowCount", Constants.InitializeInt, ParameterDirection.ReturnValue);

            DataAccess.ExecuteNonQueryStoredProc(_ebvConnectionString,
                                                 "insertControlRow", objPars);

            var rowCount = (int) objPars[2].Value;
            if (rowCount != 1)
                throw new ApplicationException("Failed insert new control row into EBVControl.");

            return (int) objPars[1].Value;
        }

        private int UpdateControlStatus(int status, string statusMessage)
        {
            var objPars = new SqlParameter[4];
            objPars[0] = ParameterHelper.GetIntegerPar("@controlNumber", ControlNumber, ParameterDirection.Input);
            objPars[1] = ParameterHelper.GetIntegerPar("@status", status, ParameterDirection.Input);
            objPars[2] = ParameterHelper.GetVarCharPar("@statusMessage", statusMessage, 3000, ParameterDirection.Input);
            objPars[3] =
                ParameterHelper.GetIntegerPar("@RowCount", Constants.InitializeInt, ParameterDirection.ReturnValue);

            DataAccess.ExecuteNonQueryStoredProc(_ebvConnectionString,
                                                 "UpdateControlRowStatus", objPars);

            return (int) objPars[3].Value;
        }

        private void UpdateControlBulkInsertFileInfo(FileSystemInfo sqlServerFileInfo)
        {
            var objPars = new SqlParameter[3];
            objPars[0] = ParameterHelper.GetIntegerPar("@controlNumber", ControlNumber, ParameterDirection.Input);
            objPars[1] =
                ParameterHelper.GetVarCharPar("@BulkInsertFile", sqlServerFileInfo.FullName, 400,
                                              ParameterDirection.Input);
            objPars[2] =
                ParameterHelper.GetIntegerPar("@RowCount", Constants.InitializeInt, ParameterDirection.ReturnValue);
            DataAccess.ExecuteNonQueryStoredProc(_ebvConnectionString,
                                                 "UpdateControlBulkInsertFile", objPars);

            var rowCount = (int) objPars[2].Value;
            if (rowCount != 1)
                throw new ApplicationException(
                    string.Format("Failed to update Bulk Insert file name ({0}) into control record with key {1}",
                                  sqlServerFileInfo.FullName, ControlNumber));
        }

        #endregion Control Table Mehtods

        #region Helper Methods

        private string DiscoverStrain()
        {
            string strain = "??";
            try
            {
                using (StreamReader inEbvFile = _dataFile.OpenText())
                {
                    while (!inEbvFile.EndOfStream) 
                    {
                        // skip the header row...
                        inEbvFile.ReadLine();

                        string strainStringIn = inEbvFile.ReadLine();

                        if (strainStringIn != null)
                        {
                            string[] splitString = strainStringIn.Split(',');
                            //int rowNum = Int32.Parse(splitString[0]);
                            int calfSn = Int32.Parse(splitString[0]);
                            strain = LookupStrainCode(calfSn).ToUpper();
                            if ((strain == "M1") || (strain == "M2") || (strain == "M3") || (strain == "M4") || (strain == "TX"))
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                strain = "??";
            }
            return strain;
        }
        
        private string LookupStrainCode(int calfSn)
        {
            string strain;
            try
            {
                var objPars = new SqlParameter[2];
                objPars[0] = ParameterHelper.GetIntegerPar("@calfSN", calfSn, ParameterDirection.Input);
                objPars[1] =
                    ParameterHelper.GetVarCharPar("@strain", Constants.InitializeString, 2, ParameterDirection.Output);
                DataAccess.ExecuteNonQueryStoredProc(_ebvConnectionString,
                                                     "LookupCalfStrainCode", objPars);
                strain = (string) objPars[1].Value;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("Failed to find strain code for the selecte data file :{0}. Tried calf SN: {1}\n",
                                  DataFile.FullName, calfSn), ex);
            }
            return strain;
        }

        #endregion Helper Methods
    }
}