using System;
using System.Collections.Generic;
using System.IO;

namespace Beefbooster.ASREML
{
    /// <summary>
    /// This class holds 1 bull
    /// </summary>
    /// 
    public class ASREMLCalf
    {
        private const char TabChar = '\t';
        private Int16 _ageOfDam;
        private Int16 _backFat;
        private Int16 _backFatAge;


        // Make sure you have a default constructor 
        private DateTime _birthDate;
        private Int16 _birthWt;
        private int _calfSn;
        private Int16 _damAgeGroup;
        private Int16 _damBirthYear;
        private int _damCalfSn;
        private int _damHerdId; // this is the ASREML Herd Id
        private int _damSn;
        private List<ASREMLDamWeight> _damWts;
        private Int16 _daysOnTest;
        private Int16 _heifer18MonthWt;
        private Int16 _heifer18MonthWtAge;
        private Int16 _heiferYearlingWt;
        private Int16 _heiferYearlingWtAge;
        private int _herdId; // ASREML Herd Id
        private bool _isTwin;
        private Int16 _lastTestWt;
        private Int16 _lastTestWtAge;
        private Int16 _onTestWt;

        private decimal _rfi;
        private Int16 _rfiAge;
        private decimal _rfiFat;
        private decimal _scrotalCircum;
        private Int16 _scrotalCircumAge;
        private char _sexCode;
        private int _sireSn;
        private string _strainCode;
        private Int16 _weanAge;
        private DateTime _weanDate;
        private Int16 _weanWt;

        public ASREMLCalf()
        {
        }

        public ASREMLCalf(string strainCode,
                          int calfSn,
                          int damSn,
                          int herdId,
                          int sireSn,
                          DateTime birthDate,
                          Int16 birthWt,
                          char sexCode,
                          bool isTwin
            )
        {
            _strainCode = strainCode;
            _calfSn = calfSn;
            _damSn = damSn;
            _herdId = herdId;
            _sireSn = sireSn;
            _birthDate = birthDate;
            _birthWt = birthWt;
            _sexCode = sexCode;
            _isTwin = isTwin;
        }

        // Calf Properties
        public string StrainCode
        {
            get { return _strainCode; }
            set { _strainCode = value; }
        }

        public int CalfSn
        {
            get { return _calfSn; }
            set { _calfSn = value; }
        }

        public int DamSn
        {
            get { return _damSn; }
            set { _damSn = value; }
        }

        public int HerdId
        {
            get { return _herdId; }
            set { _herdId = value; }
        }

