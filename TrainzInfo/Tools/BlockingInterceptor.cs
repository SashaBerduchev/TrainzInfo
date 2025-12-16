using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace TrainzInfo.Tools
{
    public class BlockingInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
        {
            Log.BlockingLog(command.CommandText); // твой метод логирования
            return base.ReaderExecuting(command, eventData, result);
        }

        public override InterceptionResult<int> NonQueryExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<int> result)
        {
            Log.BlockingLog(command.CommandText);
            return base.NonQueryExecuting(command, eventData, result);
        }

        // Scalar-запросы (например SELECT COUNT(*) )
        public override InterceptionResult<object> ScalarExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<object> result)
        {
            Log.BlockingLog(command.CommandText);
            return base.ScalarExecuting(command, eventData, result);
        }
    }
}
