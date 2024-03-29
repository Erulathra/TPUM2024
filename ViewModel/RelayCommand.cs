using System;
using System.Windows.Input;

namespace ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action execute;

        private readonly Func<bool>? canExecute;

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool>? canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        internal void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> execute;

        private readonly Predicate<T>? canExecute;

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T>? canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        internal void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }
}