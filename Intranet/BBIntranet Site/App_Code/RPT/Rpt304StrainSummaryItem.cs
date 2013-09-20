using System;
using System.Collections.Generic;
using System.Linq;

public class Rpt304StrainSummaryItem
{
    public string HerdCode { get; set; }
    public int Count { get; set; }
    public DateTime Birth_Date { get; set; }
    public short RawBirth_Wt { get; set; }
    public decimal SEL_IDX { get; set; }
    public decimal BW_EBV { get; set; }
    public decimal WWD_EBV { get; set; }
    public decimal YWT_EBV { get; set; }
    public decimal SC_EBV { get; set; }
    public decimal BF_EBV { get; set; }
    public decimal WWM_EBV { get; set; }
    public decimal MW_EBV { get; set; }
    public decimal RFI_EBV { get; set; }
    public decimal H18M_EBV { get; set; }
    public decimal BW_EBV_REL { get; set; }
    public decimal WWD_EBV_REL { get; set; }
    public decimal YWT_EBV_REL { get; set; }
    public decimal SC_EBV_REL { get; set; }
    public decimal BF_EBV_REL { get; set; }
    public decimal WWM_EBV_REL { get; set; }
    public decimal MW_EBV_REL { get; set; }
    public decimal RFI_EBV_REL { get; set; }
    public decimal H18M_EBV_REL { get; set; }
    public short AgeOfDam { get; set; }
    public short Dam_Wt { get; set; }
    public byte Teat { get; set; }
    public byte Udder { get; set; }
    public byte FF { get; set; }
    public byte FL { get; set; }
    public byte HF { get; set; }
    public byte HL { get; set; }
    public byte Morph { get; set; }
    public byte Motil { get; set; }
    public byte Conc { get; set; }
    public byte Disp { get; set; }
    public short BW_ADJ { get; set; }
    public short WW_ADJ { get; set; }
    public short YW_ADJ { get; set; }
    public short H18MW_ADJ { get; set; }
    public decimal ADG_BW_ADJ { get; set; }
    public Int16 BACKFAT_ADJ { get; set; }
    public decimal SCROTCIRC_ADJ { get; set; }
    public decimal Stn_ADG { get; set; }
}


public class Rpt304StrainSummaryDataObject
{
    private readonly IEnumerable<Rpt304_DataItem> _details;

    public Rpt304StrainSummaryDataObject(IEnumerable<Rpt304_DataItem> details)
    {
        _details = details;
    }

    public IEnumerable<Rpt304StrainSummaryItem> Summarize()
    {
        var lst = new List<Rpt304StrainSummaryItem>();

        // for each herd
        IOrderedEnumerable<IGrouping<string, Rpt304_DataItem>> grplst =
            _details.GroupBy(e => e.Herd_Code).OrderBy(e => e.Key);
        lst.AddRange(grplst.Select(s => SummaryItem(s.Key)));

        // for the strain
        lst.Add(SummaryItem(null));
        return lst;
    }

