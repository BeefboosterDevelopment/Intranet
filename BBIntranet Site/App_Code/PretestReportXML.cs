using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Xml;
using Beefbooster.DataAccessLibrary;
using Beefbooster.Web;

namespace Beefbooster.BusinessLogic
{
    #region internal class ReportStrainData
    /// <summary>
    /// This class holds 1 rows worth of data per strain
    /// We populate it straight from the database
    /// Then use it to build the XML suitable for our web page to transform
    /// </summary>
    internal class ReportStrainData
    {
        private readonly string _strain;
        private readonly int _numontest;
        private readonly DateTime _birthDate;
        private readonly int _bwt;
        private readonly int _wwt;
        private readonly int _ontestwt;
        private readonly int _weanage;
        private readonly decimal _adgbw;
        private readonly decimal _wwpda;

        public ReportStrainData(    string strain, 
                                    int numontest,
                                    DateTime birthDate,
                                    int bwt,
                                    int wwt,
                                    int weanage,
                                    int ontestwt,
                                    decimal adgbw,
                                    decimal wwpda
                                )
        {
             _strain = strain;
             _numontest = numontest;
             _birthDate = birthDate;
             _bwt = bwt;
             _wwt = wwt;
             _ontestwt = ontestwt;
             _weanage = weanage;
             _adgbw = adgbw;
             _wwpda = wwpda;
        }

        public string Strain { get { return _strain; } }
        public DateTime BirthDate { get { return _birthDate; } }
        public int NumOnTest { get { return _numontest; } }
        public int BirthWt { get { return _bwt; } }
        public int WeanWt { get { return _wwt; } }
        public int WeanAge { get { return _weanage; } }
        public int OnTestWt { get { return _ontestwt; } }
        public decimal ADGBW { get { return _adgbw; } }
        public decimal WWPDA { get { return _wwpda; } }
    }
    #endregion

    /// <summary>
    /// Summary description for PretestReportXML
    /// </summary>
    public class PretestReportXML
    {
        private readonly string _fileName;
        private readonly int _birthYear;

        private const string ELNAME_ROOT = "PreTestStrainData";
        private const string ELNAME_STRAIN = "STRAIN";
        private const string ATTRNAME_GENERATEDON = "GeneratedOn";
        private const string ATTRNAME_YEAR = "BornInYear";
        private const string ATTRNAME_RPTYEAR = "ReportYear";

        private const string ATTRNAME_STRAINCODE = "CODE";
        private const string ATTRNAME_NUMONTEST = "NUMONTEST";
        private const string ATTRNAME_BIRTHDATE = "BDATE";
        private const string ATTRNAME_BIRTHWT = "BWT";
        private const string ATTRNAME_WEANWT = "WWT";
        private const string ATTRNAME_ADGBW = "ADGBW";
        private const string ATTRNAME_WWPDA = "WWPDA";
        private const string ATTRNAME_WEANAGE = "WEANAGE";
        private const string ATTRNAME_ONTESTWT = "ONTESTWT";

        public PretestReportXML(string fileName)
        {
            _fileName = mappedXMLFileName(fileName);
            _birthYear = DateTime.Now.Year;
            if (DateTime.Now.Month <= 6) _birthYear--;

        }
        public bool CheckIntegrity()
        {
            bool returnValue;// = false;
            //try
            //{
                XmlReaderSettings xs = new XmlReaderSettings();
                xs.CheckCharacters = true;
                using (XmlReader rdr = XmlReader.Create(_fileName, xs))
                {
                    returnValue = rdr.CanResolveEntity;
                }
            //}
            //catch (System.IO.FileNotFoundException fnfEx)
            //{
            //    string msg = fnfEx.Message;
            //}
            //catch (Exception ex)
            //{
            //    string m = ex.Message;
            //}
            return returnValue;

        }
        private static string getElValue_int(int val)
        {
            if (val == Constants.InitializeInt)
                return "No Data";
            return val.ToString();
        }
        private static string mappedXMLFileName(string filename)
        {
            string newPathFile = string.Empty;
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                newPathFile = context.Server.MapPath(@"~\App_Data\XML\" + filename);
            }
            return newPathFile;
        }

