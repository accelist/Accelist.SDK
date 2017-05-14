using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Dapper
{
    public class IndexScan
    {
        public string Schema { set; get; }

        public string Table { set; get; }

        public string Index { set; get; }

        public int PageCount { set; get; }

        public double FragmentRate { set; get; }

        public int FillFactor { set; get; }

        public bool IsIdentity { set; get; }

        /// <summary>
        /// Returns a calculated fill factor, depending on currently set fill factor of the index, and whether the index column is an IDENTITY-type (auto-incrementing) PRIMARY KEY.
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

    /// <summary>
    /// Provides extension methods for defragmenting SQL Server database.
    /// </summary>
    public static class DbConnectionDefragmentExtensions
    {
        /// <summary>
        /// Performs defragmentation to a provided SQL Server database connection. 
        /// Allows online rebuild only if the database server is Enterprise edition.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="onlineRebuild"></param>
        /// <returns></returns>
        public static async Task DefragmentAsync(this IDbConnection db, bool onlineRebuild = false)
        {
            var stopwatch = Stopwatch.StartNew();

            // First do a full-database index scan
            var scans = await db.QueryAsync<IndexScan>(@"SELECT DISTINCT
	s.name as [Schema],
	t.name as [Table],
	ind.name as [Index],
	indexstats.page_count as [PageCount],
	indexstats.avg_fragmentation_in_percent as [FragmentRate],
	ind.fill_factor as [FillFactor],
	col.is_identity as [IsIdentity]
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'DETAILED') indexstats 
JOIN sys.indexes ind ON indexstats.object_id = ind.object_id and indexstats.index_id = ind.index_id
JOIN sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 
JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id 
JOIN sys.tables t ON ind.object_id = t.object_id 
JOIN sys.schemas s on s.schema_id = t.schema_id
WHERE t.is_ms_shipped = 0
	AND NOT (t.name = 'sysdiagrams')
	AND indexstats.page_count > 1000
	AND indexstats.avg_fragmentation_in_percent > 10");

            Log.Information("SQL Server: Scanning indexes with > 10% fragmentation rate, with > 1000 pages count. ({ElapsedMilliseconds}ms)", stopwatch.ElapsedMilliseconds);

            if (scans.Any() == false)
            {
                Log.Information("SQL Server: No defragmentation required.");
                return;
            }

            // Build commands, based on scan results. Execute the commands in-loop because ExecuteNonQuery does NOT understand GO statement separator!
            foreach (var scan in scans)
            {
                stopwatch.Restart();
                if (scan.FragmentRate > 30)
                {
                    // rebuild
                    var ff = scan.CalculateFillFactor();
                    if (onlineRebuild)
                    {
                        await db.ExecuteAsync($"ALTER INDEX [{scan.Index}] ON [{scan.Schema}].[{scan.Table}] REBUILD WITH (FILLFACTOR = {ff}, ONLINE = ON)");
                    }
                    else
                    {
                        await db.ExecuteAsync($"ALTER INDEX [{scan.Index}] ON [{scan.Schema}].[{scan.Table}] REBUILD WITH (FILLFACTOR = {ff})");
                    }

                    Log.Information("Index {Index} on table {Schema}.{Table} is heavily fragmented at {FragmentRate}%, with {PageCount} pages. Rebuilding with fill factor: {FillFactor}%. ({ElapsedMilliseconds} ms)",
                        scan.Index, scan.Schema, scan.Table, scan.FragmentRate, scan.PageCount, ff, stopwatch.ElapsedMilliseconds
                        );
                }
                else
                {
                    //reorganize
                    await db.ExecuteAsync($"ALTER INDEX [{scan.Index}] ON [{scan.Schema}].[{scan.Table}] REORGANIZE");

                    Log.Information("Index {Index} on table {Schema}.{Table} is fragmented at {FragmentRate}%, with {PageCount} pages. Reorganizing. ({ElapsedMilliseconds} ms)",
                        scan.Index, scan.Schema, scan.Table, scan.FragmentRate, scan.PageCount, stopwatch.ElapsedMilliseconds
                        );
                }
            }
        }
    }
}