        public int SireSn
        {
            set { _sireSn = value; }
        }

        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; }
        }

        public Int16 BirthWt
        {
            //get { return _birthWt; }
            set { _birthWt = value; }
        }

        public char SexCode
        {
            set { _sexCode = value; }
        }

        public string ContemporaryGroupCode { get; set; }

        private Int16 ASREMLSexCode
        {
            get
            {
                switch (_sexCode)
                {
                    case 'M':
                        return 1;
                    case 'F':
                        return 2;
                    case 'S':
                        return 3;
                    default:
                        return 1;
                }
            }
        }

        public bool IsTwin
        {
            set { _isTwin = value; }
        }

        public DateTime WeanDate
        {
            set { _weanDate = value; }
        }

        public Int16 WeanWt
        {
            set { _weanWt = value; }
        }

        public Int16 WeanAge
        {
            set { _weanAge = value; }
        }

        // Heifer Yearling Wt Properties
        public Int16 HeiferYearlingWtAge
        {
            get { return _heiferYearlingWtAge; }
            set { _heiferYearlingWtAge = value; }
        }

        public Int16 HeiferYearlingWt
        {
            set { _heiferYearlingWt = value; }
        }

        public Int16 Heifer18MonthWtAge
        {
            get { return _heifer18MonthWtAge; }
            set { _heifer18MonthWtAge = value; }
        }

        public Int16 Heifer18MonthWt
        {
            set { _heifer18MonthWt = value; }
        }

        // Dam Properties
        public int DamCalfSn
        {
            get { return _damCalfSn; }
            set { _damCalfSn = value; }
        }

        public int DamHerdId
        {
            set { _damHerdId = value; }
        }

        public Int16 DamBirthYear
        {
            get { return _damBirthYear; }
            set { _damBirthYear = value; }
        }

        public Int16 AgeOfDam
        {
            set { _ageOfDam = value; }
        }

        public Int16 DamAgeGroup
        {
            set { _damAgeGroup = value; }
        }

        public List<ASREMLDamWeight> DamWts
        {
            set { _damWts = value; }
        }

        // Bull Properties
        public Int16 OnTestWt
        {
            get { return _onTestWt; }
            set { _onTestWt = value; }
        }

        public Int16 LastTestWt
        {
            set { _lastTestWt = value; }
        }

        public Int16 LastTestWtAge
        {
            get { return _lastTestWtAge; }
            set { _lastTestWtAge = value; }
        }

        public Int16 DaysOnTest
        {
            get { return _daysOnTest; }
            set { _daysOnTest = value; }
        }

        public Int16 BullBackFat
        {
            set { _backFat = value; }
        }

        public Int16 BullBackFatAge
        {
            get { return _backFatAge; }
            set { _backFatAge = value; }
        }

        public decimal ScrotalCircum
        {
            set { _scrotalCircum = value; }
        }

        public Int16 ScrotalCircumAge
        {
            get { return _scrotalCircumAge; }
            set { _scrotalCircumAge = value; }
        }

        public decimal Rfi
        {
            set { _rfi = value; }
        }

        public decimal RfiFat
        {
            set { _rfiFat = value; }
        }

        public Int16 RfiAge
        {
            set { _rfiAge = value; }
        }


        public string DataRow()
        {
            var sw = new StringWriter();
            sw.Write(_strainCode + TabChar); // 1
            sw.Write(_calfSn.ToString() + TabChar); // 2
            writeNaOrInt32(sw, _damCalfSn, true); // 3
            writeNaOrInt32(sw, _sireSn, true); // 4
            writeNaOrShort(sw, ASREMLSexCode, true); // 5
            writeNaOrInt32(sw, _herdId, true); // 6
            sw.Write(_isTwin ? "2\t" : "1\t"); // 7
            sw.Write(_birthDate.ToString("dd-MM-yyyy") + TabChar); // 8
            writeNaOrInt32(sw, _birthDate.Day, true); // 9
            writeNaOrInt32(sw, _birthDate.Month, true); // 10
            writeNaOrInt32(sw, _birthDate.Year, true); // 11
            writeNaOrShort(sw, _birthWt, true); // 12

            if ((_weanDate.Year > 1900) && (_weanDate.Year < 9999)) // date will be MAX DATE if null
            {
                sw.Write(_weanDate.ToString("dd-MM-yyyy") + TabChar); // 13
                writeNaOrInt32(sw, _weanDate.Day, true); // 14
                writeNaOrInt32(sw, _weanDate.Month, true); // 15
                writeNaOrInt32(sw, _weanDate.Year, true); // 16
            }
            else
                sw.Write("NA\tNA\tNA\tNA\t"); // 13 14 15 16

            writeNaOrShort(sw, _weanAge, true); // 17
            writeNaOrShort(sw, _weanWt, true); // 18
            writeNaOrShort(sw, _ageOfDam, true); // 19
            writeNaOrShort(sw, _damAgeGroup, true); // 20
            writeNaOrShort(sw, _damBirthYear, true); // 21
            writeNaOrInt32(sw, _damSn, true); // 22
            writeNaOrInt32(sw, _damHerdId, true); // 23


            if (_heiferYearlingWtAge > 0)
            {
                writeNaOrShort(sw, _heiferYearlingWtAge, true); // 24
                writeNaOrShort(sw, _heiferYearlingWt, true); // 25
            }
            else
            {
                sw.Write("NA\tNA\t"); // 24 25
            }

            if (_heifer18MonthWtAge > 0)
            {
                writeNaOrShort(sw, _heifer18MonthWtAge, true); // 26
                writeNaOrShort(sw, _heifer18MonthWt, true); // 27
            }
            else
            {
                sw.Write("NA\tNA\t"); // 26 27
            }


            // at the end of it we want 4 sets of wt/age pairs
            string damWeightString = String.Empty;

            int wtCount = 0;
            if (_damWts != null)
            {
                foreach (ASREMLDamWeight damWt in _damWts)
                {
                    damWeightString += string.Format("{0}\t{1}\t", damWt.Wt, damWt.Age);
                    wtCount++;
                    if (wtCount >= 4) break;
                }
            }

            // always want exactly 4 weights and ages 
            for (int iw = wtCount; iw < 4; iw++)
                damWeightString += "NA\tNA\t";

            sw.Write(damWeightString); // 28 29 // 30 31 // 32 33 // 34 35 

            writeNaOrShort(sw, _onTestWt, true); // 36
            writeNaOrShort(sw, _lastTestWt, true); // 37
            writeNaOrShort(sw, _lastTestWtAge, true); // 38

            writeNaOrShort(sw, _daysOnTest, true); // 39
            writeNaOrShort(sw, _backFat, true); // 40
            writeNaOrShort(sw, _backFatAge, true); // 41
            writeNaOrDecimal(sw, _scrotalCircum, true); // 42
            writeNaOrShort(sw, _scrotalCircumAge, true); // 43

            /*
                March 2013 put the RFI stuff back in as ordered by John Crowley
             */
            writeNaOrDecimal(sw, _rfi, true); // 44
            writeNaOrDecimal(sw, _rfiFat, true); // 45
            writeNaOrShort(sw, _rfiAge, true); // 46

            /*
             * 
             *  April 12 2011    Steve Charlton
             *  -------------    --------------
             *  At this point the maternal strains do not have any RFI observations
             *  Only TX has it.
             * 
             *  Only bulls have RFI taken.
             * 
             *  The ASREML software has been set up to calculate RFI now 
             *  In order to save putting in strain specific branches
             *  We will generate random data for it to calculate with.  
             *  Then on the import  we will just throw it away.
             * 
             *
             * Note:  the resulting RFI EBV's are nulled out in this stored procedure: 
             *      PROCEDURE BBSQL.EBV.dbo.Purge @strain char(2)
             *      
             *          right at the bottom of the procedure dated Apr 12 2011
             *          
             *          make sure you delete this chunk SQL if you ever want
             *          to keep RFIs for maternal strains.
             * 
             * 
             */
/*
            if ((_sexCode == 'M') && (_strainCode[0] == 'M'))
            {
                // how to calculate random number between max and min...
                // randomNumber = random.NextDouble() * (maximum - minimum) + minimum;  
                // for RFI : minimum is around -1.4  maximum is around 1.4
                var random = new Random();
                decimal rndRfi = Convert.ToDecimal(random.NextDouble()*(1.4 + 1.4) - 1.4);
                decimal rndRfiFat = Convert.ToDecimal(random.NextDouble()*(1.3 + 1.3) - 1.3);
                short rndDays = Convert.ToInt16(random.NextDouble()*(280 - 220) + 220);
                writeNaOrDecimal(sw, rndRfi, true); // 44
                writeNaOrDecimal(sw, rndRfiFat, true); // 45
                writeNaOrShort(sw, rndDays, true); // 46
            }
            else
            {
                writeNaOrDecimal(sw, _rfi, true); // 44
                writeNaOrDecimal(sw, _rfiFat, true); // 45
                writeNaOrShort(sw, _rfiAge, true); // 46
            }
*/

            sw.Write(string.IsNullOrEmpty(ContemporaryGroupCode) ? "NA" : ContemporaryGroupCode);

            return sw.ToString();
        }

