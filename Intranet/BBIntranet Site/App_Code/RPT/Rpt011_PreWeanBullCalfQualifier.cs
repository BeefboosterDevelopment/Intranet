using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;


public class Rpt011_DataItem
{
    public Rpt011_DataItem(object tag, DateTime bdate, string adjBWT, string actBWT, string ebvBWT,string pulled, string damId, string ageOfDam, bool qualifies)
    {
        CalfTag = (string)tag;
        BirthDate = bdate;
        AdjBWT = adjBWT;
        ActBWT = actBWT;
        BWTEBV = ebvBWT;
        Pulled = pulled;
        DamId = damId;
        AgeOfDam = ageOfDam;
        Qualifies = qualifies;
    }

    #region Properties

    public string CalfTag { get; set; }
    public DateTime BirthDate { get; set; }
    public string AdjBWT { get; set; }
    public string ActBWT { get; set; }
    public string BWTEBV { get; set; }
    public string Pulled { get; set; }
    public string DamId { get; set; }
    public string AgeOfDam { get; set; }
    public bool Qualifies { get; set; }

    #endregion
}

    
    public class Rpt011_PreWeanBullCalfQualifier
    {
        // Make sure you have a default constructor or this won't show up as a valid datasource
        public Rpt011_PreWeanBullCalfQualifier(int herdSN, int yearBorn, int reportScope)
        {
            _herdSN = herdSN;
            _yearBorn = yearBorn;
            _reportScope = reportScope;
            SetDefaults();
        }


        public static readonly string[] ReportScopeValues = { "All Calves", "Only Rejected Calves", "Only Qualifying Calves" };


        #region Properties
        private string _reportScopeDescription;
        public string ReportScopeDescription
        {
            get { return _reportScopeDescription; }
        }

        private int _reportScope; // 0 means all calves;  1 means only rejects;  2 means only qualifiers
        public int ReportScope
        {
            get { return _reportScope; }
            set
            {
                _reportScope = value;
                _reportScopeDescription = ReportScopeValues[_reportScope];
            }
        }
     
        private string _ranchName;
        public string RanchName
        {
            get { return _ranchName; }
        }
        private string _strainCode;
        public string StrainCode
        {
            get { return _strainCode; }
        }

        private string _breederName;
        public string BreederName
        {
            get { return _breederName; }
        }
        private readonly int _herdSN;
/*
        public int HerdSN
        {
            get { return _herdSN; }
        }
*/

        private string _herdCode;
        public string HerdCode
        {
            get { return _herdCode; }
        }

        private readonly int _yearBorn;
        public int YearBorn
        {
            get { return _yearBorn; }
        }


        private int _maxBirthMonth;
        public int MaxBirthMonth
        {
            get { return _maxBirthMonth; }
            set { _maxBirthMonth = value; }
        }

        private int _maxBirthDay;
        public int MaxBirthDay
        {
            get { return _maxBirthDay; }
            set { _maxBirthDay = value; }
        }

        private DateTime _maxBirthDate;
        public DateTime MaxBirthDate
        {
            get { return _maxBirthDate; }
        }

        private int _inclPulled;
        public int IncludePulledCalves
        {
            get { return _inclPulled; }
            set { _inclPulled = value; }
        }

        private int _inclHeiferCalves;
        public int IncludeCalvesFromHeifers
        {
            get { return _inclHeiferCalves; }
            set { _inclHeiferCalves = value; }
        }

        private int _maxBWT;
        public int MaxBWT
        {
            get { return _maxBWT; }
            set { _maxBWT = value; }
        }

        private int _minBWT;
        public int MinBWT
        {
            get { return _minBWT; }
            set { _minBWT = value; }
        }

        #endregion

        #region Private methods
        private void SetDefaults()
        {
            _minBWT = 80;
            _maxBWT = 110;
            _maxBirthMonth = 5;
            _maxBirthDay = 15;
            _maxBirthDate = DateTime.Parse("15-MAY-" + _yearBorn);
            _inclPulled = 0;
            _inclHeiferCalves = 0;

            // go to SQL Server...
            string cnxnString = ConfigurationManager.ConnectionStrings["BullConnectionString"].ConnectionString;
            SqlConnection cnxn = new SqlConnection(cnxnString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[Rpt011_GetDefaults]";
            cmd.Parameters.Add(new SqlParameter("herdSN", _herdSN));
            cmd.Parameters.Add(new SqlParameter("birthYr", _yearBorn));
            cmd.Connection = cnxn;
            cnxn.Open();
            using (IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                // get the ordinal indexes into the read buffer                
                int ordHerdCode = rdr.GetOrdinal("HerdCode");
                int ordStrainCode = rdr.GetOrdinal("StrainCode");
                int ordBreederName = rdr.GetOrdinal("BreederName");
                int ordRanchName = rdr.GetOrdinal("RanchName");
                int ordMinBWT = rdr.GetOrdinal("MinBWT");
                int ordMaxBWT = rdr.GetOrdinal("MaxBWT");
                int ordMaxBirthDate = rdr.GetOrdinal("MaxBirthDate");
                int ordMaxBirthMonth = rdr.GetOrdinal("MaxBirthMonth");
                int ordMaxBirthDay = rdr.GetOrdinal("MaxBirthDay");
                int ordInclPulled = rdr.GetOrdinal("IncludePulled");
                int ordInclHeifer = rdr.GetOrdinal("IncludeHeiferCalves");

                if (rdr.Read())
                {
                    _herdCode = rdr.GetValue(ordHerdCode).ToString();
                    _strainCode = rdr.GetValue(ordStrainCode).ToString();
                    _breederName = rdr.GetValue(ordBreederName).ToString();
                    _ranchName = rdr.GetValue(ordRanchName).ToString();
                    _minBWT = (Int32)rdr.GetValue(ordMinBWT);
                    _maxBWT = (Int32)rdr.GetValue(ordMaxBWT);
                    _maxBirthDate = (DateTime)rdr.GetValue(ordMaxBirthDate);
                    _maxBirthMonth = (Int32)rdr.GetValue(ordMaxBirthMonth);
                    _maxBirthDay = (Int32)rdr.GetValue(ordMaxBirthDay);
                    _inclPulled = (int)rdr.GetValue(ordInclPulled);
                    _inclHeiferCalves = (int)rdr.GetValue(ordInclHeifer);
                }
            }
        }

        private static string SafeGet(IDataRecord rdr, int ord, DbType dataType, string defaultValue)
        {
            if (rdr.IsDBNull(ord))
                return defaultValue;

            string retVal;
            switch (dataType)
            {
                case DbType.Int32:
                    Int32 iVal = (Int32)rdr.GetValue(ord);
                    retVal = iVal.ToString(CultureInfo.InvariantCulture);
                    break;

                case DbType.String:
                    string sVal = rdr.GetValue(ord).ToString();
                    retVal = sVal;
                    break;

                case DbType.DateTime:
                    DateTime dtVal = (DateTime)rdr.GetValue(ord);
                    retVal = dtVal.ToShortDateString();
                    break;

                case DbType.Int16:
                    Int16 i16Val = (Int16)rdr.GetValue(ord);
                    retVal = i16Val.ToString(CultureInfo.InvariantCulture);
                    break;

                case DbType.Decimal:
                    var dVal = rdr.GetDecimal(ord);
                    retVal = dVal.ToString(CultureInfo.InvariantCulture);
                    break;

                default:
                    retVal = rdr.GetValue(ord).ToString();
                    break;
            }

            return retVal;
        }
        #endregion

        public IEnumerable<Rpt011_DataItem> GetData()
        {
            List<Rpt011_DataItem> lst = new List<Rpt011_DataItem>();
            string cnxnString = ConfigurationManager.ConnectionStrings["BullConnectionString"].ConnectionString;
            SqlConnection cnxn = new SqlConnection(cnxnString);

            SqlCommand cmd = new SqlCommand
                                 {
                                     CommandType = CommandType.StoredProcedure,
                                     CommandText = "[Rpt011_Calving_PreliminaryBullCalfQualifier]",
                                     Connection = cnxn
                                 };


            cmd.Parameters.Add(new SqlParameter("herdSN", _herdSN));
            cmd.Parameters.Add(new SqlParameter("birthYr", _yearBorn));
            cmd.Parameters.Add(new SqlParameter("pulled", _inclPulled));
            cmd.Parameters.Add(new SqlParameter("minBWT", _minBWT));
            cmd.Parameters.Add(new SqlParameter("maxBWT", _maxBWT));
            cmd.Parameters.Add(new SqlParameter("maxBDate", _maxBirthDate));
            cmd.Parameters.Add(new SqlParameter("calvesFromHeifers", _inclHeiferCalves));

            cnxn.Open();



            using (IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                // get the ordinal indexes into the read buffer                
                int ordCalfTag = rdr.GetOrdinal("CalfTag_Num");
                int ordBirth_Date = rdr.GetOrdinal("Birth_Date");
                int ordBirthAdj_Wt = rdr.GetOrdinal("BW_ADJ");
                int ordBirth_Wt = rdr.GetOrdinal("Birth_Wt");
                int ordBWTEBV = rdr.GetOrdinal("BW_EBV");
                int ordAssistedBirth_Flag = rdr.GetOrdinal("AssistedBirth_Flag");
                int ordDamTag_Str = rdr.GetOrdinal("DamTag_Str");
                int ordDamYr_Code = rdr.GetOrdinal("DamYr_Code");
                int ordAgeOfDam = rdr.GetOrdinal("AgeOfDam");
                int ordQualifies = rdr.GetOrdinal("qualifies");

                while (rdr.Read())
                {
                    bool calfQualifies = ((Int32)rdr.GetValue(ordQualifies) == 1);

                    bool addIt;
                    switch (_reportScope)
                    {
                        case 0: addIt = true; break;
                        case 1: addIt = !calfQualifies; break;
                        case 2: addIt = calfQualifies; break;
                        default: addIt = true; break;
                    }

                    if (addIt)
                    {
                        lst.Add(
                            new Rpt011_DataItem(
                                                SafeGet(rdr, ordCalfTag, DbType.Int32, string.Empty),
                                                (DateTime)rdr.GetValue(ordBirth_Date),
                                                SafeGet(rdr, ordBirthAdj_Wt, DbType.Int16, "n/a"),
                                                SafeGet(rdr, ordBirth_Wt, DbType.Int16, string.Empty),
                                                SafeGet(rdr, ordBWTEBV, DbType.Decimal, string.Empty),
                                                SafeGet(rdr, ordAssistedBirth_Flag, DbType.Boolean, string.Empty),
                                                SafeGet(rdr, ordDamTag_Str, DbType.String, string.Empty) + " " +
                                                SafeGet(rdr, ordDamYr_Code, DbType.String, string.Empty),
                                                SafeGet(rdr, ordAgeOfDam, DbType.String, string.Empty),
                                                calfQualifies
                                            )
                        );
                    }
                }
            }
            return lst;
        }
    }

