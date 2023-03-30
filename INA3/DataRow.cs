using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA3
{
    class DataRow
    {
        public static DataRow Empty = new DataRow(null, -1);

        public Specimen OriginalSpecimen = null;
        public long Index;

        public double SelectionRandom;
        public double GxValue = 0.0;
        public double PxValue = 0.0;
        public double QxValue = 0.0;
        public Specimen SelectionValue;
        public double ParentRandom;
        public bool isParent => ParentRandom < StaticValues.PK;
        public DataRow ParentsWith = null;
        public int? PCValue;
        public string ChildXBin;
        public List<int> MutatedGenesValue = new List<int>();
        public string MutatedChromosomeValue = null;
        public double FinalXRealValue;
        public double FinalFXRealValue;

        public DataRow(Specimen originalSpecimen, long index)
        {
            OriginalSpecimen = originalSpecimen;
            Index = index;
        }

        public string N => Index.ToString();
        public string xReal => OriginalSpecimen.xReal1.ToString();
        public string Fx => OriginalSpecimen.Fx.ToString("N20").TrimEnd('0');

        public string Gx => GxValue.ToString();
        public string Px => PxValue.ToString();
        public string Qx => QxValue.ToString();
        public string R1 => SelectionRandom.ToString();

        public string SelectionXReal1 => SelectionValue.xReal1.ToString();
        public string SelectionXBin => SelectionValue.xBin;

        public string FirstParentXBin => isParent ? SelectionXBin : "-";

        public string PC => (PCValue != null ? PCValue.ToString() : "-");
        public string Child => ChildXBin ?? "-";

        public string AfterChild => ChildXBin != null ? ChildXBin.Replace(" | ", "") : SelectionXBin;

        public string MutatedGenes => MutatedGenesValue.Count > 0 ? MutatedGenesValue.Aggregate("", (s, i) => $"{s}, {i + 1}").Substring(1) : "-";
        public string MutatedChromosome => MutatedChromosomeValue ?? "-";

        public string FinalXReal => FinalXRealValue.ToString();
        public string FinalFXReal => FinalFXRealValue.ToString();
    }
}
