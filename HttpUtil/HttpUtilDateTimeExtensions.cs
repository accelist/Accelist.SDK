using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Contains extension methods for HttpRequestMessage object.
    /// </summary>
    public static class HttpUtilDateTimeExtensions
    {
        /// <summary>
        /// Converts the value of current DateTime object into UTC.
        /// If current object DateTimeKind is unspecified, specify the kind as UTC but no value conversion is performed.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToUniversalTimeIgnoreUnspecified(this DateTime dt)
        {
            if (dt.Kind == DateTimeKind.Local)
                return dt.ToUniversalTime();

            if (dt.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(dt, DateTimeKind.Utc);

            return dt;
        }
    }
}
