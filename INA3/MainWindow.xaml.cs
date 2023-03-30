using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ScottPlot;

namespace INA3
{
    public partial class MainWindow : Window
    {
        public double[] ComboBoxValues { get; set; }
        public Specimen Specimen { get; set; }

        ObservableCollection<DataRow> OutputDataGridValues = new ObservableCollection<DataRow>();

        List<DataRow> data = new List<DataRow>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ComboBoxValues = new double[] { 1, 0.1, 0.01, 0.001 };
        }

        public void OnClickButton(object sender, RoutedEventArgs e)
        {
            WpfPlot1.Plot.Clear();
            data = new List<DataRow>();

            if (!(
                ParseHelp.ParseDouble(ATextBox.Text, "A", out double a) &&
                ParseHelp.ParseDouble(BTextBox.Text, "B", out double b) &&
                ParseHelp.ParseInt(NTextBox.Text, "N", out int n) &&
                ParseHelp.ParseDouble(DComboBox.Text, "D", out double d) &&
                ParseHelp.ParseP(PMValue.Text, "PM", out double pm) &&
                ParseHelp.ParseP(PKValue.Text, "PK", out double pk) &&
                ParseHelp.ParseLong(TTextBox.Text, "T", out long t)
            ))
            {
                return;
            }

            if (n < 0)
            {
                MessageBox.Show("N nie może być mniejsze od 0");
                return;
            }

            if (
                pm < 0 ||
                pm > 1 ||
                pk < 0 ||
                pk > 1
            )
            {
                MessageBox.Show("PK i PM muszą być z zakresu <0;1>");
                return;
            }

            int l = (int)Math.Floor(Math.Log((b - a) / d, 2) + 1.0);

            bool elite = EliteCheckbox.IsChecked.Value;

            StaticValues.PK = pk;
            StaticValues.PM = pm;

            StaticValues.a = a;
            StaticValues.b = b;
            StaticValues.d = d;
            StaticValues.l = l;

            GenerateInitialData(n, d);

            List<double> MinFx = new List<double>();
			List<double> MaxFx = new List<double>();
			List<double> AvgFx = new List<double>();

			MinFx.Add(data.Min(x => x.OriginalSpecimen.Fx));
			MaxFx.Add(data.Max(x => x.OriginalSpecimen.Fx));
			AvgFx.Add(data.Average(x => x.OriginalSpecimen.Fx));

            for (int j = 0; j < t; j++)
            {
                CalculateGx();
                CalculatePx();
                CalculateQx();
                Selection();
                Parenting();
                PairParents();
                RandomizePC();
                Fuck();
                Mutate();
                Finalize();
                Elitism(elite);
                StartNewGeneration();
                MinFx.Add(data.Min(x => x.OriginalSpecimen.Fx));
				MaxFx.Add(data.Max(x => x.OriginalSpecimen.Fx));
				AvgFx.Add(data.Average(x => x.OriginalSpecimen.Fx));
            }
            GroupData();


            WpfPlot1.Plot.AddSignal(MaxFx.ToArray(), 1, System.Drawing.Color.Red, "Max").FillBelow();
			WpfPlot1.Plot.AddSignal(AvgFx.ToArray(), 1, System.Drawing.Color.Green, "Średnia").FillBelow();
			WpfPlot1.Plot.AddSignal(MinFx.ToArray(), 1, System.Drawing.Color.Blue, "Min").FillBelow();
			WpfPlot1.Plot.AxisAuto(0.05f, 0.1f);
			WpfPlot1.Refresh();
        }

        private void GroupData()
        {
            Dictionary<double, long> XRealCounts = new Dictionary<double, long>();
            foreach(var dataRow in data)
            {
                if(XRealCounts.ContainsKey(dataRow.OriginalSpecimen.xReal1))
                {
                    XRealCounts[dataRow.OriginalSpecimen.xReal1]++;
                }
                else
                {
                    XRealCounts.Add(dataRow.OriginalSpecimen.xReal1, 1);
                }
            }

            double dataCountAsDouble = data.Count * 0.01;

            SortedSet<DataGroup> sortedGroups = new SortedSet<DataGroup>();

            foreach(var xRealCount in XRealCounts)
            {
                sortedGroups.Add(new DataGroup(xRealCount.Key, xRealCount.Value / dataCountAsDouble, MathHelp.Fx(xRealCount.Key)));
            }

            IEnumerable<DataGroup> groupsData = sortedGroups;

            GroupDataRow[] Groups = new GroupDataRow[sortedGroups.Count];

            long i = 0;
            foreach(var group in groupsData)
            {
                Groups[i] = new GroupDataRow(i + 1, group.XReal, group.Percent);
                i++;
            }

            //MessageBox.Show($"Grup: {Groups.Length}");

            OutputDataGrid.ItemsSource = Groups.ToList();
        }

        private void Elitism(bool elite)
        {
            if (!elite) return;

            double bestGx = double.MinValue;
            double bestXReal = 0;
            double bestFx = 0;

            for(int i = 0; i < data.Count; i++)
            {
                if (data[i].GxValue > bestGx)
                {
                    bestGx = data[i].GxValue;
                    bestXReal = data[i].OriginalSpecimen.xReal1;
                    bestFx = data[i].OriginalSpecimen.Fx;
                }
            } 

            for(int i = 0; i < data.Count; i++)
            {
                double Fx = data[i].FinalFXRealValue;

                if (Fx >= bestFx)
                {
                    return;
                }
            }

            int index = StaticValues.Random.Next(0, data.Count - 1);
            data[index].FinalFXRealValue = bestFx;
            data[index].FinalXRealValue = bestXReal;
        }

