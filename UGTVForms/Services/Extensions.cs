using System;

namespace UGTVForms.Services
{
    public static class Extensions
    {
        public static string UGTVDateTimeFormatted(this DateTime dt)
        {
            return dt.ToString("MM/dd/yy");
        }
    }
}