/*        private decimal GetRandomNumber(double minimum, double maximum)
        {
            var random = new Random();
            var randomNumber = random.NextDouble()*(maximum - minimum) + minimum;
            return Convert.ToDecimal(randomNumber);
        }

        private short GetRandomNumber(int minimum, int maximum)
        {
            var random = new Random();
            var randomNumber = random.NextDouble() * (maximum - minimum) + minimum;
            return Convert.ToInt16(randomNumber);
        }
 */

        private void writeNaOrDecimal(StringWriter sw, Decimal val, bool addSeparator)
        {
            sw.Write(val == Decimal.MaxValue ? "NA" : val.ToString());
            if (addSeparator) sw.Write(TabChar);
        }

        //public void WriteNaOrDateTime(StringWriter sw, DateTime val, bool addSeparator)
        //{
        //    sw.Write(val == DateTime.MaxValue ? "NA" : val.ToString());
        //    if (addSeparator) sw.Write(TAB_CHAR);
        //}
        //public void WriteNaOrTinyInt(StringWriter sw, byte val)
        //{
        //    sw.Write(val == byte.MaxValue ? "NA" : val.ToString());
        //}

        private void writeNaOrInt32(StringWriter sw, int val, bool addSeparator)
        {
            sw.Write(val == int.MaxValue ? "NA" : val.ToString());
            if (addSeparator) sw.Write(TabChar);
        }

        private void writeNaOrShort(StringWriter sw, short val, bool addSeparator)
        {
            sw.Write(val == short.MaxValue ? "NA" : val.ToString());
            if (addSeparator) sw.Write(TabChar);
        }

        //public void writeNaOrDouble(StringWriter sw, double val)
        //{
        //    sw.Write(val == double.MaxValue ? "NA" : val.ToString());
        //}
    }
}