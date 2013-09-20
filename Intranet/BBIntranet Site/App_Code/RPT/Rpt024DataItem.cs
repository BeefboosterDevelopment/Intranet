namespace App_Code.RPT
{
    public class Rpt024DataItem
    {
        #region Properties

        private int? _adjBirthWt;
        private decimal? _birthWtEbv;
        private decimal? _birthWtEbvAcc;
        private int? _adjWeanWt;
        private decimal? _weanWtGrowthEbv;
        private decimal? _weanWtMilkEbv;
        private decimal? _weanWtGrowthEbvAcc;
        private decimal? _weanWtMilkEbvAcc;

        private decimal? _ywtEbv;
        private decimal? _ywtEbvAcc;

        private decimal? _matwtEbv;
        private decimal? _matwtEbvAcc;

        private decimal? _scEbv;
        private decimal? _scEbvAcc;

        private decimal? _bfEbv;
        private decimal? _bfEbvAcc;

        private decimal? _h18MEbv;
        private decimal? _h18MEbvAcc;

        private decimal? _adgBW;
        private decimal? _selectionIndex;
        private decimal? _selectionAcc;
        private int? _selectionRank;

        private int? _h18MWt;
        private int? _yearlingWt;
        private decimal? _postWeanGain;

        
        public string CalfId { get; set; }

        public string BirthDate { get; set; }

        public int? AdjBirthWt
        {
            get { return _adjBirthWt; }
            set { if (value.HasValue) _adjBirthWt = value; }
        }

        public decimal? BirthWtEbv
        {
            get { return _birthWtEbv; }
            set { if (value.HasValue) _birthWtEbv = value; }
        }

        public decimal? BirthWtEbvAcc
        {
            get { return _birthWtEbvAcc; }
            set { if (value.HasValue) _birthWtEbvAcc = value; }
        }

        public int? AdjWeanWt
        {
            get { return _adjWeanWt; }
            set { if (value.HasValue) _adjWeanWt = value; }
        }

        public decimal? WeanWtGrowthEbv
        {
            get { return _weanWtGrowthEbv; }
            set { if (value.HasValue) _weanWtGrowthEbv = value; }
        }

        public decimal? WeanWtGrowthEbvAcc
        {
            get { return _weanWtGrowthEbvAcc; }
            set { if (value.HasValue) _weanWtGrowthEbvAcc = value; }
        }

        public decimal? WeanWtMilkEbv
        {
            get { return _weanWtMilkEbv; }
            set { if (value.HasValue) _weanWtMilkEbv = value; }
        }

        public decimal? WeanWtMilkEbvAcc
        {
            get { return _weanWtMilkEbvAcc; }
            set { if (value.HasValue) _weanWtMilkEbvAcc = value; }
        }

        public decimal? YwtEbv
        {
            get { return _ywtEbv; }
            set { if (value.HasValue) _ywtEbv = value; }
        }

        public decimal? YwtEbvAcc
        {
            get { return _ywtEbvAcc; }
            set { if (value.HasValue) _ywtEbvAcc = value; }
        }


        public decimal? MatwtEbv
        {
            get { return _matwtEbv; }
            set { if (value.HasValue) _matwtEbv = value; }
        }
        public decimal? MatwtEbvAcc
        {
            get { return _matwtEbvAcc; }
            set { if (value.HasValue) _matwtEbvAcc = value; }
        }


        public decimal? ScEbv
        {
            get { return _scEbv; }
            set { if (value.HasValue) _scEbv = value; }
        }
        public decimal? ScEbvAcc
        {
            get { return _scEbvAcc; }
            set { if (value.HasValue) _scEbvAcc = value; }
        }

        public decimal? BfEbv
        {
            get { return _bfEbv; }
            set { if (value.HasValue) _bfEbv = value; }
        }
        public decimal? BfEbvAcc
        {
            get { return _bfEbvAcc; }
            set { if (value.HasValue) _bfEbvAcc = value; }
        }

        public decimal? H18MEbv
        {
            get { return _h18MEbv; }
            set { if (value.HasValue) _h18MEbv = value; }
        }
        public decimal? H18MEbvAcc
        {
            get { return _h18MEbvAcc; }
            set { if (value.HasValue) _h18MEbvAcc = value; }
        }

        public decimal? ADGBW
        {
            get { return _adgBW; }
            set { if (value.HasValue) _adgBW = value; }
        }

        public decimal? SelectionIndex
        {
            get { return _selectionIndex; }
            set { if (value.HasValue) _selectionIndex = value; }
        }

        public decimal? SelectionAcc
        {
            get { return _selectionAcc; }
            set { if (value.HasValue) _selectionAcc = value; }
        }

        public int? SelectionRank
        {
            get { return _selectionRank; }
            set { if (value.HasValue) _selectionRank = value; }
        }

        public string SireId { get; set; }
        public string DamId { get; set; }
        public int AgeOfDam { get; set; }
        public int TeatScore { get; set; }
        public int UdderScore { get; set; }
        public int NumberWeaned { get; set; }

        public int? H18MWt
        {
            get { return _h18MWt; }
            set { if (value.HasValue) _h18MWt = value; }
        }

        public int? YearlingWt
        {
            get { return _yearlingWt; }
            set { if (value.HasValue) _yearlingWt = value; }
        }

        public decimal? PostWeanGain
        {
            get { return _postWeanGain; }
            set { if (value.HasValue) _postWeanGain = value; }
        }          
        #endregion
    }
}