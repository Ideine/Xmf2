using System;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Xmf2.Commons.MvxExtends.Interactions
{
	public class PopupMenuRequest
    {
        public class Item
        {
            public Item(string title, int order, ICommand command)
            {
                this.Title = title;
                this.Order = order;
                this.Command = new WeakReference<ICommand>(command);
            }

            public Item(string title, int order, ICommand command, object commandParameter)
                : this(title, order, command)
            {
                this.CommentParameter = new WeakReference(commandParameter);
            }

            public string Title { get; private set; }
            public int Order { get; private set; }
            public WeakReference<ICommand> Command { get; private set; }
            public WeakReference CommentParameter { get; private set; }

            public void Clean()
            {
                this.Command = null;
            }

            public void Execute()
            {
                ICommand command;
                if (!this.Command.TryGetTarget(out command))
                    return;

                MvxAsyncCommand asyncCommand = command as MvxAsyncCommand;
                if (asyncCommand != null)
                {
                    Task.Run(() => asyncCommand.ExecuteAsync());
                }
                else
                {
                    object commandParameter = null;
                    if (this.CommentParameter != null && this.CommentParameter.IsAlive)
                        commandParameter = this.CommentParameter.Target;
                    command.Execute(commandParameter);
                }
            }
        }

        public PopupMenuRequest()
        {
            this.LstPopupItem = new List<Item>();
        }

        public List<Item> LstPopupItem { get; private set; }
        public WeakReference<ICommand> CancelCommand { get; private set; }

        public void Clean()
        {
            foreach (var item in this.LstPopupItem)
                item.Clean();
            this.LstPopupItem.Clear();
            this.CancelCommand = null;
        }

        public void Execute(int order)
        {
            if (this.LstPopupItem != null)
            {
                var item = this.LstPopupItem.FirstOrDefault(itemTmp => itemTmp.Order == order);
                if (item != null)
                    item.Execute();
            }
        }

        public void SetCancelCommand(ICommand cancelCommand)
        {
            this.CancelCommand = new WeakReference<ICommand>(cancelCommand);
        }

        public void ExecuteCancel()
        {
            ICommand command;
            if (this.CancelCommand == null || !this.CancelCommand.TryGetTarget(out command))
                return;

            MvxAsyncCommand asyncCommand = command as MvxAsyncCommand;
            if (asyncCommand != null)
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
