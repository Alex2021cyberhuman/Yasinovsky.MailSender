using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Yasinovsky.MailSender.Core.Models.Base;

namespace Yasinovsky.MailSender.WpfApplication.Models.Base
{
    public class WpfCustomCommand : CustomCommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public WpfCustomCommand([NotNull] Action execute, Func<bool> canExecute = null) : 
            this((o) => execute.Invoke(),
                canExecute is null ? null : o => canExecute.Invoke())
        { }

        public WpfCustomCommand([NotNull] Action<object> execute, Func<object, bool> canExecute = null)
            : base(execute, canExecute)
        { }

        public override event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}