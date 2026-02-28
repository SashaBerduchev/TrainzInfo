using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Threading.Tasks;

namespace TrainzInfo.Tools.DB
{
    public static class DbContextExtensions
    {
        public static async Task ExecuteInTransactionAsync(
        this DbContext context,
        Func<Task> action,
        IsolationLevel isolationLevel)
        {
            // Если транзакция уже запущена выше по стеку, просто выполняем код
            if (context.Database.CurrentTransaction != null)
            {
                await action();
                return;
            }

            using var transaction = await context.Database.BeginTransactionAsync(isolationLevel);
            try
            {
                await action();
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw; // Пробрасываем ошибку дальше
            }
        }
    }
}
