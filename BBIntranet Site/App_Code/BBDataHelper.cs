using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Beefbooster.BusinessLogic;
using Beefbooster.DataAccessLibrary;
using Beefbooster.DataAccessLibrary.Domain;

namespace Beefbooster.Web
{
    [DataObject]
    public class BBDataHelper
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<BBStrain> GetStrains()
        {
            return new List<BBStrain>
                       {
                           new BBStrain("M1"),
                           new BBStrain("M2"),
                           new BBStrain("M3"),
                           new BBStrain("M4"),
                           new BBStrain("TX"),
                           new BBStrain("CR")
                       };
        }


        public BBHerd GetHerd(int herdSN)
        {
            IEnumerable<BBHerd> herdList = GetHerds(string.Empty);
            return herdList.FirstOrDefault(herd => herd.SN == herdSN);
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<BBYearLetter> GetCalfYearLetters(int startingYear)
        {
            var lst = new List<BBYearLetter>();

            HttpContext objContext = HttpContext.Current;
            if (objContext.Cache[CacheStaticValues.YearList] != null)
                return (List<BBYearLetter>) (HttpContext.Current.Cache[CacheStaticValues.YearList]);
            SqlParameter[] objPars = null;
            SqlDataReader objDataReader = null;
            // if it's not in cache, then create it
            try
            {
                string sqlString = "SELECT Yr_Num, Yr_Code from dbo.tblYearLetterCode WHERE ";
                if (startingYear != Constants.InitializeInt)
                {
                    objPars = new SqlParameter[1];
                    objPars[0] = ParameterHelper.GetIntegerPar("@startingYear", startingYear);
                    sqlString += " ((Yr_Num>=@startingYear) OR (@startingYear IS NULL)) AND ";
                }
                sqlString += " (Yr_Num <= YEAR(GETDATE())) ORDER BY Yr_Num";

                objDataReader = DataAccess.GetDataReader(WebConfigSettings.Configurations.CowCalf_ConnectionString,
                                                         sqlString, objPars, objDataReader);
                if (objDataReader.HasRows)
                {
                    int ordYrNum = objDataReader.GetOrdinal("Yr_Num");
                    int ordYrCode = objDataReader.GetOrdinal("Yr_Code");
                    while (objDataReader.Read())
                    {
                        int yrNum = objDataReader.GetInt16(ordYrNum);
                        string yrCode = objDataReader.GetString(ordYrCode);
                        var cy = new BBYearLetter(yrNum, yrCode);
                        lst.Add(cy);
                    }
                }
                if (!objDataReader.IsClosed)
                    objDataReader.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to read Beefbooster herd codes from the database", ex);
            }
            HttpContext.Current.Cache.Add(CacheStaticValues.YearList, lst, null,
                                          DateTime.Now.AddDays(Convert.ToInt32(1)), TimeSpan.Zero,
                                          CacheItemPriority.Normal, null);
            return lst;
        }

        private static IEnumerable<BBHerd> FilterBBHerdsByStrain(IEnumerable<BBHerd> herdList, string filterByStrain)
        {
            if (string.IsNullOrEmpty(filterByStrain))
                return herdList;

            var strainHerdList = new List<BBHerd>();

            foreach (BBHerd herd in herdList)
            {
                if (herd.Strain == filterByStrain)
                    strainHerdList.Add(herd);
            }
            return strainHerdList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static IEnumerable<BBHerd> GetHerds(string inStrain)
        {
            List<BBHerd> herdList;
            HttpContext objContext = HttpContext.Current;
            // if we have it in cache
            if (objContext.Cache[CacheStaticValues.HerdList] != null)
            {
                // return it
                herdList = (List<BBHerd>) (HttpContext.Current.Cache[CacheStaticValues.HerdList]);
                return FilterBBHerdsByStrain(herdList, inStrain);
            }


            // it its not in cache then we need to query the db
            //   build the list
            //   and store it in cache
            herdList = new List<BBHerd>();
            SqlDataReader objDataReader = null;
            string sqlString =
                "SELECT Herd_SN, Herd_Code, Strain_Code, Person_Name, GeneticHerd_Name FROM vwHerd_Strain_Person WHERE ";
            sqlString += " (Disabled_Flag=0) ORDER BY Herd_Code";
            try
            {
                objDataReader =
                    DataAccess.GetDataReader(WebConfigSettings.Configurations.CowCalf_ConnectionString,
                                             sqlString, null, objDataReader);
                if (objDataReader.HasRows)
                {
                    int ordHerd_SN = objDataReader.GetOrdinal("Herd_SN");
                    int ordHerd_Code = objDataReader.GetOrdinal("Herd_Code");
                    int ordStrainCode = objDataReader.GetOrdinal("Strain_Code");
                    int ordPerson_Name = objDataReader.GetOrdinal("Person_Name");
                    int ordGeneticHerd_Name = objDataReader.GetOrdinal("GeneticHerd_Name");
                    while (objDataReader.Read())
                    {
                        int sn = objDataReader.GetInt32(ordHerd_SN);
                        var code = (string) objDataReader.GetSqlString(ordHerd_Code);
                        var strain = (string) objDataReader.GetSqlString(ordStrainCode);
                        var breederName = (string) objDataReader.GetSqlString(ordPerson_Name);
                        var ranchName = (string) objDataReader.GetSqlString(ordGeneticHerd_Name);
                        var h = new BBHerd(sn, code, strain, breederName, ranchName);
                        herdList.Add(h);
                    }
                }
                if (!objDataReader.IsClosed)
                    objDataReader.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to read Beefbooster herd codes from the database", ex);
            }
            HttpContext.Current.Cache.Add(CacheStaticValues.HerdList, herdList, null,
                                          DateTime.Now.AddDays(Convert.ToInt32(1)), TimeSpan.Zero,
                                          CacheItemPriority.Normal, null);
            return FilterBBHerdsByStrain(herdList, inStrain);
        }









        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static IEnumerable<BBBreeder> GetBreeders()
        {
            List<BBBreeder> breederList;
            HttpContext objContext = HttpContext.Current;
            if (objContext.Cache[CacheStaticValues.BreederList] != null)
            {
                // return it
                breederList = (List<BBBreeder>)(HttpContext.Current.Cache[CacheStaticValues.BreederList]);
                return breederList;
            }


            // it its not in cache then we need to query the db
            //   build the list
            //   and store it in cache
            breederList = new List<BBBreeder>();
            const string sqlString = "SELECT ACCOUNTNO, COMPANY, CONTACT, LASTNAME, CITY FROM [CC2007].dbo.vwContacts WHERE KEY1 = 'CUST BREEDER' ORDER BY LASTNAME, CONTACT";
            try
            {
                SqlDataReader objDataReader = DataAccess.GetDataReader(WebConfigSettings.Configurations.CowCalf_ConnectionString, sqlString, null);
                if (objDataReader.HasRows)
                {
                    int ordAccountNo = objDataReader.GetOrdinal("ACCOUNTNO");
                    int ordCompany = objDataReader.GetOrdinal("COMPANY");
                    int ordContact = objDataReader.GetOrdinal("CONTACT");
                    int ordLastName = objDataReader.GetOrdinal("LASTNAME");
                    int ordCity = objDataReader.GetOrdinal("CITY");
                    while (objDataReader.Read())
                    {
                        string accountNo = (string)objDataReader.GetSqlString(ordAccountNo);
                        var contactName = (string)objDataReader.GetSqlString(ordContact);
                        var lastName = (string)objDataReader.GetSqlString(ordLastName);
                        var city = (string)objDataReader.GetSqlString(ordCity);
                        var company = (string)objDataReader.GetSqlString(ordCompany);
                        var breeder = new BBBreeder(accountNo, contactName, lastName, city, company);
                        breederList.Add(breeder);
                    }
                }
                if (!objDataReader.IsClosed)
                    objDataReader.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to read vwContacts from the CowCalf database", ex);
            }
            HttpContext.Current.Cache.Add(CacheStaticValues.HerdList, breederList, null,
                                          DateTime.Now.AddDays(Convert.ToInt32(1)), TimeSpan.Zero,
                                          CacheItemPriority.Normal, null);
            return breederList;
        }    
    
    
    
    
    
    
    }
}