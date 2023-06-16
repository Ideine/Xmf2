using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xmf2.Authentications.OAuth2.Authentication
{
	public interface ILogoutCallbackService
	{
		void RegisterForLogout(Action action);

		void RegisterForLogoutAsync(Func<Task> action);

		Task Logout();
	}

	public class LogoutCallbackService : ILogoutCallbackService
	{
		private readonly List<Action> _syncActions = new List<Action>();
		private readonly List<Func<Task>> _asyncActions = new List<Func<Task>>();

		public void RegisterForLogout(Action action) => _syncActions.Add(action);

		public void RegisterForLogoutAsync(Func<Task> action) => _asyncActions.Add(action);

		public async Task Logout()
		{
			//TODO VJU 16/06/2023 : quid en cas de crash ?

			foreach (Action action in _syncActions)
			{
				action();
			}

			foreach (Func<Task> action in _asyncActions)
			{
				await action();
			}
		}
	}
}