        private void StartNewGeneration()
        {
            for (int i = 0; i < data.Count; i++)
            {
                Specimen specimen = new Specimen();
                specimen.xReal1 = data[i].FinalXRealValue;
                specimen.xInt1 = MathHelp.XRealToXInt(specimen.xReal1);
                specimen.xBin = MathHelp.XIntToXBin(specimen.xInt1);
                specimen.Fx = MathHelp.Fx(specimen.xReal1);

                data[i] = new DataRow(specimen, i + 1);
            }
        }

        private void GenerateInitialData(int n, double d)
        {
            for (int i = 0; i < n; i++)
            {
                int accuracy = d switch
                {
                    1.0 => 0,
                    0.1 => 1,
                    0.01 => 2,
                    0.001 => 3
                };

                Specimen specimen = new Specimen();
                specimen.xReal1 = StaticValues.RandomXReal();
                specimen.xInt1 = MathHelp.XRealToXInt(specimen.xReal1);
                specimen.xBin = MathHelp.XIntToXBin(specimen.xInt1);
                specimen.Fx = MathHelp.Fx(specimen.xReal1);

                var dataRow = new DataRow(specimen, i + 1);
                data.Add(dataRow);
                OutputDataGridValues.Add(dataRow);
            }
        }

        public void CalculateGx()
        {
            double min = data.Min(x => x.OriginalSpecimen.Fx);
            foreach (var dataRow in data)
            {
                dataRow.GxValue = dataRow.OriginalSpecimen.Fx - min + StaticValues.d;
            }
        }

        public void CalculatePx()
        {
            double sum = data.Sum(x => x.GxValue);
            foreach (var dataRow in data)
            {
                dataRow.PxValue = dataRow.GxValue / sum;
            }
        }

        public void CalculateQx()
        {
            double sum = 0.0;
            foreach (var dataRow in data)
            {
                sum += dataRow.PxValue;
                dataRow.QxValue = sum;
            }
        }

        private void Selection()
        {
            foreach (var row in data)
            {
                row.SelectionRandom = StaticValues.Random.NextDouble();
                int selectedIndex = data.Count - 1;
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].QxValue >= row.SelectionRandom)
                    {
                        selectedIndex = i;
                        break;
                    }
                }

                row.SelectionValue = data[selectedIndex].OriginalSpecimen;
            }
        }

        private void Parenting()
        {
            foreach (var row in data)
            {
                row.ParentRandom = StaticValues.Random.NextDouble();
            }
        }

        private void PairParents()
        {
            for (int i = 0; i < data.Count; i++)
            {
                DataRow row = data[i];
                if (!row.isParent || row.ParentsWith != null) continue;
                DataRow pair = null;
                if (i + 1 < data.Count)
                {
                    for (int j = i + 1; j < data.Count; j++)
                    {
                        if (!data[j].isParent) continue;
                        pair = data[j];
                        break;
                    }
                }

                if (pair != null)
                {
                    row.ParentsWith = pair;
                    pair.ParentsWith = row;
                }
            }
        }

        private void RandomizePC()
        {
            for (int i = 0; i < data.Count; i++)
            {
                var row = data[i];
                if (row.ParentsWith == null || row.PCValue != null) continue;

                int pc = 1 + (int)Math.Round(StaticValues.Random.NextDouble() * (StaticValues.l - 2));
                row.PCValue = pc;
                row.ParentsWith.PCValue = pc;
            }
        }

        private void Fuck()
        {
            for (int i = 0; i < data.Count; i++)
            {
                var row = data[i];
                if (row.ParentsWith == null) continue;

                string FirstParentPart = row.SelectionXBin.Substring(0, row.PCValue.Value);
                string spacer = " | ";
                string SecondParentPart = row.ParentsWith.SelectionXBin.Substring(row.PCValue.Value);
                row.ChildXBin = FirstParentPart + spacer + SecondParentPart;
            }
        }

        private void Mutate()
        {
            for (int i = 0; i < data.Count; i++)
            {
                data[i].MutatedGenesValue = new List<int>();
                data[i].MutatedChromosomeValue = "";
                string chromosom = data[i].AfterChild;
                for (int bit = 0; bit < StaticValues.l; bit++)
                {
                    if (StaticValues.Random.NextDouble() < StaticValues.PM)
                    {
                        data[i].MutatedGenesValue.Add(bit);

                        if (chromosom[bit] == '0')
                        {
                            data[i].MutatedChromosomeValue += "1";
                        }
                        else
                        {
                            data[i].MutatedChromosomeValue += "0";
                        }
                    }
                    else
                    {
                        data[i].MutatedChromosomeValue += chromosom[bit];
                    }
                }
            }
        }

        private void Finalize()
        {
            foreach (var row in data)
            {
                row.FinalXRealValue = MathHelp.XBinToXReal(row.MutatedChromosomeValue);
                row.FinalFXRealValue = MathHelp.Fx(row.FinalXRealValue);
            }
        }
    }
}


