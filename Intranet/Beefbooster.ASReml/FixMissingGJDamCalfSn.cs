using System;
using System.Collections.Generic;
using System.Linq;

namespace Beefbooster.ASREML
{
    public class FixMissingGJDamCalfSn
    {
        private readonly int _calfBirthYear;
        private readonly int _calfHerdId;
        private readonly IEnumerable<ASREMLCalf> _originalList;
        private readonly string _strain;

        public FixMissingGJDamCalfSn(int calfBirthYear, string strain, int calfHerdId,
                                   IEnumerable<ASREMLCalf> originalList)
        {
            _calfBirthYear = calfBirthYear;
            _calfHerdId = calfHerdId;
            _originalList = originalList;
            _strain = strain;
        }

        public List<ASREMLCalf> FilterCalvesWithoutDamCalfSNs()
        {
            return
                _originalList.Where(
                    c =>
                    c.DamCalfSn == int.MaxValue && c.HerdId == _calfHerdId && c.BirthDate.Year == _calfBirthYear &&
                    c.StrainCode == _strain).ToList();
        }

        public List<ASREMLCalf> GenerateNewCalves(IEnumerable<ASREMLCalf> listToFix)
        {
            var nextCalfSn = 3000000;

            return listToFix.Select(calf => new ASREMLCalf
                                                {
                                                    StrainCode = calf.StrainCode,
                                                    CalfSn = nextCalfSn++,
                                                    DamSn = calf.CalfSn,  // hide the original calf serial number in the dam sn
                                                    HerdId = calf.HerdId,
                                                    SireSn = int.MaxValue,
                                                    SexCode = 'F',
                                                    BirthDate = DateTime.Parse("1-Apr-" + calf.DamBirthYear),
                                                    BirthWt = short.MaxValue,
                                                    IsTwin = false,
                                                    // Apr 5 2011 - values of the bogus calves have should be NA
                                                    DamCalfSn = int.MaxValue,
                                                    WeanAge = short.MaxValue,
                                                    WeanWt = short.MaxValue,
                                                    WeanDate = DateTime.MinValue,
                                                    AgeOfDam = short.MaxValue,
                                                    BullBackFat = short.MaxValue,
                                                    BullBackFatAge = short.MaxValue,
                                                    DamHerdId = int.MaxValue,
                                                    Heifer18MonthWt = short.MaxValue,
                                                    Heifer18MonthWtAge = short.MaxValue,
                                                    HeiferYearlingWt = short.MaxValue,
                                                    HeiferYearlingWtAge = short.MaxValue,
                                                    OnTestWt = short.MaxValue,
                                                    Rfi = decimal.MaxValue,
                                                    RfiAge = short.MaxValue,
                                                    RfiFat = decimal.MaxValue,
                                                    LastTestWt = short.MaxValue,
                                                    LastTestWtAge = short.MaxValue,
                                                    ScrotalCircum = decimal.MaxValue,
                                                    ScrotalCircumAge = short.MaxValue,
                                                    DamAgeGroup = short.MaxValue,
                                                    DamBirthYear = short.MaxValue,
                                                    DaysOnTest = short.MaxValue
                                                })
                .ToList();
        }


        public List<ASREMLCalf> UpdateOriginalCalves(List<ASREMLCalf> originalCalves, IEnumerable<ASREMLCalf> newCalves)
        {
            // in each of the newly added calves, we used the dam SN to hold the original calf SN
            // so we can look it up and update its DamCalf_SN field
            foreach (var newCalf in newCalves)
            {
                var calf = newCalf;
                var calfToUpdate = originalCalves.Where(c => c.CalfSn == calf.DamSn).FirstOrDefault();
                var indexOfCalf = originalCalves.IndexOf(calfToUpdate);

                // update the field
                if (calfToUpdate != null) calfToUpdate.DamCalfSn = newCalf.CalfSn;

                // replace it.
                originalCalves.RemoveAt(indexOfCalf);
                originalCalves.Insert(indexOfCalf, calfToUpdate);
            }
            return originalCalves;
        }

    }
}