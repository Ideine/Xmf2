using System;
using System.Diagnostics;
using System.IO;
using Foundation;
using Newtonsoft.Json;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Core.iOS.Services
{
	[DebuggerDisplay("{GetDebuggerDisplay()}")]
	public class JsonNotificationWriter : IDisposable
	{
		private readonly Xmf2Disposable _disposable = new Xmf2Disposable();
		private readonly StringWriter _textWriter;
		private readonly JsonWriter _jsonWriter;

		public JsonNotificationWriter(Formatting formatting = Formatting.None)
		{
			_textWriter = new StringWriter().DisposeWith(_disposable);
			_jsonWriter = new JsonTextWriter(_textWriter)
			{
				Formatting = formatting
			}.DisposeWith(_disposable);
		}

		public override string ToString()
		{
			if (_disposedValue)
			{
				throw new ObjectDisposedException(nameof(JsonNotificationWriter));
			}
			return _disposedValue.ToString();
		}

		protected virtual string GetDebuggerDisplay()
		{
			try
			{
				return $"{nameof(JsonNotificationWriter)} {_textWriter.ToString()}";
			}
			catch
			{
				return this.ToString();
			}
		}

		private void Write(NSDictionary dictionary)
		{
			_jsonWriter.WriteStartObject();

			foreach (NSObject key in dictionary.Keys)
			{
				_jsonWriter.WritePropertyName(key.ToString());
				NSObject value = dictionary[key];

				WriteObject(value);
			}

			_jsonWriter.WriteEndObject();
		}

		private void Write(NSArray array)
		{
			_jsonWriter.WriteStartArray();

			for (nuint i = 0; i < array.Count; ++i)
			{
				try
				{
					NSString value = array.GetItem<NSString>(i);

					WriteObject(value);
				}
				catch (Exception)
				{
					//ignored by design
				}
			}

			_jsonWriter.WriteEndArray();
		}

		public void WriteObject(NSObject value)
		{
			if (value is NSDictionary childDictionary)
			{
				Write(childDictionary);
			}
			else if (value is NSArray childArray)
			{
				Write(childArray);
			}
			else
			{
				_jsonWriter.WriteValue(value.ToString());
			}
		}

		#region IDisposable Support

		private bool _disposedValue = false; // Pour détecter les appels redondants

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					_disposable.Dispose();
				}
				_disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion  IDisposable Support
	}
}