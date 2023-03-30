using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA3
{
	public static class StaticValues
	{
		public static double PK = 0.5;
		public static double PM = 0.02;

		public static double a;
		public static double b;
		public static double d;
		public static int l;

		public static Random Random = new Random();

		public static double RandomXReal()
		{
			int accuracy = MathHelp.Accuracy(d);
			var truexReal = Random.NextDouble() * (b - a) + a;
			return Math.Round(truexReal, accuracy);
		}
	}
}
