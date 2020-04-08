using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminTerminal_v2
{
    public static class AsyncCommandHelpers
    {

        #pragma warning disable RECS0165 // Async methods should return a Task instead of void
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
#pragma warning disable RECS0165 // Async methods should return a Task instead of void
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
        }

    }


    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
