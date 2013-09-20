using System.Collections.Generic;
using System.ComponentModel;

namespace Beefbooster.DataAccessLibrary.Domain
{
    [DataObjectAttribute]
    public class StrainHelper
    {
        public StrainHelper() { }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<BBStrain> GetStrains()
        {
           return new List<BBStrain>
                              {
                                  new BBStrain("M1"),
                                  new BBStrain("M2"),
                                  new BBStrain("M3"),
                                  new BBStrain("M4"),
                                  new BBStrain("TX")
                              };

        }
    }
    public class BBBreeder
    {
        public BBBreeder() { }

        public BBBreeder(string accountNo, string contactName, string lastName, string city, string company)
        {
            _accountNo = accountNo;
            _contactName = contactName;
            _lastName = lastName;
            _city = city;
            _company = company;
        }

        private string _accountNo;
        public string AccountNo
        {
            get { return _accountNo; }
            set { _accountNo = value; }
        }

        private readonly string _contactName;
        public string ContactName
        {
            get { return _contactName; }
        }

        private readonly string _lastName;
        public string LastName
        {
            get { return _lastName; }
        }

        
        private readonly string _company;
        public string Company
        {
            get { return _company; }
        }


        private readonly string _city;
        public string City
        {
            get { return _city; }
        }

 
        public string Description
        {
            get { return _contactName + " : " + _company + " : " + _city; }
        }
    }
    public class BBHerd
    {
        public BBHerd() { }
        public BBHerd(int sn, string code, string strain, string breederName, string ranchName)
        {
            _sn = sn;
            _code = code;
            _strain = strain;
            _breederName = breederName;
            _ranchName = ranchName;
        }

        private readonly int _sn;
        public int SN
        {
            get { return _sn; }
        }

        private readonly string _code;
        public string Code
        {
            get { return _code; }
        }

        private readonly string _strain;
        public string Strain
        {
            get { return _strain; }
        }

        private readonly string _breederName;
        public string BreederName
        {
            get { return _breederName; }
        }

        private readonly string _ranchName;
        public string RanchName
        {
            get { return _ranchName; }
        }

        public string Description
        {
            get { return _code + " : " + _strain + " : " + _breederName; }
        }
    }
    public class BBYearLetter
    {
        public BBYearLetter(int year, string letter)
        {
            _year = year;
            _letter = letter;
        }
        private readonly int _year;
        public int YearNumber
        {
            get { return _year; }
        }

        private readonly string _letter;
        public string Letter
        {
            get { return _letter; }
        }

        public string YearNumberAndLetter
        {
            get { return _letter + " - " + _year; }
        }
    }
    public class BBStrain
    {
        private readonly string _strainCode;
        public BBStrain(string strainCode) { _strainCode = strainCode; }
        public string StrainCode { get { return _strainCode; } }
    }

}