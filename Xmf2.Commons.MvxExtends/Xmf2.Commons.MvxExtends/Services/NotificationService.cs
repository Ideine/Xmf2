﻿using System;
using System.Threading.Tasks;
using MvvmCross.Platform.Platform;

namespace Xmf2.Commons.MvxExtends.Services
{
	public interface INotificationService
	{
		void SetToken(string token);

		Task RegisterForNotification();

		Task UnregisterForNotification();
	}

	public interface IKeyValueStorageService
	{
		bool Has(string key);

		string Get(string key);

		bool Set(string key, string value);

		void Delete(string key);

		void Clear();
	}

	public interface INotificationDataService
	{
		Task<string> Register(string token, DeviceType deviceType);

		Task Unregister(string registrationId);
	}

	public enum DeviceType
	{
		Android,
		iOS
	}

	public abstract class BaseNotificationService : INotificationService
	{
		private readonly IKeyValueStorageService _settingsService;
		private readonly INotificationDataService _notificationDataService;
		private TaskCompletionSource<string> _tokenTcs;

		protected BaseNotificationService(IKeyValueStorageService settingsService, INotificationDataService notificationDataService)
		{
			_settingsService = settingsService;
			_notificationDataService = notificationDataService;
		}

		public async Task RegisterForNotification()
		{
			try
			{
				string token = await GetToken();

				if (!string.IsNullOrEmpty(token))
				{
					string registerId = await _notificationDataService.Register(token, Device);
					_settingsService.Set(nameof(INotificationService), registerId);
				}
			}
			catch (Exception ex)
			{
				MvxTrace.Trace($"Error while registering for notification: {ex}");
			}
		}

		public async Task UnregisterForNotification()
		{
			try
			{
				string registerId = _settingsService.Get(nameof(INotificationService));
				DeleteRegisterId();

				if (!string.IsNullOrEmpty(registerId))
				{
					await _notificationDataService.Unregister(registerId);
				}
			}
			catch (Exception ex)
			{
				MvxTrace.Trace($"Error while unregistering for notification: {ex}");
			}
		}

		public void SetToken(string token)
		{
			_tokenTcs?.SetResult(token);
			_tokenTcs = null;
		}

		protected Task<string> GetToken()
		{
			if (_tokenTcs != null)
			{
				return _tokenTcs.Task;
			}

			_tokenTcs = new TaskCompletionSource<string>();

			RequestToken();

			return _tokenTcs.Task;
		}

		protected virtual void DeleteRegisterId()
		{
			_settingsService.Delete(nameof(INotificationService));
		}

		protected abstract void RequestToken();

		protected abstract DeviceType Device { get; }
	}
}