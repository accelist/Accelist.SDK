using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accelist.SDK.SQL.Defragment
{
    /// <summary>
    /// Contains fragmentation information about an index in the database.
    /// </summary>
    public class IndexScan
    {
        /// <summary>
        /// Sets or gets the schema name of the table.
        /// </summary>
        public string Schema { set; get; }

        /// <summary>
        /// Sets or gets the name of the table.
        /// </summary>
        public string Table { set; get; }

        /// <summary>
        /// Sets or gets the name of the index.
        /// </summary>
        public string Index { set; get; }

        /// <summary>
        /// Sets or gets the number of pages in the index.
        /// </summary>
        public int PageCount { set; get; }

        /// <summary>
        /// Sets or gets the fragmentation rate of the index.
        /// </summary>
        public double FragmentRate { set; get; }

        /// <summary>
        /// Sets or gets the information of current fill factor of the index.
        /// </summary>
        public int FillFactor { set; get; }

        /// <summary>
        /// Sets or gets the information whether the index is created on a PRIMARY KEY IDENTITY column.
        /// </summary>
        public bool IsIdentity { set; get; }

        /// <summary>
        /// Returns a computed fill factor, depending on currently set fill factor of the index, and whether the index column is an IDENTITY-type (auto-incrementing) PRIMARY KEY.
        /// </summary>
        /// <returns></returns>
        public int CalculateFillFactor()
        {
            if (FillFactor != 0)
            {
                // Has been set previously, either by this library or manually. Use it
                return FillFactor;
            }
            if (IsIdentity)
            {
                return 100;
            }
            else
            {
                return 90;
            }
        }
    }
}
