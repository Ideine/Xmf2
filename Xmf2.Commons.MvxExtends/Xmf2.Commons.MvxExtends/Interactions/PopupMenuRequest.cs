using System;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using MvvmCross.Commands;

namespace Xmf2.Commons.MvxExtends.Interactions
{
	public class PopupMenuRequest
	{
		public class Item
		{
			public Item(string title, int order, ICommand command)
			{
				Title = title;
				Order = order;
				Command = new WeakReference<ICommand>(command);
			}

			public Item(string title, int order, ICommand command, object commandParameter) : this(title, order, command)
			{
				CommentParameter = new WeakReference(commandParameter);
			}

			public string Title { get; private set; }
			public int Order { get; private set; }
			public WeakReference<ICommand> Command { get; private set; }
			public WeakReference CommentParameter { get; private set; }

			public void Clean()
			{
				Command = null;
			}

			public void Execute()
			{
				if (!Command.TryGetTarget(out ICommand command))
				{
					return;
				}

				if (command is MvxAsyncCommand asyncCommand)
				{
					Task.Run(() => asyncCommand.ExecuteAsync());
				}
				else
				{
					object commandParameter = null;
					if (CommentParameter != null && CommentParameter.IsAlive)
					{
						commandParameter = CommentParameter.Target;
					}

					command.Execute(commandParameter);
				}
			}
		}

		public PopupMenuRequest()
		{
			LstPopupItem = new List<Item>();
		}

		public List<Item> LstPopupItem { get; private set; }
		public WeakReference<ICommand> CancelCommand { get; private set; }

		public void Clean()
		{
			foreach (Item item in LstPopupItem)
			{
				item.Clean();
			}

			LstPopupItem.Clear();
			CancelCommand = null;
		}

		public void Execute(int order)
		{
			Item item = LstPopupItem?.FirstOrDefault(itemTmp => itemTmp.Order == order);
			item?.Execute();
		}

		public void SetCancelCommand(ICommand cancelCommand)
		{
			CancelCommand = new WeakReference<ICommand>(cancelCommand);
		}

		public void ExecuteCancel()
		{
			if (CancelCommand == null || !CancelCommand.TryGetTarget(out ICommand command))
			{
				return;
			}

			if (command is MvxAsyncCommand asyncCommand)
			{
				Task.Run(() => asyncCommand.ExecuteAsync());
			}
			else
			{
				command.Execute(null);
			}
		}
	}
}