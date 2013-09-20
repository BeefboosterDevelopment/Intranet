using System;

namespace Beefbooster.BusinessLogic
{
	public class Constants
	{
		private Constants(){}

        public static readonly Int32 InitializeInt = -1;
        public static readonly short InitializeShort = -1;
        public static readonly byte InitializeByte = (byte)0;
        public static readonly decimal InitializeDecimal = 0;
        public static readonly double InitializeDouble = double.NaN;
        public static readonly float InitializeFloat = float.NaN;
        public static readonly string InitializeString = String.Empty;
		public static readonly DateTime InitializeDateTime = DateTime.MinValue;
		public static readonly int MaxSmallXmlSize = 2048;
	}
}