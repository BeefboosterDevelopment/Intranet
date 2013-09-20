using System;
using System.Collections.Generic;
using System.Linq;
using Beefbooster.ASREML;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BBIntranetSite.UnitTests
{
    [TestClass]
    public class If_there_are_calves_without_a_DamCalfSN_for_GJ
    {
        private FixMissingGJDamCalfSn _sut;

        private ASREMLCalf _calf1;
        private ASREMLCalf _calf2;
        private ASREMLCalf _calf3;
        private ASREMLCalf _calf4;
        private ASREMLCalf _calf5;
        private ASREMLCalf _calf6;

        [TestInitialize]
        public void Init()
        {
            // make some test calves

            // OK
            _calf1 = new ASREMLCalf
            {
                StrainCode = "M1",
                HerdId = 91,
                BirthDate = DateTime.Parse("1-Mar-2010"),
                DamCalfSn = int.MaxValue,
                DamBirthYear = 2007
            };

            // OK
            _calf2 = new ASREMLCalf
            {
                StrainCode = "M1",
                HerdId = 91,
                BirthDate = DateTime.Parse("1-Mar-2010"),
                DamCalfSn = int.MaxValue,
                DamBirthYear = 2007
            };

            // Not OK : wrong strain
            _calf3 = new ASREMLCalf
            {
                StrainCode = "M2",
                HerdId = 91,
                BirthDate = DateTime.Parse("1-Mar-2010"),
                DamCalfSn = int.MaxValue,
                DamBirthYear = 2007
            };

            //  Not OK : wrong herd id
            _calf4 = new ASREMLCalf
            {
                StrainCode = "M1",
                HerdId = 22,
                BirthDate = DateTime.Parse("1-Mar-2010"),
                DamCalfSn = int.MaxValue,
                DamBirthYear = 2007
            };

            //  Not OK : wrong birth year
            _calf5 = new ASREMLCalf
            {
                StrainCode = "M1",
                HerdId = 91,
                BirthDate = DateTime.Parse("1-Mar-2009"),
                DamCalfSn = int.MaxValue,
                DamBirthYear = 2007
            };

            //  Not OK : has a Dam Calf SN
            _calf6 = new ASREMLCalf
            {
                StrainCode = "M1",
                HerdId = 91,
                BirthDate = DateTime.Parse("1-Mar-2010"),
                DamCalfSn = 123,
                DamBirthYear = 2007
            };
        }

        [TestMethod]
        public void They_should_be_found()
        {
            var lst = new List<ASREMLCalf> { _calf1, _calf2, _calf3, _calf4, _calf5, _calf6 };
            _sut = new FixMissingGJDamCalfSn(DateTime.Now.Year - 1, "M1", 91, lst);

            var filterCalvesWithoutDamCalfSNs = _sut.FilterCalvesWithoutDamCalfSNs();
            Assert.AreEqual(2, filterCalvesWithoutDamCalfSNs.Count());
        }

    }


}