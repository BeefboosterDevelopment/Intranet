using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Beefbooster.BusinessLogic;
using Beefbooster.DataAccessLibrary;
using Beefbooster.Web;

#region class Rpt301_DataItem

/// <summary>
/// This class holds 1 bull
/// </summary>
public class Rpt301_DataItem
{
    // Make sure you have a default constructor or this won't show up as a valid datasource
    private readonly string _calfId;
    private readonly int _calfSN;
    private readonly string _colourCode;
    private readonly int _feedId;
    private readonly string _herdCode;
    private readonly string _pen;
    private readonly string _strainCode;
    private readonly int _tagNumber;
    private DateTime _birthDate;

    public Rpt301_DataItem(string strainCode,
                           string herdCode,
                           int calfSN,
                           string calfId,
                           int tagNumber,
                           int feedId,
                           DateTime birthDate,
                           string pen,
                           string colourCode
        )
    {
        _strainCode = strainCode;
        _herdCode = herdCode;
        _calfSN = calfSN;
        _calfId = calfId;
        _tagNumber = tagNumber;
        _feedId = feedId;
        _birthDate = birthDate;
        _pen = pen;
        _colourCode = colourCode;
    }

    public int CalfSN
    {
        get { return _calfSN; }
    }

    public string StrainCode
    {
        get { return _strainCode; }
    }

    public string HerdCode
    {
        get { return _herdCode; }
    }

    public string CalfId
    {
        get { return _calfId; }
    }

    public int TagNumber
    {
        get { return _tagNumber; }
    }

    public int FeedId
    {
        get { return _feedId; }
    }

    public DateTime BirthDate
    {
        get { return _birthDate; }
    }

    public string DOB
    {
        get { return _birthDate.ToString("MMM dd"); }
    }

    public string Pen
    {
        get { return _pen; }
    }

    public string ColourCode
    {
        get { return _colourCode; }
    }
}

#endregion

public class Rpt301_DataObject
{
    public Rpt301_DataObject(int bornInYear, string strainCode, string reportStyle)
    {
        _yearBorn = bornInYear;
        _strainCode = strainCode;
        _reportStyle = reportStyle;
    }

    #region Properties

    private readonly string _reportStyle = string.Empty;
    private readonly string _strainCode;

    private readonly int _yearBorn;

    public string StrainCode
    {
        get { return _strainCode; }
    }

    public int YearBorn
    {
        get { return _yearBorn; }
    }

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

    public IEnumerable<Rpt301_DataItem> GetData()
    {
        var lst = new List<Rpt301_DataItem>();

        var procParams = new SqlParameter[2];
        procParams[0] = ParameterHelper.GetVarCharPar("@strain", StrainCode, 2, ParameterDirection.Input);
        procParams[1] = ParameterHelper.GetIntegerPar("@birthYr", YearBorn, ParameterDirection.Input);

        var objCommand = new SqlCommand
                             {CommandText = "Rpt301_Bull_WorksheetReport", CommandType = CommandType.StoredProcedure};

        using (var objConnection = new SqlConnection(WebConfigSettings.Configurations.Bull_ConnectionString))
        {
            objCommand.Connection = objConnection;
            foreach (SqlParameter objPar in procParams)
                objCommand.Parameters.Add(objPar);
            objCommand.CommandTimeout = 60;
            objConnection.Open();

            SqlDataReader rdr = objCommand.ExecuteReader();
            if (rdr.HasRows)
            {
                // get the ordinal indexes into the read buffer                
                int ordSTRAIN = rdr.GetOrdinal("STRAIN");
                int ordHerdcode = rdr.GetOrdinal("HERDCODE");
                int ordCalfSn = rdr.GetOrdinal("CALFSN");
                int ordCalfid = rdr.GetOrdinal("CALFID");
                int ordTagnumber = rdr.GetOrdinal("TAGNUMBER");
                int ordFeedid = rdr.GetOrdinal("FEEDID");
                int ordBdate = rdr.GetOrdinal("BDATE");
                int ordPen = rdr.GetOrdinal("PEN");
                int ordColour = rdr.GetOrdinal("COLOUR");

                while (rdr.Read())
                {
                    var strainCode =
                        ((string)
                         ParameterUtils.SafeGetValue(rdr.GetValue(ordSTRAIN), typeof (string),
                                                     Constants.InitializeString));
                    var herdCode =
                        ((string)
                         ParameterUtils.SafeGetValue(rdr.GetValue(ordHerdcode), typeof (string),
                                                     Constants.InitializeString));
                    var calfSN =
                        ((int)
                         ParameterUtils.SafeGetValue(rdr.GetValue(ordCalfSn), typeof (int), Constants.InitializeInt));
                    var calfId =
                        ((string)
                         ParameterUtils.SafeGetValue(rdr.GetValue(ordCalfid), typeof (string),
                                                     Constants.InitializeString));
                    var tagNumber =
                        ((int)
                         ParameterUtils.SafeGetValue(rdr.GetValue(ordTagnumber), typeof (int), Constants.InitializeInt));
                    var feedId =
                        ((int)
                         ParameterUtils.SafeGetValue(rdr.GetValue(ordFeedid), typeof (int), Constants.InitializeInt));
                    var birthDate =
                        ((DateTime)
                         ParameterUtils.SafeGetValue(rdr.GetValue(ordBdate), typeof (DateTime),
                                                     Constants.InitializeDateTime));
                    var pen =
                        ((string)
                         ParameterUtils.SafeGetValue(rdr.GetValue(ordPen), typeof (string), Constants.InitializeString));
                    var colourCode =
                        ((string)
                         ParameterUtils.SafeGetValue(rdr.GetValue(ordColour), typeof (string),
                                                     Constants.InitializeString));

                    lst.Add(new Rpt301_DataItem(strainCode,
                                                herdCode,
                                                calfSN,
                                                calfId,
                                                tagNumber,
                                                feedId,
                                                birthDate,
                                                pen,
                                                colourCode));
                }
            }
            return lst;
        }
    }
}