    private Rpt304StrainSummaryItem SummaryItem(string herdCode)
    {
        List<Rpt304_DataItem> subset = string.IsNullOrEmpty(herdCode)
                                           ? _details.ToList()
                                           : _details.Where(s => s.Herd_Code == herdCode).ToList();
        if (subset.Count == 0) return new Rpt304StrainSummaryItem {HerdCode = herdCode, Count = 0};

/*
        var strainSummary = new Rpt304StrainSummaryItem
                                {
                                    HerdCode = string.IsNullOrEmpty(herdCode)?"STRAIN":herdCode,
                                    Count = subset.Count,
                                    AgeOfDam = (short)subset.Where(s => s.AgeOfDam > 0).Average(s => s.AgeOfDam),
                                    BACKFAT_ADJ = (short)subset.Where(s => s.BACKFAT_ADJ > 0).Average(s => s.BACKFAT_ADJ),
                                    ADG_BW_ADJ = subset.Where(s => s.ADG_BW_ADJ > 0).Average(s => s.ADG_BW_ADJ),
                                    BF_EBV = subset.Average(s => s.BF_EBV),
                                    BF_EBV_REL = subset.Average(s => s.BF_EBV_REL),
                                    BW_EBV = subset.Average(s => s.BW_EBV),
                                    BW_ADJ = (short)subset.Where(s => s.BW_ADJ > 0).Average(s => s.BW_ADJ),
                                    BW_EBV_REL = subset.Average(s => s.BW_EBV_REL),
                                    //Birth_Date = (DateTime)subset.Average(s => s.Birth_Date),
                                    Conc = (byte)subset.Where(s => s.Conc > 0).Average(s => s.Conc),
                                    Dam_Wt = (short)subset.Where(s => s.Dam_Wt > 0).Average(s => s.Dam_Wt),
                                    Disp = (byte)subset.Where(s => s.Disp > 0).Average(s => s.Disp),
                                    FF = (byte)subset.Where(s => s.FF > 0).Average(s => s.FF),
                                    FL = (byte)subset.Where(s => s.FL > 0).Average(s => s.FL),
                                    H18MW_ADJ = (short)subset.Where(s => s.H18MW_ADJ > 0).Average(s => s.H18MW_ADJ),
                                    H18M_EBV = subset.Average(s => s.H18M_EBV),
                                    H18M_EBV_REL = subset.Average(s => s.H18M_EBV_REL),
                                    HF = (byte)subset.Where(s => s.HF > 0).Average(s => s.HF),
                                    HL = (byte)subset.Where(s => s.HL > 0).Average(s => s.HL),
                                    MW_EBV = subset.Average(s => s.MW_EBV),
                                    MW_EBV_REL = subset.Average(s => s.MW_EBV_REL),
                                    Morph = (byte)subset.Where(s => s.Morph > 0).Average(s => s.Morph),
                                    Motil = (byte)subset.Where(s => s.Motil > 0).Average(s => s.Motil),
                                    RFI_EBV = subset.Average(s => s.RFI_EBV),
                                    RFI_EBV_REL = subset.Average(s => s.RFI_EBV_REL),
                                    RawBirth_Wt = (short)subset.Where(s => s.RawBirth_Wt > 0).Average(s => s.RawBirth_Wt),
                                    SCROTCIRC_ADJ = subset.Where(s => s.SCROTCIRC_ADJ > 0).Average(s => s.SCROTCIRC_ADJ),
                                    SC_EBV = subset.Average(s => s.SC_EBV),
                                    SC_EBV_REL = subset.Average(s => s.SC_EBV_REL),
                                    SEL_IDX = subset.Average(s => s.SEL_IDX),
                                    Stn_ADG = subset.Where(s => s.Stn_ADG > 0).Average(s => s.Stn_ADG),
                                    Teat = (byte)subset.Where(s => s.Teat > 0).Average(s => s.Teat),
                                    Udder = (byte)subset.Where(s => s.Udder > 0).Average(s => s.Udder),
                                    WWD_EBV = subset.Average(s => s.WWD_EBV),
                                    WWD_EBV_REL = subset.Average(s => s.WWD_EBV_REL),
                                    WWM_EBV = subset.Average(s => s.WWM_EBV),
                                    WWM_EBV_REL = subset.Average(s => s.WWM_EBV_REL),
                                    WW_ADJ = (short)subset.Where(s => s.WW_ADJ > 0).Average(s => s.WW_ADJ),
                                    YWT_EBV = subset.Average(s => s.YWT_EBV),
                                    YWT_EBV_REL = subset.Average(s => s.YWT_EBV_REL),
                                    YW_ADJ = (short)subset.Where(s => s.YW_ADJ > 0).Average(s => s.YW_ADJ)
                                };
        */
        if (subset.Count == 0) return null;

        var strainSummary = new Rpt304StrainSummaryItem
        {
            HerdCode = string.IsNullOrEmpty(herdCode) ? "STRAIN" : herdCode,
            Count = subset.Count,
            AgeOfDam = (short)subset.Average(s => s.AgeOfDam),
            BACKFAT_ADJ = (short)subset.Average(s => s.BACKFAT_ADJ),
            ADG_BW_ADJ = subset.Average(s => s.ADG_BW_ADJ),
            BF_EBV = subset.Average(s => s.BF_EBV),
            BF_EBV_REL = subset.Average(s => s.BF_EBV_REL),
            BW_EBV = subset.Average(s => s.BW_EBV),
            BW_ADJ = (short)subset.Average(s => s.BW_ADJ),
            BW_EBV_REL = subset.Average(s => s.BW_EBV_REL),
            //Birth_Date = (DateTime)subset.Average(s => s.Birth_Date),
            Conc = (byte)subset.Average(s => s.Conc),
            Disp = (byte)subset.Average(s => s.Disp),
            FF = (byte)subset.Average(s => s.FF),
            FL = (byte)subset.Average(s => s.FL),
            H18MW_ADJ = (short)subset.Average(s => s.H18MW_ADJ),
            H18M_EBV = subset.Average(s => s.H18M_EBV),
            H18M_EBV_REL = subset.Average(s => s.H18M_EBV_REL),
            HF = (byte)subset.Average(s => s.HF),
            HL = (byte)subset.Average(s => s.HL),
            MW_EBV = subset.Average(s => s.MW_EBV),
            MW_EBV_REL = subset.Average(s => s.MW_EBV_REL),
            Morph = (byte)subset.Average(s => s.Morph),
            Motil = (byte)subset.Average(s => s.Motil),
            RFI_EBV = subset.Average(s => s.RFI_EBV),
            RFI_EBV_REL = subset.Average(s => s.RFI_EBV_REL),
            RawBirth_Wt = (short)subset.Average(s => s.RawBirth_Wt),
            SCROTCIRC_ADJ = subset.Average(s => s.SCROTCIRC_ADJ),
            SC_EBV = subset.Average(s => s.SC_EBV),
            SC_EBV_REL = subset.Average(s => s.SC_EBV_REL),
            SEL_IDX = subset.Average(s => s.SEL_IDX),
            Stn_ADG = subset.Average(s => s.Stn_ADG),

            WWD_EBV = subset.Average(s => s.WWD_EBV),
            WWD_EBV_REL = subset.Average(s => s.WWD_EBV_REL),
            WWM_EBV = subset.Average(s => s.WWM_EBV),
            WWM_EBV_REL = subset.Average(s => s.WWM_EBV_REL),
            WW_ADJ = (short)subset.Average(s => s.WW_ADJ),
            YWT_EBV = subset.Average(s => s.YWT_EBV),
            YWT_EBV_REL = subset.Average(s => s.YWT_EBV_REL),
            YW_ADJ = (short)subset.Average(s => s.YW_ADJ)
        };
        strainSummary.Dam_Wt = (short)((subset.Count(s => s.Dam_Wt > 0) == 0)
                                   ? 0
                                   : subset.Where(s => s.Dam_Wt>0).Average(s => s.Dam_Wt));

        strainSummary.Teat = (byte)((subset.Count(s => s.Teat > 0) == 0)
                                   ? 0
                                   : subset.Where(s => s.Teat > 0).Average(s => s.Teat));

        strainSummary.Udder = (byte)((subset.Count(s => s.Udder > 0) == 0)
                                   ? 0
                                   : subset.Where(s => s.Udder > 0).Average(s => s.Udder));
        return strainSummary;
    }
}