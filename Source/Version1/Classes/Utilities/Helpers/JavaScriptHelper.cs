using System;

namespace Csbs.Utilities
{
    public static class JavaScriptHelper
    {
        public static String GetJsValue(Object value, String nullValue = "null")
        {
            String result = nullValue;

            if (value != null)
            {
                if (value is Boolean)
                {
                    Boolean boolValue = (Boolean)value;

                    result = boolValue ? "true" : "false";
                }
                else if (value is String)
                {
                    String stringValue = value as String;

                    result = !String.IsNullOrEmpty(stringValue) ? "'" + stringValue.Replace("\r", "\\r").Replace("\n", "\\n").Replace("'", "\\'") + "'" : nullValue;
                }
                else
                {
                    result = value.ToString();
                }
            }

            return result;
        }

        public static String GetShowMessageScript(String message)
        {
            return String.IsNullOrEmpty(message)
                ? null
                : String.Format("\r\nalert({0});\r\n", GetJsValue(message));
        }

        public static String GetRedirectScript(String url)
        {
            return String.IsNullOrEmpty(url)
                ? null
                : String.Format("\r\ndocument.location = {0};\r\n", GetJsValue(url));
        }
    }
}