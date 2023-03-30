namespace INA3
{
    class GroupDataRow
    {
        public long Index = 0;
        public double xRealValue = 0;
        public string xBinValue = "";
        public double FxValue = 0;
        public double PercentValue = 0;

        public static GroupDataRow Empty = new GroupDataRow();

        public GroupDataRow()
        {
        }

        public GroupDataRow(long index, double xRealValue, double percentValue)
        {
            Index = index;
            this.xRealValue = xRealValue;
            this.xBinValue = MathHelp.XIntToXBin(MathHelp.XRealToXInt(xRealValue));
            this.FxValue = MathHelp.Fx(xRealValue);
            PercentValue = percentValue;
        }

        public long N => Index;
        public double xReal => xRealValue;
        public string xBin => xBinValue;
        public double Fx => FxValue;
        public double Percent => PercentValue;  
    }
}
