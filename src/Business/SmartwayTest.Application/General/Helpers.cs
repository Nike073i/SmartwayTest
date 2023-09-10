using System.Transactions;

namespace SmartwayTest.Application.General
{
    public static class Helpers
    {
        public static TransactionScope CreateTransactionScope(int seconds = 10)
        {
            return new TransactionScope(
                TransactionScopeOption.Required,
                new TimeSpan(0, 0, seconds),
                TransactionScopeAsyncFlowOption.Enabled
            );
        }
    }
}
