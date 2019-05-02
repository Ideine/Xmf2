using System;
using System.Collections.Generic;
using Plugin.Settings.Abstractions;

namespace Xmf2.Core.Settings
{
	public interface IInMemorySettings : ISettings
	{
	
	}
	public class InMemorySettingsImplementation : IInMemorySettings
	{
		private Dictionary<string, object> _noFileNameKvpDic;
		private Dictionary<string, Dictionary<string, object>> _byFilenameKvpDic;

		public InMemorySettingsImplementation()
		{
			_noFileNameKvpDic = new Dictionary<string, object>();
			_byFilenameKvpDic = new Dictionary<string, Dictionary<string, object>>();
		}

		private Dictionary<string, object> AddOrKvpDic(string filename)
		{
			if (filename is null)
			{
				return _noFileNameKvpDic;
			}
			else if (_byFilenameKvpDic.TryGetValue(filename, out var kvpDic))
			{
				return kvpDic;
			}
			else
			{
				var newDic = new Dictionary<string, object>();
				_byFilenameKvpDic.Add(filename, newDic);
				return newDic;
			}
		}

		private bool AddOrUpdateValueInternal<T>(string key, T value, string fileName = null)
		{
			AddOrKvpDic(fileName)[key] = value;
			return true;
		}

		public bool AddOrUpdateValue(string key, decimal value, string fileName = null) => AddOrUpdateValueInternal(key, value, fileName);
		public bool AddOrUpdateValue(string key, bool value, string fileName = null) => AddOrUpdateValueInternal(key, value, fileName);
		public bool AddOrUpdateValue(string key, long value, string fileName = null) => AddOrUpdateValueInternal(key, value, fileName);
		public bool AddOrUpdateValue(string key, string value, string fileName = null) => AddOrUpdateValueInternal(key, value, fileName);
		public bool AddOrUpdateValue(string key, int value, string fileName = null) => AddOrUpdateValueInternal(key, value, fileName);
		public bool AddOrUpdateValue(string key, float value, string fileName = null) => AddOrUpdateValueInternal(key, value, fileName);
		public bool AddOrUpdateValue(string key, DateTime value, string fileName = null) => AddOrUpdateValueInternal(key, value, fileName);
		public bool AddOrUpdateValue(string key, Guid value, string fileName = null) => AddOrUpdateValueInternal(key, value, fileName);
		public bool AddOrUpdateValue(string key, double value, string fileName = null) => AddOrUpdateValueInternal(key, value, fileName);

		public void Clear(string fileName = null) => AddOrKvpDic(fileName).Clear();
		public bool Contains(string key, string fileName = null) => AddOrKvpDic(fileName).ContainsKey(key);

		private T GetValueOrDefaultInternal<T>(string key, T defaultValue, string fileName = null)
		{
			return AddOrKvpDic(fileName).TryGetValue(key, out var rawValue)
			 	&& rawValue is T castedValue
				? castedValue
				: defaultValue;
		}

		public decimal GetValueOrDefault(string key, decimal defaultValue, string fileName = null) => GetValueOrDefaultInternal(key, defaultValue, fileName);
		public bool GetValueOrDefault(string key, bool defaultValue, string fileName = null) => GetValueOrDefaultInternal(key, defaultValue, fileName);
		public long GetValueOrDefault(string key, long defaultValue, string fileName = null) => GetValueOrDefaultInternal(key, defaultValue, fileName);
		public string GetValueOrDefault(string key, string defaultValue, string fileName = null) => GetValueOrDefaultInternal(key, defaultValue, fileName);
		public int GetValueOrDefault(string key, int defaultValue, string fileName = null) => GetValueOrDefaultInternal(key, defaultValue, fileName);
		public float GetValueOrDefault(string key, float defaultValue, string fileName = null) => GetValueOrDefaultInternal(key, defaultValue, fileName);
		public DateTime GetValueOrDefault(string key, DateTime defaultValue, string fileName = null) => GetValueOrDefaultInternal(key, defaultValue, fileName);
		public Guid GetValueOrDefault(string key, Guid defaultValue, string fileName = null) => GetValueOrDefaultInternal(key, defaultValue, fileName);
		public double GetValueOrDefault(string key, double defaultValue, string fileName = null) => GetValueOrDefaultInternal(key, defaultValue, fileName);

		public bool OpenAppSettings()
		{
			throw new NotImplementedException();
		}

		public void Remove(string key, string fileName = null)
		{
			var kvpDic = AddOrKvpDic(fileName);
			if (kvpDic.ContainsKey(key))
			{
				kvpDic.Remove(key);
			}
		}
	}
}
