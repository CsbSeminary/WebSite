using System;

namespace Csbs.Utilities
{
    public class ValueHelper
    {
        public static String ToString(Object value)
        {
            return ToString(value, null);
        }

        public static String ToString(Object value, String formatString)
        {
            String result = null;

            if (IsNotNull(value))
            {
                if (value is DateTime)
                    result = ((DateTime)value).ToString(formatString);
                else if (value is Boolean)
                    result = (Boolean)value ? "1" : "0";
                else if (value is Enum)
                    result = ((Int32)value).ToString();
                else
                    result = value.ToString();
            }

            return result;
        }

        public static DateTime? ToDateTimeNullable(Object value)
        {
            DateTime? result = null;

            if (IsNotNull(value))
                result = Convert.ToDateTime(value);

            return result;
        }

        public static Boolean IsNotNull(Object value)
        {
            return !IsNull(value);
        }

        public static Boolean IsNull(Object value)
        {
            return value == null || value == DBNull.Value;
        }
    }
}