using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accelist.SDK.SQL.Defragment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace Dapper
{
    /// <summary>
    /// A gallery of extension methods for working with Entity Framework Core database context.
    /// </summary>
    public static class AccelistDbContextExtensions
    {
        /// <summary>
        /// Helper extension method for logging Dapper query via Entity Framework Core database context using Serilog.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="commandText"></param>
        /// <param name="elapsedMilliseconds"></param>
        private static void LogDapperQuery(DbContext dbContext, string commandText, long elapsedMilliseconds)
        {
            // This technique will cause the log to not be logged into loggers other than Serilog.
            // But eh, we're only using Serilog anyway so...
            Log.Information("Dapper via Entity Framework Core database context {DbContext}: ({ElapsedMilliseconds} ms) {CommandText}", dbContext.GetType().Name, elapsedMilliseconds, commandText);
        }

        /// <summary>
        /// Synchronously sends parameterized SQL non-result query to the underlying database connection of the Entity Framework Core database context using Dapper,
        /// returning the number of rows affected by the query.
        /// Allows passing object array as query parameter, for bulk operation using the same command text against each object.
        /// Automatically captures ongoing database context transaction to be passed into the query method.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static int Execute(this DbContext dbContext, string commandText, object param = null, CommandType? commandType = null)
        {
            var stopwatch = Stopwatch.StartNew();

            var db = dbContext.Database.GetDbConnection();
            var transaction = dbContext.Database.CurrentTransaction?.GetDbTransaction();

            var result = db.Execute(
                sql: commandText,
                param: param,
                transaction: transaction,
                commandType: commandType
            );

            LogDapperQuery(dbContext, commandText, stopwatch.ElapsedMilliseconds);
            return result;
        }

        /// <summary>
        /// Asynchronously sends parameterized SQL non-result query to the underlying database connection of the Entity Framework Core database context using Dapper,
        /// returning the number of rows affected by the query.
        /// Allows passing object array as query parameter, for bulk operation using the same command text against each object.
        /// Automatically captures ongoing database context transaction to be passed into the query method.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteAsync(this DbContext dbContext, string commandText, object param = null, CommandType? commandType = null)
        {
            var stopwatch = Stopwatch.StartNew();

            var db = dbContext.Database.GetDbConnection();
            var transaction = dbContext.Database.CurrentTransaction?.GetDbTransaction();

            var result = await db.ExecuteAsync(
                sql: commandText,
                param: param,
                transaction: transaction,
                commandType: commandType
            );

            LogDapperQuery(dbContext, commandText, stopwatch.ElapsedMilliseconds);
            return result;
        }

        /// <summary>
        /// Synchronously sends parameterized SQL query to the underlying database connection of the Entity Framework Core database context using Dapper,
        /// returning the buffered result as an explicitly typed List.
        /// Automatically captures ongoing database context transaction to be passed into the query method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static List<T> Query<T>(this DbContext dbContext, string commandText, object param = null, CommandType? commandType = null)
        {
            var stopwatch = Stopwatch.StartNew();

            var db = dbContext.Database.GetDbConnection();
            var transaction = dbContext.Database.CurrentTransaction?.GetDbTransaction();

            var result = db.Query<T>(
                sql: commandText,
                param: param,
                transaction: transaction,
                commandType: commandType
            );

            LogDapperQuery(dbContext, commandText, stopwatch.ElapsedMilliseconds);
            return result.AsList();
        }

        /// <summary>
        /// Asynchronously sends parameterized SQL query to the underlying database connection of the Entity Framework Core database context using Dapper,
        /// returning the buffered result as an explicitly typed List.
        /// Automatically captures ongoing database context transaction to be passed into the query method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<List<T>> QueryAsync<T>(this DbContext dbContext, string commandText, object param = null, CommandType? commandType = null)
        {
            var stopwatch = Stopwatch.StartNew();

            var db = dbContext.Database.GetDbConnection();
            var transaction = dbContext.Database.CurrentTransaction?.GetDbTransaction();

            var result = await db.QueryAsync<T>(
                sql: commandText,
                param: param,
                transaction: transaction,
                commandType: commandType
            );

            LogDapperQuery(dbContext, commandText, stopwatch.ElapsedMilliseconds);
            return result.AsList();
        }

        /// <summary>
        /// Performs whole index defragmentation to an SQL Server database context. 
        /// Allows online rebuild only if the SQL Server instance is Enterprise edition.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="onlineRebuild"></param>
        /// <returns></returns>
        public static Task DefragmentAsync(this DbContext db, bool onlineRebuild = false)
        {
            Log.Information("Attempting to defragment SQL Server database via Entity Framework Core database context {DbContext}.", db.GetType().Name);
            return db.Database.GetDbConnection().DefragmentAsync(onlineRebuild);
        }
    }
}
