using System;
using System.Globalization;

namespace Csbs.Utilities
{
    public class StringHelper
    {
        #region Culture info

        private static readonly CultureInfo currentCulture = new CultureInfo("en-CA");

        #endregion

        #region Public methods

        public static Boolean ToBoolean(String value)
        {
            return ToBoolean(value, false);
        }

        public static Boolean ToBoolean(String value, Boolean defaultValue)
        {
            if (String.IsNullOrEmpty(value))
                return defaultValue;

            return String.Compare(value, "yes", true) == 0 || String.Compare(value, "1", true) == 0 || String.Compare(value, "true", true) == 0 || String.Compare(value, "y", true) == 0;
        }

        public static Int32 ToInt32(String value)
        {
            Int32? result = ToInt32Nullable(value);

            return result.HasValue ? result.Value : Int32.MinValue;
        }

        public static Int32? ToInt32Nullable(String value)
        {
            Int32 result;

            if (Int32.TryParse(value, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, NumberFormatInfo.CurrentInfo, out result))
                return result;

            return null;
        }

        public static DateTime ToDateTime(String value)
        {
            DateTime result;

            if (!DateTime.TryParse(value, out result))
                result = DateTime.MinValue;

            return result;
        }

        public static String GetMonthName(Int32 month)
        {
            return currentCulture.DateTimeFormat.GetMonthName(month);
        }

        public static String GetMonthName(DateTime date)
        {
            return GetMonthName(date.Month);
        }

        public static String ReplaceNonAlphabetCharacters(String value)
        {
            return ReplaceNonAlphabetCharacters(value, '-');
        }

        public static String ReplaceNonAlphabetCharacters(String value, Char separator)
        {
            String result = null;

            if (!String.IsNullOrEmpty(value))
            {
                foreach (Char ch in value.ToLower())
                    result += ch >= 'a' && ch <= 'z' || ch >= '0' && ch <= '9' ? ch : separator;

                String strDoubleSeparator = String.Concat(separator, separator);
                String strSeparator = String.Empty + separator;

                while (result.Contains(strDoubleSeparator))
                    result = result.Replace(strDoubleSeparator, strSeparator);

                if (result.StartsWith(strSeparator) && result.EndsWith(strSeparator))
                    result = result.Substring(1, result.Length - 2);
                else if (result.StartsWith(strSeparator))
                    result = result.Substring(1);
                else if (result.EndsWith(strSeparator))
                    result = result.Substring(0, result.Length - 1);
            }

            return result;
        }

        #endregion
    }
}