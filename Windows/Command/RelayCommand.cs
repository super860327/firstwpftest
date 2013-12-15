using System;
using System.Diagnostics;
using System.Windows.Input;
namespace IDKin.IM.Windows.Command
{
	public class RelayCommand : ICommand
	{
		private readonly System.Action<object> _execute;
		private readonly System.Predicate<object> _canExecute;
		public event System.EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}
		public RelayCommand(System.Action<object> execute) : this(execute, null)
		{
		}
		public RelayCommand(System.Action<object> execute, System.Predicate<object> canExecute)
		{
			if (execute == null)
			{
				throw new System.ArgumentNullException("execute");
			}
			this._execute = execute;
			this._canExecute = canExecute;
		}
		[System.Diagnostics.DebuggerStepThrough]
		public bool CanExecute(object parameter)
		{
			return this._canExecute == null || this._canExecute(parameter);
		}
		public void Execute(object parameter)
		{
			this._execute(parameter);
		}
	}
}
