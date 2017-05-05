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
    public static class AccelistEFCoreDapperExtensions
    {
        public static void LogDapperQuery(string commandText, long elapsedMilliseconds)
        {
            // This technique will cause the log to not be logged into loggers other than Serilog.
            // But eh, we're using Serilog anyway so...
            Log.Information("Accelist - Dapper SQL Query ({ElapsedMilliseconds}) {CommandText}", elapsedMilliseconds, commandText);
        }

        public static List<dynamic> Query(this DbContext db, string commandText, object param = null, CommandType? commandType = null)
        {
            var stopwatch = Stopwatch.StartNew();

            var con = db.Database.GetDbConnection();
            var transaction = db.Database.CurrentTransaction?.GetDbTransaction();

            var result = con.Query(
                sql: commandText,
                param: param,
                transaction: transaction,
                commandType: commandType
            );

            LogDapperQuery(commandText, stopwatch.ElapsedMilliseconds);
            return result.AsList();
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

            LogDapperQuery(commandText, stopwatch.ElapsedMilliseconds);
            return result.AsList();
        }

        public static async Task<List<dynamic>> QueryAsync(this DbContext db, string commandText, object param = null, CommandType? commandType = null)
        {
            var stopwatch = Stopwatch.StartNew();

            var con = db.Database.GetDbConnection();
            var transaction = db.Database.CurrentTransaction?.GetDbTransaction();

            var result = await con.QueryAsync(
                sql: commandText,
                param: param,
                transaction: transaction,
                commandType: commandType
            );

            LogDapperQuery(commandText, stopwatch.ElapsedMilliseconds);
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

            LogDapperQuery(commandText, stopwatch.ElapsedMilliseconds);
            return result.AsList();
        }
    }
}
