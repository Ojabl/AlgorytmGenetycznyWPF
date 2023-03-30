using System;
using System.Collections.Generic;
using System.Text;

namespace INA3
{
	public static class MathHelp
	{
		public static int Accuracy(double d)
		{
			return d switch
			{
				1.0 => 0,
				0.1 => 1,
				0.01 => 2,
				0.001 => 3
			};
		}

		public static long XBinToXInt(string xBin)
		{
			return Convert.ToInt64(xBin, 2);
		}

		public static double XIntToXReal(long xInt)
		{
			double trueXReal = ((StaticValues.b - StaticValues.a) * xInt) / (Math.Pow(2.0, StaticValues.l) - 1.0) + StaticValues.a;
			return Math.Round(trueXReal, Accuracy(StaticValues.d));
		}

		public static double XBinToXReal(string xBin)
		{
			return XIntToXReal(XBinToXInt(xBin));
		}

		public static double Fx(double xReal)
		{
			return (xReal % 1.0) * (Math.Cos(20.0 * Math.PI * xReal) - Math.Sin(xReal));
		}

		public static long XRealToXInt(double xReal)
		{
			return (long)Math.Round((1.0 / (StaticValues.b - StaticValues.a)) * (xReal - StaticValues.a) * ((Math.Pow(2.0, StaticValues.l)) - 1.0));
		}

		public static string XIntToXBin(long xInt)
		{
			return Convert.ToString(xInt, 2).PadLeft(StaticValues.l, '0');
		}
	}
}
