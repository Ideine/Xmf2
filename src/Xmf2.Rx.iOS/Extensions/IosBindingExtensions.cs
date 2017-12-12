using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;

// ReSharper disable once CheckNamespace => Extension class
namespace UIKit
{
	public static class IosBindingExtensions
	{
		public static IDisposable OnClickCommand(this UIControl button, Func<ICommand> getCommand)
		{
			return button.SubscribeOnClickCommand(() => getCommand()?.TryExecute());
		}
		
		public static IDisposable SubscribeOnClickCommand(this UIControl button, Action action)
		{
			return button.Events().TouchUpInside
				.OnTaskThread()
				.Subscribe(_ => action());
		}

		public static IObservable<EventArgs> ClickObservable(this UIControl button)
		{
			return button.Events().TouchUpInside;
		}

		public static IObservable<object> ShouldReturnObservable(this UITextField input)
		{
			return Observable.FromEventPattern<UITextFieldCondition, EventArgs>(x => field =>
			{
				x.Invoke(field, EventArgs.Empty);
				return true;
			}, x => input.ShouldReturn = x, x => input.ShouldReturn = null);
		}
	}
}