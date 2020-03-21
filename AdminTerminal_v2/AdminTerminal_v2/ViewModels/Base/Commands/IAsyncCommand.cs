using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdminTerminal_v2
{
    /// <summary>
    /// An Interface to execute async tasks with a command
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// The task to run
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync();

        /// <summary>
        /// The checker
        /// </summary>
        /// <returns></returns>
        bool CanExecute();
    }
}
