// ReSharper disable once CheckNamespace
namespace Newtonsoft.Json
{
	public static class JsonConvertExtensions
	{
		public static string SerializeObjectWithoutNull(this object objectToSerialize)
		{
			return JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore
			});
		}
	}
}