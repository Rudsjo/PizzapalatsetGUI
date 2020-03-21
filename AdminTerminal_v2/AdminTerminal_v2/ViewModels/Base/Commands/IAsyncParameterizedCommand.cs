using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdminTerminal_v2.ViewModels.Base.Commands
{
    public interface IAsyncParameterizedCommand<T> : ICommand
    {

        Task ExecuteAsync(T parameter);
        bool CanExecute(T parameter);

    }
}
