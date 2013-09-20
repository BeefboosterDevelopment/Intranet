using System;
using System.Collections.Generic;
using System.Linq;
using Beefbooster.ASREML;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BBIntranetSite.UnitTests
{
    [TestClass]
    public class When_updating_original_calves_with_DamCalfSn_for_GJ
    {
        private ASREMLCalf _calf1;
        private ASREMLCalf _calf2;
        private ASREMLCalf _calf3;
        private ASREMLCalf _calf4;
        private FixMissingGJDamCalfSn _sut;
        private List<ASREMLCalf> _originalList;
        private List<ASREMLCalf> _filterCalvesWithoutDamCalfSNs;
        private List<ASREMLCalf> _newCalves;

        [TestInitialize]
        public void Init()
        {
            // OK
            _calf1 = new ASREMLCalf
            {
                CalfSn = 1,
                StrainCode = "M1",
                HerdId = 91,
                BirthDate = DateTime.Parse("1-Mar-2010"),
                DamCalfSn = int.MaxValue,
                DamBirthYear = 2007
            };

            // OK
            _calf2 = new ASREMLCalf
            {
                CalfSn = 2,
                StrainCode = "M1",
                HerdId = 91,
                BirthDate = DateTime.Parse("1-Mar-2010"),
                DamCalfSn = int.MaxValue,
                DamBirthYear = 2007
            };

            // Not OK : wrong strain
            _calf3 = new ASREMLCalf
            {
                CalfSn = 3,
                StrainCode = "M2",
                HerdId = 91,
                BirthDate = DateTime.Parse("1-Mar-2010"),
                DamCalfSn = int.MaxValue,
                DamBirthYear = 2007
            };

            // OK
            _calf4 = new ASREMLCalf
            {
                CalfSn = 54321,
                StrainCode = "M1",
                HerdId = 91,
                BirthDate = DateTime.Parse("1-Mar-2010"),
                DamCalfSn = int.MaxValue,
                DamBirthYear = 2005
            };


            _originalList = new List<ASREMLCalf> { _calf1, _calf2, _calf3, _calf4 };
            _sut = new FixMissingGJDamCalfSn(DateTime.Now.Year - 1, "M1", 91, _originalList);

            _filterCalvesWithoutDamCalfSNs = _sut.FilterCalvesWithoutDamCalfSNs();
            Assert.AreEqual(3, _filterCalvesWithoutDamCalfSNs.Count());

            _newCalves = _sut.GenerateNewCalves(_filterCalvesWithoutDamCalfSNs);
            Assert.AreEqual(3, _newCalves.Count());
        }

        [TestMethod]
        public void Should_update_the_DamCalfSn_correctly()
        {
            var newList = _sut.UpdateOriginalCalves(_originalList, _newCalves);

            Assert.AreEqual(newList[0].DamCalfSn, _newCalves[0].CalfSn);
            Assert.AreEqual(newList[1].DamCalfSn, _newCalves[1].CalfSn);
            Assert.AreEqual(newList[2].DamCalfSn, int.MaxValue);
            Assert.AreEqual(newList[3].DamCalfSn, _newCalves[2].CalfSn);
        }
    }
}