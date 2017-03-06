using MvvmCross.Platform.Platform;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Xmf2.Commons.MvxExtends.Helpers
{
	public class NavigationJsonConverter : IMvxJsonConverter
	{
		private static readonly JsonSerializerSettings Settings;

		static NavigationJsonConverter()
		{
			Settings = new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
				DateFormatHandling = DateFormatHandling.IsoDateFormat
			};
		}

		public T DeserializeObject<T>(string inputText)
		{
			return JsonConvert.DeserializeObject<T>(inputText, Settings);
		}

		public string SerializeObject(object toSerialise)
		{
			return JsonConvert.SerializeObject(toSerialise, Formatting.None, Settings);
		}

		public object DeserializeObject(Type type, string inputText)
		{
			return JsonConvert.DeserializeObject(inputText, type, Settings);
		}

		public T DeserializeObject<T>(Stream stream)
		{
			var serializer = JsonSerializer.Create(Settings);

			using (var sr = new StreamReader(stream))
			using (var jsonTextReader = new JsonTextReader(sr))
			{
				return serializer.Deserialize<T>(jsonTextReader);
			}
		}
	}
}
