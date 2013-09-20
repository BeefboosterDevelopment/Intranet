using System;
using System.Collections.Generic;
using System.Linq;
using Beefbooster.ASREML;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BBIntranetSite.UnitTests
{
    [TestClass]
    public class When_generating_new_calves_for_GJ
    {
        private ASREMLCalf _calf1;
        private ASREMLCalf _calf2;
        private ASREMLCalf _calf3;
        private FixMissingGJDamCalfSn _sut;
        private List<ASREMLCalf> _originalList;
        private List<ASREMLCalf> _filterCalvesWithoutDamCalfSNs;

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

            _originalList = new List<ASREMLCalf> { _calf1, _calf2, _calf3 };
            _sut = new FixMissingGJDamCalfSn(DateTime.Now.Year - 1, "M1", 91, _originalList);

            _filterCalvesWithoutDamCalfSNs = _sut.FilterCalvesWithoutDamCalfSNs();
            Assert.AreEqual(2, _filterCalvesWithoutDamCalfSNs.Count());

        }

        [TestMethod]
        public void They_should_be_correct()
        {
            var newCalves = _sut.GenerateNewCalves(_filterCalvesWithoutDamCalfSNs);

            Assert.AreEqual(2, newCalves.Count());
            Assert.IsTrue(newCalves.All(c => c.DamCalfSn == int.MaxValue));
            Assert.AreEqual(1, newCalves.Where(c => c.CalfSn == 3000000).Count());
            Assert.IsTrue(newCalves.All(c => c.CalfSn >= 3000000));
        }

        [TestMethod]
        public void The_dam_sn_should_hold_the_original_calf_sn()
        {
            var newCalves = _sut.GenerateNewCalves(_filterCalvesWithoutDamCalfSNs);

            _originalList[0].CalfSn = newCalves[0].DamSn;
            _originalList[1].CalfSn = newCalves[1].DamSn;
        }
    }
}