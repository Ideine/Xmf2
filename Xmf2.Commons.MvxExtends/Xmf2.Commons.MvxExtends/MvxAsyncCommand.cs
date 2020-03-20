
using System;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.ExtensionMethods;

namespace Xmf2.Commons.MvxExtends
{
	public class MvxAsyncCommand : MvxCommandBase, IMvxCommand
	{
		private readonly Func<bool> _canExecute;
		private readonly Func<Task> _execute;

		public MvxAsyncCommand(Func<Task> execute) : this(execute, null) { }

		public MvxAsyncCommand(Func<Task> execute, Func<bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

		public bool CanExecute() => CanExecute(null);

		public async void Execute(object parameter)
		{
			if (CanExecute(parameter))
			{
				await _execute().ConfigureAwait(false);
			}
		}

		public void Execute() => Execute(null);

		public async Task ExecuteAsync(object parameter)
		{
			if (CanExecute(parameter))
			{
				await _execute().ConfigureAwait(false);
			}
		}

		public async Task ExecuteAsync() => await ExecuteAsync(null).ConfigureAwait(false);
	}

	public class MvxAsyncCommand<T> : MvxCommandBase, IMvxCommand
	{
		private readonly Func<T, bool> _canExecute;
		private readonly Func<T, Task> _execute;

		public MvxAsyncCommand(Func<T, Task> execute) : this(execute, null) { }

		public MvxAsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute((T)(typeof(T).MakeSafeValueCore(parameter)));
		}

		public bool CanExecute() => CanExecute(null);

		public async void Execute(object parameter)
		{
			if (CanExecute(parameter))
			{
				await _execute((T)(typeof(T).MakeSafeValueCore(parameter))).ConfigureAwait(false);
			}
		}

		public void Execute() => Execute(null);
	}
}