        public void Refresh()
        {
            int testStationYear = _birthYear + 1;
            string errorMessage = string.Empty;
            List<ReportStrainData> lst = null;
            try
            {
                // get the data from the database into list of objects
                lst = getStrainList();
            }
            catch (ApplicationException appEx)
            {
                errorMessage = appEx.Message;
            }
            if (lst == null)
                return;

            // create the XML document
            XmlDocument doc = new XmlDocument();

            // init the root node
            XmlElement rootEl = (XmlElement)doc.AppendChild(doc.CreateElement(ELNAME_ROOT));
            rootEl.SetAttribute(ATTRNAME_GENERATEDON, DateTime.Now.ToString("dd MMM yyyy"));
            rootEl.SetAttribute(ATTRNAME_YEAR, _birthYear.ToString());
            rootEl.SetAttribute(ATTRNAME_RPTYEAR, _birthYear + "-" + testStationYear.ToString().Substring(2));
            if (!string.IsNullOrEmpty(errorMessage))
            {
                rootEl.SetAttribute("ERROR", errorMessage);
            }
            else
            {
                // create and load the strain elements
                foreach (ReportStrainData data in lst)
                {
                    XmlElement strainEl = (XmlElement)rootEl.AppendChild(doc.CreateElement(ELNAME_STRAIN));
                    strainEl.SetAttribute(ATTRNAME_STRAINCODE, data.Strain);

                    strainEl.SetAttribute(ATTRNAME_NUMONTEST, getElValue_int(data.NumOnTest));
                    strainEl.SetAttribute(ATTRNAME_BIRTHDATE, data.BirthDate.ToString("MMM d yyyy"));
                    strainEl.SetAttribute(ATTRNAME_BIRTHWT, getElValue_int(data.BirthWt));
                    strainEl.SetAttribute(ATTRNAME_WEANWT, getElValue_int(data.WeanWt));
                    strainEl.SetAttribute(ATTRNAME_ADGBW, data.ADGBW.ToString());
                    strainEl.SetAttribute(ATTRNAME_WWPDA, data.WWPDA.ToString());
                    strainEl.SetAttribute(ATTRNAME_WEANAGE, getElValue_int(data.WeanAge));
                    strainEl.SetAttribute(ATTRNAME_ONTESTWT, getElValue_int(data.OnTestWt));
                }
            }

            // save it
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.CheckCharacters = true;
            ws.CloseOutput = true;
            ws.OmitXmlDeclaration = false;
            ws.NewLineHandling = NewLineHandling.Replace;
            ws.NewLineChars = @"\n";
            using (XmlWriter xw = XmlWriter.Create(_fileName, ws))
            {
                doc.Save(xw);
                xw.Flush();
                xw.Close();
            }
        }

        private List<ReportStrainData> getStrainList()
        {
            List<ReportStrainData> lst = new List<ReportStrainData>();
            SqlDataReader objDataReader;
            SqlParameter[] objPars;
            objPars = new SqlParameter[1];
            objPars[0] = ParameterHelper.GetIntegerPar("@birthYr", _birthYear, ParameterDirection.Input);
            
            // execute the procedure or query
    		try
    		{
                objDataReader = DataAccess.GetDataReaderStoredProc(WebConfigSettings.Configurations.Bull_ConnectionString,
                        "web_PretestReport", objPars, null);

                if (objDataReader.HasRows)
                {
                    int ordStrain_Code = objDataReader.GetOrdinal("STRAIN");
                    int ordNumOnTest = objDataReader.GetOrdinal("NUMONTEST");
                    int ordBDate = objDataReader.GetOrdinal("BDATE");
                    int ordBWT = objDataReader.GetOrdinal("BWT");
                    int ordWWT = objDataReader.GetOrdinal("WWT");
                    int ordWeanAge = objDataReader.GetOrdinal("WEANAGE");
                    int ordOntestWt = objDataReader.GetOrdinal("ONTESTWT");
                    int ordADGBW = objDataReader.GetOrdinal("ADGBW");
                    int ordWWPDA = objDataReader.GetOrdinal("WWPDA");

                    while (objDataReader.Read())
                    {
                        string strainCode = objDataReader.GetString(ordStrain_Code);
                        int numontest = objDataReader.GetInt32(ordNumOnTest);
                        DateTime bdate = DataAccess.SafeGetDateTime(objDataReader, ordBDate, Constants.InitializeDateTime);
                        int bwt = DataAccess.SafeGetInt32(objDataReader, ordBWT, 0);
                        int wwt = DataAccess.SafeGetInt32(objDataReader, ordWWT, 0);
                        int weanage = DataAccess.SafeGetInt32(objDataReader, ordWeanAge, 0);
                        int ontestwt = DataAccess.SafeGetInt32(objDataReader, ordOntestWt, 0);
                        decimal adgbw = DataAccess.SafeGetDecimal(objDataReader, ordADGBW, 0);
                        decimal wwpda = DataAccess.SafeGetDecimal(objDataReader, ordWWPDA, 0);
                        lst.Add(new ReportStrainData(strainCode, numontest, bdate, bwt, wwt, weanage, ontestwt, adgbw, wwpda));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to execute query", ex);
            }
            return lst;
    	}

    }
}
