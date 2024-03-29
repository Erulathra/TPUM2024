namespace ServerPresentation
{
	public abstract class Serializer
	{
		public abstract string Serialize<T>(T objectToSerialize);
		public abstract T Deserialize<T>(string message);

		public static Serializer Create()
		{
			return new JsonSerializer();
		}
	}
}