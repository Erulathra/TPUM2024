using Newtonsoft.Json;

namespace ServerPresentation
{
	internal class JsonSerializer : Serializer
	{
		public override string Serialize<T>(T objectToSerialize)
		{
			return JsonConvert.SerializeObject(objectToSerialize);
		}

		public override T Deserialize<T>(string message)
		{
			return JsonConvert.DeserializeObject<T>(message);
		}
	}
}