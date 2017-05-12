SELECT DISTINCT
	s.name as [Schema],
	t.name as [Table],
	ind.name as [Index],
	indexstats.page_count as [PageCount],
	indexstats.avg_fragmentation_in_percent as [Fragment],
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
	AND indexstats.avg_fragmentation_in_percent > 10

/*
http://sqlmag.com/blog/what-best-value-fill-factor-index-fill-factor-and-performance-part-2

if PageCount < 1000, do nothing
if Fragment < 10, do nothing
if Fragment 10-30, reorganize
if Fragment 30+, rebuild
when rebuilding, if IsIdentity: FillFactor = 100, else FillFactor = 90

ALTER INDEX IX_Employee_Name ON dbo.Employee REORGANIZE
GO

ALTER INDEX IX_Employee_Name ON dbo.Employee REBUILD WITH (FILLFACTOR = 90)
GO
*/