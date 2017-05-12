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

    public static class DbConnectionDefragmentExtensions
    {
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

            // Build commands, based on scan results
            var sb = new StringBuilder();
            foreach (var scan in scans)
            {
                if (scan.FragmentRate > 30)
                {
                    // rebuild
                    var ff = scan.CalculateFillFactor();
                    Log.Information("Index {Index} on table {Schema}.{Table} is heavily fragmented at {FragmentRate}%, with {PageCount} pages. Rebuilding with fill factor: {FillFactor}%.",
                        scan.Index, scan.Schema, scan.Table, scan.FragmentRate, scan.PageCount, ff
                        );
                    if (onlineRebuild)
                    {
                        sb.AppendLine($"ALTER INDEX [{scan.Index}] ON [{scan.Schema}].[{scan.Table}] REBUILD WITH (FILLFACTOR = {ff}, ONLINE = ON)");
                    }
                    else
                    {
                        sb.AppendLine($"ALTER INDEX [{scan.Index}] ON [{scan.Schema}].[{scan.Table}] REBUILD WITH (FILLFACTOR = {ff})");
                    }
                }
                else
                {
                    //reorganize
                    Log.Information("Index {Index} on table {Schema}.{Table} is fragmented at {FragmentRate}%, with {PageCount} pages. Reorganizing.",
                        scan.Index, scan.Schema, scan.Table, scan.FragmentRate, scan.PageCount
                        );
                    sb.AppendLine($"ALTER INDEX [{scan.Index}] ON [{scan.Schema}].[{scan.Table}] REORGANIZE");
                }
                sb.AppendLine("GO");
            }

            var sql = sb.ToString();

            // Execute the commands
            stopwatch.Restart();
            await db.ExecuteAsync(sql);
            Log.Information("SQL Server: Execute defragmentation commands. ({ElapsedMilliseconds}ms) {CommandText}", stopwatch.ElapsedMilliseconds, sql);
        }
    }
}
