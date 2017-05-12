using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dapper
{
    public static class DbContextDefragmentExtensions
    {
        public static async Task DefragmentAsync(this DbContext db, bool onlineRebuild = false)
        {
            await db.Database.GetDbConnection().DefragmentAsync(onlineRebuild);
        }
    }
}
