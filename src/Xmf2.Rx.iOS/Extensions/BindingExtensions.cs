using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;

// ReSharper disable once CheckNamespace => Extension class
namespace UIKit
{
	public static class BindingExtensions
	{
		public static IDisposable OnClickCommand(this UIControl button, Func<ICommand> getCommand)
		{
			return button.Events().TouchUpInside
				.OnTaskThread()
				.Subscribe(_ => getCommand()?.TryExecute());
		}
	}
}