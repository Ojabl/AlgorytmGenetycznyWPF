using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA3
{
	public class Specimen
	{
		public double accuracy { get; set; }
		public double LP { get; set; }
		public double xReal1 { get; set; }
		public long xInt1 { get; set; }
		public string xBin { get; set; }
		public double Fx { get; set; }
	}
}
