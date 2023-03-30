using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;

namespace INA3
{
    public static class ParseHelp
    {
        public static bool ParseP(string text, string textName, out double output, string culture = "")
        {
            if (culture.Length == 0)
                culture = CultureInfo.CurrentCulture.ToString();

            try
            {
                output = Double.Parse(text, NumberStyles.Number, new CultureInfo(culture));
                return true;
            }
            catch (FormatException _)
            {
                MessageBox.Show($"{textName} ma niepoprawny format, przykład poprawnego formatu: {0.3.ToString(new CultureInfo(culture))}");
            }
            catch
            {
                MessageBox.Show($"{textName} nie należy do przedziału od {double.MinValue} do {double.MaxValue}");
            }

            output = double.NaN;
            return false;
        }

        public static bool ParseDouble(string text, string textName, out double output, string culture = "")
        {
            if (culture.Length == 0)
                culture = CultureInfo.CurrentCulture.ToString();

            try
            {
                output = Double.Parse(text, NumberStyles.Number, new CultureInfo(culture));
                return true;
            }
            catch (FormatException _)
            {
                MessageBox.Show($"{textName} ma niepoprawny format, przykład poprawnego formatu: {3.2.ToString(new CultureInfo(culture))}");
            }
            catch
            {
                MessageBox.Show($"{textName} nie należy do przedziału od {double.MinValue} do {double.MaxValue}");
            }

            output = double.NaN;
            return false;
        }

        public static bool ParseLong(string text, string textName, out long output, string culture = "")
        {
            if (culture.Length == 0)
                culture = CultureInfo.CurrentCulture.ToString();

            try
            {
                output = long.Parse(text, NumberStyles.Number, new CultureInfo(culture));
                return true;
            }
            catch (FormatException _)
            {
                MessageBox.Show($"{textName} ma niepoprawny format, przykład poprawnego formatu: {8}");
            }
            catch
            {
                MessageBox.Show($"{textName} nie należy do przedziału od {long.MinValue.ToString(culture)} do {long.MaxValue.ToString(culture)}");
            }

            output = 0;
            return false;
        }

        public static bool ParseInt(string text, string textName, out int output, string culture = "")
        {
            if (culture.Length == 0)
                culture = CultureInfo.CurrentCulture.ToString();

            try
            {
                output = int.Parse(text, NumberStyles.Number, new CultureInfo(culture));
                return true;
            }
            catch (FormatException _)
            {
                MessageBox.Show($"{textName} ma niepoprawny format, przykład poprawnego formatu: {9.75}");
            }
            catch
            {
                MessageBox.Show($"{textName} nie należy do przedziału od {Int32.MinValue.ToString(culture)} do {Int32.MaxValue.ToString(culture)}");
            }

            output = 0;
            return false;
        }
    }
}
