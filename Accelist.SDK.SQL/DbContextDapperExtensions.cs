using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace Dapper
{
    public static class DbContextDapperExtensions
    {
        public static void LogDapperQuery(DbContext db, string commandText, long elapsedMilliseconds)
        {
            // This technique will cause the log to not be logged into loggers other than Serilog.
            // But eh, we're using Serilog anyway so...
            Log.Information("Dapper Query via {DbContext} ({ElapsedMilliseconds}ms) {CommandText}", db.GetType().Name, elapsedMilliseconds, commandText);
        }

        public static int Execute(this DbContext db, string commandText, object param = null, CommandType? commandType = null)
        {
            var stopwatch = Stopwatch.StartNew();

            var con = db.Database.GetDbConnection();
            var transaction = db.Database.CurrentTransaction?.GetDbTransaction();

            var result = con.Execute(
                sql: commandText,
                param: param,
                transaction: transaction,
                commandType: commandType
            );

            LogDapperQuery(db, commandText, stopwatch.ElapsedMilliseconds);
            return result;
        }

        public static async Task<int> ExecuteAsync(this DbContext db, string commandText, object param = null, CommandType? commandType = null)
        {
            var stopwatch = Stopwatch.StartNew();

            var con = db.Database.GetDbConnection();
            var transaction = db.Database.CurrentTransaction?.GetDbTransaction();

            var result = await con.ExecuteAsync(
                sql: commandText,
                param: param,
                transaction: transaction,
                commandType: commandType
            );

            LogDapperQuery(db, commandText, stopwatch.ElapsedMilliseconds);
            return result;
        }

        public static List<T> Query<T>(this DbContext db, string commandText, object param = null, CommandType? commandType = null)
        {
            var stopwatch = Stopwatch.StartNew();

            var con = db.Database.GetDbConnection();
            var transaction = db.Database.CurrentTransaction?.GetDbTransaction();

            var result = con.Query<T>(
                sql: commandText,
                param: param,
                transaction: transaction,
                commandType: commandType
            );

            LogDapperQuery(db, commandText, stopwatch.ElapsedMilliseconds);
            return result.AsList();
        }

        public static async Task<List<T>> QueryAsync<T>(this DbContext db, string commandText, object param = null, CommandType? commandType = null)
        {
            var stopwatch = Stopwatch.StartNew();

            var con = db.Database.GetDbConnection();
            var transaction = db.Database.CurrentTransaction?.GetDbTransaction();

            var result = await con.QueryAsync<T>(
                sql: commandText,
                param: param,
                transaction: transaction,
                commandType: commandType
            );
            
            LogDapperQuery(db, commandText, stopwatch.ElapsedMilliseconds);
            return result.AsList();
        }
    }
}
