using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace Yasinovsky.MailSender.Core.Models.Base
{
    public class CustomCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public CustomCommand([NotNull] Action execute, Func<bool> canExecute = null) : 
            this((o) => execute.Invoke(),
                canExecute is null ? null : o => canExecute.Invoke())
        {

        }

        public CustomCommand([NotNull] Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute is null || _canExecute.Invoke(parameter);

        public void Execute(object parameter) => _execute.Invoke(parameter);

        public virtual event EventHandler CanExecuteChanged;
    }
}