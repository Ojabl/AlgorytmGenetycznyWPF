using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA3
{
    class DataGroup : IComparer<DataGroup>, IComparable<DataGroup>
    {
        public double XReal;
        public double Percent;
        public double Fx;

        public DataGroup(double xReal, double percent, double fx)
        {
            XReal = xReal;
            Percent = percent;
            Fx = fx;
        }

        public int Compare(DataGroup x, DataGroup y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(x, null)) return 1;
            if (ReferenceEquals(x, null)) return -1;
            return x.Fx.CompareTo(y.Fx);
        }

        public int CompareTo(DataGroup other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Fx.CompareTo(other.Fx);
        }
    }
}
