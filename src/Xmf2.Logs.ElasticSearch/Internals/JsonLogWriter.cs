using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Xmf2.Logs.ElasticSearch.Interfaces;

namespace Xmf2.Logs.ElasticSearch.Internals
{
	internal class JsonLogWriter : IObjectWriter, IArrayWriter
	{
		private readonly bool _extraLine;
		private readonly StringWriter _textWriter = new StringWriter();
		private readonly JsonWriter _jsonWriter;

		public JsonLogWriter(Formatting formatting = Formatting.None, bool extraLine = false)
		{
			this._extraLine = extraLine;

			_jsonWriter = new JsonTextWriter(_textWriter) {Formatting = formatting};
			_jsonWriter.WriteStartObject();
		}

		IArrayWriter IArrayWriter.WriteValue(string value)
		{
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value);
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteValue(double value)
		{
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value);
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteValue(long value)
		{
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value);
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteValue(bool value)
		{
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value);
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteValue(DateTime value)
		{
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value.ToString("o", CultureInfo.InvariantCulture));
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteValue(DateTimeOffset value)
		{
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value.ToString("o", CultureInfo.InvariantCulture));
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteValue(TimeSpan value)
		{
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value);
			//Console.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, string value)
		{
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value);
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, double value)
		{
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value);
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, long value)
		{
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value);
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, bool value)
		{
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value);
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, DateTime value)
		{
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value.ToString("o", CultureInfo.InvariantCulture));
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, DateTimeOffset value)
		{
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value.ToString("o", CultureInfo.InvariantCulture));
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, TimeSpan value)
		{
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value);
			//Console.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteObject(string property, Action<IObjectWriter> objectWriter)
		{
			//Console.WriteLine($"WriteObject(Action): {property}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteStartObject();

			objectWriter?.Invoke(this);

			_jsonWriter.WriteEndObject();
			//Console.WriteLine($"WriteObject(Action): {property} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteArray(string property, Action<IArrayWriter> arrayWriter)
		{
			//Console.WriteLine($"WriteArray(Action): {property}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteStartArray();

			arrayWriter?.Invoke(this);

			_jsonWriter.WriteEndArray();
			//Console.WriteLine($"WriteArray(Action): {property} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteObject(Action<IObjectWriter> objectWriter)
		{
			//Console.WriteLine($"WriteObject(Action) in array");
			_jsonWriter.WriteStartObject();

			objectWriter?.Invoke(this);

			_jsonWriter.WriteEndObject();
			//Console.WriteLine($"WriteObject(Action) in array : END");
			return this;
		}

		public override string ToString()
		{
			_jsonWriter.WriteEndObject();

			var result = _textWriter.ToString();

			if (_extraLine)
			{
				result += Environment.NewLine;
			}

			return result;
		}
	}
}