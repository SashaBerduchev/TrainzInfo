using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace TrainzInfo.Tools
{
    public class BlockingInterceptor : DbCommandInterceptor
    {
        //public override InterceptionResult<DbDataReader> ReaderExecuting(
        //DbCommand command,
        //CommandEventData eventData,
        //InterceptionResult<DbDataReader> result)
        //{
        //    Log.BlockingLog(command.CommandText); // твой метод логирования
        //    return base.ReaderExecuting(command, eventData, result);
        //}

        //public override InterceptionResult<int> NonQueryExecuting(
        //    DbCommand command,
        //    CommandEventData eventData,
        //    InterceptionResult<int> result)
        //{
        //    Log.BlockingLog(command.CommandText);
        //    return base.NonQueryExecuting(command, eventData, result);
        //}

        //// Scalar-запросы (например SELECT COUNT(*) )
        //public override InterceptionResult<object> ScalarExecuting(
        //    DbCommand command,
        //    CommandEventData eventData,
        //    InterceptionResult<object> result)
        //{
        //    Log.BlockingLog(command.CommandText);
        //    return base.ScalarExecuting(command, eventData, result);
        //}

        //// --- АСИНХРОННІ МЕТОДИ (Це те, чого не вистачало) ---

        //public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        //    DbCommand command,
        //    CommandEventData eventData,
        //    InterceptionResult<DbDataReader> result,
        //    CancellationToken cancellationToken = default)
        //{
        //    LogCommand(command);
        //    return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        //}

        //public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(
        //    DbCommand command,
        //    CommandEventData eventData,
        //    InterceptionResult<int> result,
        //    CancellationToken cancellationToken = default)
        //{
        //    LogCommand(command);
        //    return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
        //}

        //public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(
        //    DbCommand command,
        //    CommandEventData eventData,
        //    InterceptionResult<object> result,
        //    CancellationToken cancellationToken = default)
        //{
        //    LogCommand(command);
        //    return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        //}

        //// Виніс логіку в окремий метод, щоб не дублювати код
        //private void LogCommand(DbCommand command)
        //{
        //    // Тут можна додати перевірку на null або форматування
        //    if (command != null && !string.IsNullOrWhiteSpace(command.CommandText))
        //    {
        //        Log.BlockingLog(command.CommandText);

        //        // Порада: command.CommandText покаже лише SQL із параметрами (наприклад @p0)
        //        // Щоб побачити значення, треба перебирати command.Parameters
        //    }
        //}

        // --- СИНХРОННИЙ Failed ---
        public override void CommandFailed(
            DbCommand command,
            CommandErrorEventData eventData)
        {
            AnalyzeError(command, eventData.Exception, eventData.Duration);
            base.CommandFailed(command, eventData);
        }

        // --- АСИНХРОННИЙ Failed ---
        public override Task CommandFailedAsync(
            DbCommand command,
            CommandErrorEventData eventData,
            CancellationToken cancellationToken = default)
        {
            AnalyzeError(command, eventData.Exception, eventData.Duration);
            return base.CommandFailedAsync(command, eventData, cancellationToken);
        }

        // Логіка аналізу помилки
        private void AnalyzeError(DbCommand command, System.Exception exception, System.TimeSpan duration)
        {
            string errorType = "SQL ERROR";
            string extraInfo = "";

            // Перевіряємо, чи це специфічна помилка SQL Server
            if (exception is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 1205: // Це код DEADLOCK victim
                        errorType = "DEADLOCK DETECTED";
                        break;
                    case -2:   // Це код Timeout (Client Timeout)
                        errorType = "TIMEOUT (POSSIBLE BLOCKING)";
                        extraInfo = "(Запит чекав занадто довго, ймовірно таблиця заблокована)";
                        break;
                }
            }

            // Формуємо повідомлення
            string message = $@"
            !!! {errorType} !!!
            Time: {duration.TotalSeconds} sec
            Query: {command.CommandText}
            Error: {exception.Message}
            {extraInfo}";

            // Логуємо
            Log.BlockingLog(message);
        }
    }
}
