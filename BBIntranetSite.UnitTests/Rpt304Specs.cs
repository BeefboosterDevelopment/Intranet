using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BBIntranetSite.UnitTests
{
/*    [TestClass]
    public class When_generating_the_strain_summary
    {
        List<Rpt304_DataItem> _details = new List<Rpt304_DataItem>();

        [TestInitialize]
        public void inittest()
        {
            _details.Add(new Rpt304_DataItem {Strain_Code = "M3", BW_EBV = .1M, Morph = 99});
            _details.Add(new Rpt304_DataItem {Strain_Code = "M3", BW_EBV = .2M, Morph = 102});
            _details.Add(new Rpt304_DataItem {Strain_Code = "M3", BW_EBV = .2M, Morph = 102});
            _details.Add(new Rpt304_DataItem {Strain_Code = "M3", BW_EBV = .3M, Morph = 103});
        }
    

        public class testavg
        {
            public decimal bwebv { get; set; }
            public byte morph { get; set; }
        }

        [TestMethod]
        public void It_should_average_EBVs()
        {

            var avgs = new testavg
                           {
                               bwebv = _details.Average(s => s.BW_EBV),
                               morph = (byte)_details.Average(s => s.Morph)
                           };

            Assert.AreEqual(0.2M, avgs.bwebv);
            Assert.AreEqual(102, avgs.morph);


        }

    }*/
}